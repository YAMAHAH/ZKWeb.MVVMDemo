using System;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes
{
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
