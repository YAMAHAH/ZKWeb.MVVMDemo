using System;

namespace InfrastructurePlugins.BaseModule.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AngularModuleAttribute : Attribute
    {
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
        /// 模块包含的类型
        /// </summary>
        public Type[] TemplateTypes { get; set; }

        public AngularModuleAttribute(string pModuleName,params Type[] pTemplateTypes)
        {
            ModuleName = pModuleName;
            TemplateTypes = pTemplateTypes;
        }
    }
}
