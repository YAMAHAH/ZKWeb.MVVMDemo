using System;
using System.Collections.Generic;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Module
{
    /// <summary>
    /// 模板信息类
    /// </summary>
    public class TemplateInfo
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        public string TempId { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string TempName { get; set; }
        /// <summary>
        /// 模板类型
        /// </summary>
        public Type TempClassType { get; set; }
        /// <summary>
        /// 模块ID
        /// </summary>
        public string ModuleId { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 模块类型
        /// </summary>
        public Type ModuleType { get; set; }
        /// <summary>
        /// 模板字段特性集合
        /// </summary>
        public IEnumerable<TempDataFieldAttribute> TempDataFields { get; set; }
        /// <summary>
        /// 模板功能特性集合
        /// </summary>
        public IEnumerable<TempActionAttribute> TempActions { get; set; }
        /// <summary>
        /// 模板过滤器集合
        /// </summary>
        public IEnumerable<Type> TempFilters { get; set; }
    }
}
