using SimpleEasy.Core.lib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Module
{
    /// <summary>
    /// 所有ANGULAR模块的基类
    /// </summary>
    public abstract class AngularModuleBase : IAngularModule
    {
        public AngularModuleBase()
        {
            RegisterModuleTemplate();
        }
        /// <summary>
        /// 注册模块的所有模板到模块容器
        /// </summary>
        private void RegisterModuleTemplate()
        {
            var moduleTemps = EnumModuleTemplates();
            ModuleManager.RegisterModuleTemplate(GetType(), new List<Type>(moduleTemps));
        }
        public IContainer Injector
        {
            get
            {
                return ZKWeb.Application.Ioc;
            }
        }
        /// <summary>
        /// 模块管理器
        /// </summary>
        public ModuleManager ModuleManager
        {
            get
            {
                return Injector.Resolve<ModuleManager>();
            }
        }
        /// <summary>
        /// 获取模块的所有模板
        /// </summary>
        public List<Type> ModuleTemplates
        {
            get
            {
                return ModuleManager.GetModuleTemplates(GetType());
            }
        }
        private string xModuleId;
        /// <summary>
        /// 模块ID
        /// </summary>
        public string ModuleId
        {
            get
            {
                if (xModuleId == null) xModuleId = MD5Utils.GetGuidByMD5(GetType().FullName, "X2");
                return xModuleId;
            }
        }

        private List<Type> typeLists = new List<Type>();
        /// <summary>
        /// 获取某个类型的所有属性(递归)
        /// </summary>
        /// <param name="tempClsType"></param>
        /// <returns></returns>
        protected List<EnumPropInfo> TraversalProperties(Type tempClsType)
        {
            var propinfos = new List<EnumPropInfo>();
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
                        // _properties.AppendFormat("\"{0}_{1}\", ", tempClsType.Name.Replace("OutputDto", ""), propInfo.Name);
                        propinfos.Add(new EnumPropInfo() { ParentType = tempClsType, PropInfo = propInfo });
                    }
                }
                else
                {
                    propinfos.Add(new EnumPropInfo() { ParentType = tempClsType, PropInfo = propInfo });

                    if (tempClsType.Name.Contains("TenantOutputDto"))
                    {
                        // _properties.AppendFormat("\"{0}\", ", propInfo.Name);
                    }
                    else
                    {
                        // _properties.AppendFormat("\"{0}_{1}\", ", tempClsType.Name.Replace("OutputDto", ""), propInfo.Name);
                    }
                }
            }
            return propinfos;
        }
        /// <summary>
        /// 获取模块所有模板的对象信息
        /// </summary>
        /// <returns></returns>
        public List<TemplateInfo> GetTemplateObjects()
        {
            var moduleType = GetType();
            var ngModuleAttr = moduleType.GetTypeInfo().GetCustomAttribute<AngularModuleAttribute>();
            var moduleName = ngModuleAttr == null ? moduleType.Name : ngModuleAttr.ModuleName;

            var tempObjects = ModuleTemplates
                .Where(a => a.GetTypeInfo().GetCustomAttribute<TempClassAttribute>() != null)
                .Select(a =>
                {
                    var nameLists = new List<string>();
                    var tempAttr = a.GetTypeInfo().GetCustomAttribute<TempClassAttribute>();
                    if (tempAttr != null)
                    {
                        if (tempAttr.TemplateId == null) tempAttr.TemplateId = MD5Utils.GetGuidByMD5(a.FullName, "X2");
                        if (tempAttr.TempClassType == null) tempAttr.TempClassType = a;
                        if (tempAttr.TempName == null) tempAttr.TempName = a.Name.Replace("Service", "");
                    }

                    return new TemplateInfo()
                    {
                        ModuleType = moduleType,
                        ModuleId = MD5Utils.GetGuidByMD5(GetType().FullName, "X2"),
                        ModuleName = moduleName,
                        TempClassType = a,
                        TempId = tempAttr?.TemplateId,
                        TempName = tempAttr?.TempName,
                        TempActions = a.GetTypeInfo().GetMethods()
                            .Where(m => m.GetCustomAttribute<TempActionAttribute>() != null)
                            .Select(m => m.GetCustomAttribute<TempActionAttribute>())
                            .ToList(),

                        TempDataFields = tempAttr.TemplateModels
                                .SelectMany(t =>
                                {
                                    this.typeLists.Clear();
                                    return this.TraversalProperties(t);
                                })
                                .Where(p => p.PropInfo.GetCustomAttribute<TempDataFieldAttribute>() != null)
                                .SelectMany(p =>
                                {
                                    var datafield = p.PropInfo.GetCustomAttribute<TempDataFieldAttribute>();
                                    var propType = p.PropInfo.PropertyType;
                                    if (datafield != null)
                                    {
                                        if (datafield.DataType == null)
                                        {
                                            if (propType.GetTypeInfo().IsGenericType)
                                            {
                                                datafield.DataType = propType.GetGenericArguments()[0].Name;
                                            }
                                            else
                                            {
                                                datafield.DataType = propType.Name;
                                            }
                                        }
                                        if (datafield.Name == null) datafield.Name = p.PropInfo.Name;
                                        if (datafield.Alias == null) datafield.Alias = p.PropInfo.Name;
                                        if (nameLists.Contains(datafield.Alias))
                                        {
                                            datafield.Alias = p.ParentType.Name.Replace("OutputDto", "") + "_" + datafield.Name;
                                        }
                                        nameLists.Add(datafield.Alias);
                                    }
                                    return new TempDataFieldAttribute[] { datafield };
                                })
                                .ToList(),

                        TempFilters = tempAttr.FilterTypes
                    };
                })
                .ToList();
            return tempObjects;
        }
        /// <summary>
        /// 枚举模块所有的模板
        /// </summary>
        /// <returns></returns>
        public List<Type> EnumModuleTemplates()
        {
            var moduleTemps = ModuleManager.AllModuleTemplates
               .Where(t =>
               {
                   var tempClassAttr = t.GetType().GetTypeInfo().GetCustomAttribute<TempClassAttribute>();
                   return tempClassAttr != null && tempClassAttr.ModuleType == GetType();
               })
               .Select(t => t.GetType())
               .ToList();
            return moduleTemps;
        }
    }

    public class EnumPropInfo
    {
        public Type ParentType { get; set; }
        public PropertyInfo PropInfo { get; set; }
    }

}
