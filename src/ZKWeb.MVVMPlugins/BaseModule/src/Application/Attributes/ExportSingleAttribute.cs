using System;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Application.Attributes
{
    /// <summary>
    /// 测试自定义特性,直接使用export,可以注册单个类型
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct,
        Inherited = false,
        AllowMultiple = false)]
    public class ExportSingleAttribute : ExportAttributeBase
    {
        public override void RegisterToContainer(IContainer container, Type type, ReuseType reuseType)
        {
            if (ExportType != null)
            {
                container.Register(ExportType, type, reuseType);
            }
        }
        public Type ExportType { get; set; }
    }
}
