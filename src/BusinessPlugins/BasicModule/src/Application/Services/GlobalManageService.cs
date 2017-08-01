using BusinessPlugins.BasicModule.Application.Module;
using BusinessPlugins.BasicModule.ModuleCatalogs;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Application.Services.Bases;
using System.ComponentModel;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Application.Services
{
    /// <summary>
    /// 全局管理服务
    /// </summary>
    [ExportMany, SingletonReuse, Description("全局管理服务")]
    [ComponentClass(typeof(GlobalManagerModule), typeof(BasicModuleCatalog), "全局管理")]
    public class GlobalManageService : ApplicationServiceBase
    {

    }
}
