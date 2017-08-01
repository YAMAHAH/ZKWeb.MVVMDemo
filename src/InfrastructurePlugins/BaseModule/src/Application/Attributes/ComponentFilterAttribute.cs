using System;

namespace InfrastructurePlugins.BaseModule.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ComponentFilterAttribute : Attribute
    {
        /// <summary>
        /// 所属模板类类型
        /// </summary>
        public Type TempClassType { get; set; }
        /// <summary>
        /// 过滤器文本
        /// </summary>
        public string FilterText { get; set; }

        /// <summary>
        /// 所属模块目录类型
        /// </summary>
        public Type ModuleCatalogType { get; set; }

        /// <summary>
        /// 所属模块类型
        /// </summary>
        public Type ModuleType { get; set; }
        /// <summary>
        /// 过滤器类型
        /// </summary>
        public Type FilterType { get; set; }

        public ComponentFilterAttribute(Type moduleCatalog, Type moduleType, Type tempType, string text)
        {
            ModuleCatalogType = moduleCatalog;
            ModuleType = moduleType;
            TempClassType = tempType;
            FilterText = text;
        }
    }
}
