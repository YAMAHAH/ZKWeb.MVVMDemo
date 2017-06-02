using System;
using System.Linq;
using System.Reflection;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TempClassAttribute : Attribute
    {
        public string TemplateId { get; set; }
        /// <summary>
        /// 模板的名称
        /// </summary>
        public string TempName { get; set; }

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
        public Type[] TemplateModels { get; set; } = new Type[] { };
        /// <summary>
        /// 过滤器的类型
        /// </summary>
        public Type[] FilterTypes { get; set; } = new Type[] { };

        public TempClassAttribute(Type pModuleType, params Type[] types)
        {
            ModuleType = pModuleType;
            TemplateModels = types.Where(t => typeof(IOutputDto).IsAssignableFrom(t)).ToArray();
            FilterTypes = types.Where(t => t.IsQueryOrOperationFilter()).ToArray();
        }
    }
}
