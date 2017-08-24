using System;

namespace InfrastructurePlugins.BaseModule.Application.Attributes
{
    /// <summary>
    /// 模型类型映射
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModelTypeMapperAttribute : Attribute
    {
        /// <summary>
        /// 对应的模型类型
        /// </summary>
        public Type ModelType { get; set; }

        public ModelTypeMapperAttribute(Type modelType)
        {
            ModelType = modelType;
        }
    }
}
