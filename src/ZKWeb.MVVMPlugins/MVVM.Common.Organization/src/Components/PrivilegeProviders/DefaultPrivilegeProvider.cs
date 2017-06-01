using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.ActionFilters;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.Attributes;
using ZKWeb.MVVMPlugins.SimpleEasy.Business.Product.src.Domain.Entities;
using ZKWeb.Plugins.MVVM.Common.Organization.src.Components.PrivilegeProviders.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

public class TempModelInfo
{
    public Type TempClassType { get; set; }
    public IEnumerable<TempDataFieldAttribute> TempDataFields { get; set; }
}
public class TempActionInfo
{
    public Type TempClassType { get; set; }
    public IEnumerable<TempActionAttribute> TempActions { get; set; }
}

public class TempFilterInfo
{
    public Type TempClassType { get; set; }
    public IEnumerable<Type> TempFilters { get; set; }
}

public class TemplateObjectInfo
{
    public Type TempClassType { get; set; }
    public IEnumerable<TempDataFieldAttribute> TempDataFields { get; set; }
    public IEnumerable<TempActionAttribute> TempActions { get; set; }
    public IEnumerable<Type> TempFilters { get; set; }
}

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.PrivilegeProviders
{
    /// <summary>
    /// 默认的权限提供器
    /// </summary>
    [ExportMany]
    public class DefaultPrivilegeProvider : IPrivilegesProvider
    {
        public IContainer Injector { get { return ZKWeb.Application.Ioc; } }

        private IEnumerable<IApplicationService> xAppServices;
        public IEnumerable<IApplicationService> appServices
        {
            get
            {
                if (this.xAppServices == null)
                {
                    xAppServices = Injector.ResolveMany<IApplicationService>();
                }
                return this.xAppServices;
            }
        }
        /// <summary>
        /// 查找应用服务中的权限并返回
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetPrivileges()
        {
            var applicationServices = ZKWeb.Application.Ioc.ResolveMany<IApplicationService>();
            return applicationServices
                .SelectMany(a => a.GetApiMethods())
                .SelectMany(m => m.Attributes.OfType<CheckPrivilegeAttribute>())
                .SelectMany(a => a.RequirePrivileges)
                .Distinct();
        }

        private StringBuilder _properties = new StringBuilder();

        public void MainView()
        {
            //MultiTenant.src.Application.Dtos.TenantOutputDto
            TraversalProperties(typeof(Product));

            File.WriteAllText(@"Properties.txt", _properties.ToString());
        }

        private List<Type> typeLists = new List<Type>();
        /// <summary>
        /// 获取某个类型的所有属性(递归)
        /// </summary>
        /// <param name="tempClsType"></param>
        /// <returns></returns>
        private List<PropertyInfo> TraversalProperties(Type tempClsType)
        {
            var propinfos = new List<PropertyInfo>();
            if (null == tempClsType) { return propinfos; }
            foreach (PropertyInfo pi in tempClsType.GetProperties(BindingFlags.Public |
                BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                PropertyInfo propInfo = tempClsType.GetProperty(pi.Name);

                var propType = propInfo.PropertyType;
                //if (propType.Equals(tempClsType)) return null;

                if (propType.IsArray || (propType.GetTypeInfo().IsClass &&
                    !propType.GetTypeInfo().IsGenericType &&
                    !propType.Equals(typeof(String)) &&
                    !propType.GetTypeInfo().IsValueType))
                {
                    if (!this.typeLists.Contains(propType))
                    {
                        this.typeLists.Add(propType);
                        propinfos.AddRange(TraversalProperties(propType));
                    }
                }
                else if (propType.GetTypeInfo().IsGenericType)
                {
                    var t = propType.GetGenericArguments()[0];
                    if (propType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                    {
                        t = propType.GetGenericArguments()[1];
                    }
                    if (t.GetTypeInfo().IsClass &&
                        !t.GetTypeInfo().IsGenericType &&
                        !t.Equals(typeof(String)) &&
                        !t.GetTypeInfo().IsValueType)
                    {
                        if (!this.typeLists.Contains(t))
                        {
                            this.typeLists.Add(t);
                            propinfos.AddRange(TraversalProperties(t));
                        }
                    }
                    else
                    {
                        _properties.AppendFormat("\"{0}_{1}\", ", tempClsType.Name.Replace("OutputDto", ""), propInfo.Name);
                        propinfos.Add(propInfo);
                    }
                }
                else
                {
                    propinfos.Add(propInfo);

                    if (tempClsType.Name.Contains("TenantOutputDto"))
                    {
                        _properties.AppendFormat("\"{0}\", ", propInfo.Name);
                    }
                    else
                    {
                        _properties.AppendFormat("\"{0}_{1}\", ", tempClsType.Name.Replace("OutputDto", ""), propInfo.Name);
                    }
                }
            }
            return propinfos;
        }

        public List<TemplateObjectInfo> GetTemplateObjects()
        {
            var tempObjects = appServices
                .Where(a => a.GetAttributeMethod<TempActionAttribute>() != null ||
                            a.GetAttribute<TempModelAttribute>() != null ||
                            a.GetAttribute<TempFilterAttribute>() != null)
                .Select(a => new TemplateObjectInfo()
                {
                    TempClassType = a.GetType(),
                    TempActions = a.GetTypeInfoEx().GetMethods()
                                    .Where(m => m.GetCustomAttribute<TempActionAttribute>() != null)
                                    .Select(m => m.GetCustomAttribute<TempActionAttribute>())
                                    .ToList(),
                    TempDataFields = a.GetAttribute<TempModelAttribute>().TemplateModels
                                        .SelectMany(t =>
                                        {
                                            this.typeLists.Clear();
                                            return this.TraversalProperties(t);
                                        })
                                        .SelectMany(p => p.GetCustomAttributes<TempDataFieldAttribute>())
                                        .ToList(),
                    TempFilters = a.GetAttribute<TempFilterAttribute>().FilterTypes
                                    .Where(t => t.IsQueryOrOperationFilter()).ToArray()
                })
                .ToList();
            return tempObjects;
        }

        public List<TempActionInfo> GetTemplateActions()
        {
            var applicationServices = appServices;
            //功能
            var tempActions = applicationServices
                .Select(a => new TempActionInfo()
                {
                    TempClassType = a.GetType(),
                    TempActions = a.GetTypeInfoEx().GetMethods()
                                    .Where(m => m.GetCustomAttribute<TempActionAttribute>() != null)
                                    .Select(m => m.GetCustomAttribute<TempActionAttribute>()).ToList()
                })
                .Where(t => t.TempActions.Count() > 0).ToList();
            return tempActions;
        }

        public List<TempModelInfo> GetTemplateDataFileds()
        {
            var applicationServices = xAppServices;

            //数据字段
            var templateDataFields = applicationServices
                .Where(a => a.GetAttribute<TempModelAttribute>() != null)
                .Select(a => new TempModelInfo()
                {
                    TempClassType = a.GetType(),
                    TempDataFields = a.GetAttribute<TempModelAttribute>().TemplateModels
                                        .SelectMany(t =>
                                        {//return t.GetTypeInfo().GetProperties();
                                            this.typeLists.Clear();
                                            return this.TraversalProperties(t);
                                        })
                                        .SelectMany(p => p.GetCustomAttributes<TempDataFieldAttribute>())
                })
                .ToList();
            //.SelectMany(m =>
            //{
            //    m.TempDataFields.ForEach(t => t.TempClassType = m.TempClassType);
            //    return m.TempDataFields;
            //})
            //.ToList();
            return templateDataFields;
        }

        public List<TempFilterInfo> GetTemplateFilters()
        {
            //模板过滤器
            var tempFilters = appServices
                .Where(a => a.GetAttribute<TempFilterAttribute>() != null)
                .Select(a => new TempFilterInfo()
                {
                    TempClassType = a.GetType(),
                    TempFilters = a.GetAttribute<TempFilterAttribute>().FilterTypes
                                    .Where(t => t.IsQueryOrOperationFilter()).ToArray()
                })
                .ToList();
            return tempFilters;
        }
    }
}
