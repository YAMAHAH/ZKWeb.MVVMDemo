using System;

namespace InfrastructurePlugins.BaseModule.Application.Attributes
{
    /// <summary>
    /// NG模块
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AngularModuleAttribute : Attribute
    {
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
        /// 模块包含的类型
        /// </summary>
        public Type[] TemplateTypes { get; set; }

        public AngularModuleAttribute(string moduleName, params Type[] templateTypes)
        {
            ModuleName = moduleName;
            TemplateTypes = templateTypes;
        }
    }
}
