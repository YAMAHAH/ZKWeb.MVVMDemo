using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Module;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Application.Module
{
    /// <summary>
    /// 全局懒加载模块
    /// </summary>
    [ExportMany, SingletonReuse]
    [AngularModule("GlobalManager")]
    public class GlobalManagerModule : AngularModuleBase
    {
    }
}
