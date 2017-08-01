using System;
using System.Collections.Generic;
using InfrastructurePlugins.BaseModule.Application.Attributes;

namespace InfrastructurePlugins.BaseModule.Module
{
    /// <summary>
    /// 模板信息类
    /// </summary>
    public class ComponentClassInfo
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        public Guid TempId { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string TempName { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string TempTitle { get; set; }
        /// <summary>
        /// 模板类型
        /// </summary>
        public Type TempClassType { get; set; }
        /// <summary>
        /// 模块ID
        /// </summary>
        public Guid ModuleId { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 模块类型
        /// </summary>
        public Type ModuleType { get; set; }

        /// <summary>
        /// 模块目录ID
        /// </summary>
        public Guid ModuleCatalogId { get; set; }
        /// <summary>
        /// 模块目录名称
        /// </summary>
        public string ModuleCatalogName { get; set; }
        /// <summary>
        /// 模块目录类型
        /// </summary>
        public Type ModuleCatalogType { get; set; }
        /// <summary>
        /// 模板字段特性集合
        /// </summary>
        public IEnumerable<ComponentPropertyAttribute> TempDataFields { get; set; }
        /// <summary>
        /// 模板功能特性集合
        /// </summary>
        public IEnumerable<ComponentMethodAttribute> TempActions { get; set; }
        /// <summary>
        /// 模板过滤器集合
        /// </summary>
        public IEnumerable<ComponentFilterAttribute> TempFilters { get; set; } = new List<ComponentFilterAttribute>();
    }
}
