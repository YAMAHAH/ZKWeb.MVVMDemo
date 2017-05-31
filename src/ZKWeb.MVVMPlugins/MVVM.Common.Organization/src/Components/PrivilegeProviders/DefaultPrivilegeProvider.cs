using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.ActionFilters;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.Attributes;
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
namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.PrivilegeProviders
{
    /// <summary>
    /// 默认的权限提供器
    /// </summary>
    [ExportMany]
    public class DefaultPrivilegeProvider : IPrivilegesProvider
    {
        /// <summary>
        /// 查找应用服务中的权限并返回
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetPrivileges()
        {
            var applicationServices = ZKWeb.Application.Ioc.ResolveMany<IApplicationService>();
            //功能
            var tempActions = applicationServices.Select(a => new TempActionInfo()
            {
                TempClassType = a.GetType(),
                TempActions = a.GetType().GetTypeInfo().GetMethods().Where(m => m.GetCustomAttribute<TempActionAttribute>() != null)
                              .Select(m => m.GetCustomAttribute<TempActionAttribute>()).ToList()
                    ?? new List<TempActionAttribute>()
            }).Where(t => t.TempActions.Count() > 0).ToList();

            //数据字段
            var dataFieldPrivileges = applicationServices.Select(a => new TempModelInfo()
            {
                TempClassType = a.GetType(),
                TempDataFields = a.GetType().GetTypeInfo().GetCustomAttribute<TempModelAttribute>()
                            ?.TemplateModels.SelectMany(t => t.GetTypeInfo().GetProperties())
                            .SelectMany(p => p.GetCustomAttributes<TempDataFieldAttribute>()) ?? new List<TempDataFieldAttribute>()
            }).SelectMany(m =>
            {
                m.TempDataFields.ForEach(t => t.TempClassType = m.TempClassType);
                return m.TempDataFields;
            }).ToList();

            //模板过滤器
            var tempFilters = applicationServices.Where(a => a.GetType().GetTypeInfo().GetCustomAttribute<TempFilterAttribute>() != null)
                .Select(a => new TempFilterInfo()
                {
                    TempClassType = a.GetType(),
                    TempFilters = a.GetType().GetTypeInfo().GetCustomAttribute<TempFilterAttribute>()
                               ?.FilterTypes.Where(t => typeof(IEntityQueryFilter).IsAssignableFrom(t)
                               || typeof(IEntityOperationFilter).IsAssignableFrom(t)).ToArray() ?? new Type[] { }
                }).ToList();
            this.MainView();
            return applicationServices
                .SelectMany(a => a.GetApiMethods())
                .SelectMany(m => m.Attributes.OfType<CheckPrivilegeAttribute>())
                .SelectMany(a => a.RequirePrivileges)
                .Distinct();
        }

        private StringBuilder _properties = new StringBuilder();

        public void MainView()
        {
            TraversalProperties(typeof(MultiTenant.src.Application.Dtos.TenantOutputDto));

            File.WriteAllText(@"d:\Properties.txt", _properties.ToString());
        }


        private void TraversalProperties(Type tempClsType)
        {
            if (null == tempClsType){  return; }
            foreach (PropertyInfo pi in tempClsType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                PropertyInfo needUpdateValue = tempClsType.GetProperty(pi.Name);

                if (needUpdateValue.PropertyType.Equals(tempClsType))
                {
                    return;
                }

                if (needUpdateValue.PropertyType.IsArray
                                    || (needUpdateValue.PropertyType.GetTypeInfo().IsClass
                                        && !needUpdateValue.PropertyType.GetTypeInfo().IsGenericType
                                        && !needUpdateValue.PropertyType.Equals(typeof(String))
                                        && !needUpdateValue.PropertyType.GetTypeInfo().IsValueType
                                        )
                                    )
                {
                    TraversalProperties(needUpdateValue.PropertyType);
                }
                else if (needUpdateValue.PropertyType.GetTypeInfo().IsGenericType
                                    && needUpdateValue.PropertyType.GetGenericTypeDefinition() == typeof(ObservableCollection<>))
                {
                    TraversalProperties(needUpdateValue.PropertyType.GetGenericArguments()[0]);
                }
                else
                {
                    if (tempClsType.Name.Contains("TenantOutputDto"))
                    {
                        _properties.AppendFormat("\"{0}\", ", needUpdateValue.Name);
                    }
                    else
                    {
                        _properties.AppendFormat("\"{0}_{1}\", ", tempClsType.Name.Replace("OutputDto", ""), needUpdateValue.Name);
                    }
                }
            }
        }

    }
}
