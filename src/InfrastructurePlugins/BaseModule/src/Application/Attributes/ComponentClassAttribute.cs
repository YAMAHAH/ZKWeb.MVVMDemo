using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace InfrastructurePlugins.BaseModule.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ComponentClassAttribute : Attribute
    {
        public string TemplateId { get; set; }
        /// <summary>
        /// 模板的名称
        /// </summary>
        public string TempName { get; set; }

        /// <summary>
        /// 模板标题
        /// </summary>
        public string TempTitle { get; set; }

        /// <summary>
        /// 所属模块目录类型
        /// </summary>
        public Type ModuleCatalogType { get; set; }

        /// <summary>
        /// 所属模块类型
        /// </summary>
        public Type ModuleType { get; set; }
        /// <summary>
        /// 所属模板类类型
        /// </summary>
        public Type TempClassType { get; set; }
        /// <summary>
        /// 模板的的数据模型类型
        /// </summary>
        public Type[] ComponentModels { get; set; } = new Type[] { };

        public Type[] ComponentTemplates { get; set; } = new Type[] { };
        /// <summary>
        /// 过滤器的类型
        /// </summary>
        public Type[] FilterTypes { get; set; } = new Type[] { };

        public ComponentClassAttribute(Type pModuleType, Type moduleCatalog, string title, params Type[] types)
        {
            ModuleCatalogType = moduleCatalog;
            TempTitle = title;
            ModuleType = pModuleType;
            ComponentModels = types.Where(t => typeof(IOutputDto).IsAssignableFrom(t)).ToArray();
            FilterTypes = types.Where(t => t.IsQueryOrOperationFilter()).ToArray();
        }
    }
}
