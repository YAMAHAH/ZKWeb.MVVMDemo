using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Module;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Application.Module
{
    /// <summary>
    /// ng懒加载模块
    /// </summary>
    [ExportMany, SingletonReuse]
    [AngularModule("UserManager")]
    public class UserManagerModule : AngularModuleBase
    {
    }
}
