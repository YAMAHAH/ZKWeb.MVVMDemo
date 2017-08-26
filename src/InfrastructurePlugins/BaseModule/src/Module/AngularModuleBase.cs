using CoreLibModule.Utils;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Domain.Filters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Module
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
            ModuleManager.RegisterModuleComponent(GetType(), new List<Type>(moduleTemps));
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
        public List<Type> ModuleComponents
        {
            get
            {
                return ModuleManager.GetModuleComponents(GetType());
            }
        }
        private Guid xModuleId;
        /// <summary>
        /// 模块ID
        /// </summary>
        public Guid ModuleId
        {
            get
            {
                if (xModuleId == null) xModuleId = MD5Utils.GetGuidByMD5(GetType().FullName);
                return xModuleId;
            }
        }

        private List<Type> typeLists = new List<Type>();
        /// <summary>
        /// 获取某个类型的所有属性(递归)
        /// </summary>
        /// <param name="tempClsType"></param>
        /// <returns></returns>
        protected List<ViewModelPropInfo> TraversalProperties(Type tempClsType)
        {
            var propinfos = new List<ViewModelPropInfo>();
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
                        propinfos.Add(new ViewModelPropInfo() { DtoEntityType = tempClsType, PropInfo = propInfo });
                    }
                }
                else
                {
                    propinfos.Add(new ViewModelPropInfo() { DtoEntityType = tempClsType, PropInfo = propInfo });
                }
            }
            return propinfos;
        }
        /// <summary>
        /// 获取模块所有模板的对象信息
        /// </summary>
        /// <returns></returns>
        public List<ComponentClassInfo> GetComponentClassInfoes()
        {
            var moduleType = GetType();
            var ngModuleAttr = moduleType.GetTypeInfo().GetCustomAttribute<AngularModuleAttribute>();
            var moduleName = ngModuleAttr == null ? moduleType.Name : ngModuleAttr.ModuleName;

            var tempObjects = ModuleComponents
                .Where(a => a.GetTypeInfo().GetCustomAttribute<ComponentClassAttribute>() != null)
                .Select(a =>
                {
                    var nameLists = new List<string>();
                    var tempAttr = a.GetTypeInfo().GetCustomAttribute<ComponentClassAttribute>();
                    if (tempAttr != null)
                    {
                        if (tempAttr.TemplateId == Guid.Empty) tempAttr.TemplateId = MD5Utils.GetGuidByMD5(a.FullName);
                        if (tempAttr.TempClassType == null) tempAttr.TempClassType = a;
                        if (tempAttr.TempName == null) tempAttr.TempName = a.Name.Replace("Service", "");
                    }

                    return new ComponentClassInfo()
                    {
                        ModuleCatalogId = MD5Utils.GetGuidByMD5(tempAttr.ModuleCatalogType.FullName),
                        ModuleCatalogName = tempAttr.ModuleCatalogType.Name,
                        ModuleCatalogType = tempAttr.ModuleCatalogType,
                        ModuleType = moduleType,
                        ModuleId = MD5Utils.GetGuidByMD5(GetType().FullName),
                        ModuleName = moduleName,
                        TempClassType = a,
                        TempId = tempAttr.TemplateId,
                        TempName = tempAttr?.TempName,
                        TempTitle = tempAttr.TempTitle,
                        TempActions = a.GetTypeInfo().GetMethods()
                            .Where(m => m.GetCustomAttribute<ComponentMethodAttribute>() != null)
                            .Select(m =>
                            {
                                var methodAttr = m.GetCustomAttribute<ComponentMethodAttribute>();
                                methodAttr.ActionId = MD5Utils.GetGuidByMD5(a.FullName + m.Name);
                                methodAttr.ActionName = m.Name;
                                if (string.IsNullOrEmpty(methodAttr.Name))
                                {
                                    methodAttr.Name = m.Name;
                                }
                                return methodAttr;
                            })
                            .ToList(),

                        TempDataFields = tempAttr.ComponentModels
                                .SelectMany(t =>
                                {
                                    this.typeLists.Clear();
                                    return this.TraversalProperties(t);
                                })
                                .Where(p => p.PropInfo.GetCustomAttribute<ComponentPropertyAttribute>() != null)
                                .SelectMany(p =>
                                {
                                    var datafield = p.PropInfo.GetCustomAttribute<ComponentPropertyAttribute>();
                                    datafield.GroupType = p.DtoEntityType;
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
                                        if (datafield.Alias == null) datafield.Alias = p.DtoEntityType.Name.Replace("OutputDto", "") + "_" + p.PropInfo.Name;
                                        if (nameLists.Contains(datafield.Alias))
                                        {
                                            datafield.Alias = p.DtoEntityType.Name.Replace("OutputDto", "") + "_" + datafield.Name;
                                        }
                                        nameLists.Add(datafield.Alias);
                                    }
                                    return new ComponentPropertyAttribute[] { datafield };
                                }).ToList(),

                        TempFilters = Injector.ResolveMany<IEntityOperationFilter>()
                        .Select(f => f.GetType())
                        .Where(f =>
                        {
                            var filterAttr = f.GetTypeInfo().GetCustomAttribute<ComponentFilterAttribute>();
                            return filterAttr != null && filterAttr.ModuleType == GetType();
                        })
                        .Select(m =>
                        {
                            var filterAttr = m.GetTypeInfo().GetCustomAttribute<ComponentFilterAttribute>();
                            filterAttr.FilterType = m;
                            return filterAttr;
                        })
                        .Union(Injector.ResolveMany<IEntityQueryFilter>()
                        .Select(f => f.GetType())
                        .Where(f =>
                            {
                                var filterAttr = f.GetTypeInfo().GetCustomAttribute<ComponentFilterAttribute>();
                                return filterAttr != null && filterAttr.ModuleType == GetType();
                            }).Select(m =>
                            {
                                var filterAttr = m.GetTypeInfo().GetCustomAttribute<ComponentFilterAttribute>();
                                filterAttr.FilterType = m;
                                return filterAttr;
                            })).ToList()
                    };
                }).ToList();
            return tempObjects;
        }
        /// <summary>
        /// 枚举模块所有的模板
        /// </summary>
        /// <returns></returns>
        public List<Type> EnumModuleTemplates()
        {
            var moduleTemps = ModuleManager.AllModuleComponents
               .Where(t =>
               {
                   var tempClassAttr = t.GetType().GetTypeInfo().GetCustomAttribute<ComponentClassAttribute>();
                   return tempClassAttr != null && tempClassAttr.ModuleType == GetType();
               })
               .Select(t => t.GetType())
               .ToList();
            return moduleTemps;
        }
    }

    public class ViewModelPropInfo
    {
        /// <summary>
        /// 域模型类型
        /// </summary>
        public Type ModelType { get; set; }
        /// <summary>
        /// 父域模型类型
        /// </summary>
        public Type ParentModelType { get; set; }
        /// <summary>
        /// Dto实体类型
        /// </summary>
        public Type DtoEntityType { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// 父结点的属性信息,
        /// </summary>
        public PropertyInfo ParentPropInfo { get; set; }
        /// <summary>
        /// 结点的属性信息
        /// </summary>
        public PropertyInfo PropInfo { get; set; }
        /// <summary>
        /// 属性的类型
        /// 基本类型,对象类型,列表类型
        /// </summary>
        public PropClassify PropClassify { get; set; }
        /// <summary>
        /// 指示是否是集合结点
        /// </summary>
        public bool IsSetNode { get; set; }
        /// <summary>
        /// 指示是否引用独立实体
        /// </summary>
        public bool IsRefNode { get; set; }

    }

    /// <summary>
    /// 属性归类
    /// </summary>
    public enum PropClassify
    {
        /// <summary>
        /// 基本类型
        /// </summary>
        Basic = 0,
        /// <summary>
        /// 列表类型
        /// </summary>
        List = 1,
        /// <summary>
        /// 对象类型
        /// </summary>
        Object = 2
    }


}
