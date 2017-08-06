using BusinessPlugins.BasicModule.Application.Module;
using BusinessPlugins.BasicModule.ModuleCatalogs;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Application.Services.Bases;
using InfrastructurePlugins.BaseModule.Application.Services.Interfaces;
using System.ComponentModel;
using ZKWeb.Web;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Application.Services
{
    /// <summary>
    /// 全局管理服务
    /// </summary>
    [ExportMany, SingletonReuse, Description("全局管理服务")]
    [ComponentClass(typeof(GlobalManagerModule), typeof(BasicModuleCatalog), "全局管理")]
    public class GlobalManageService : ApplicationServiceBase, IGlobalManageService
    {
        [Description("删除数据")]
        [Action("Test", HttpMethods.GET)]
        public ActionResponseDto Test()
        {
            return ActionResponseDto.CreateSuccess("Saved Successfully");
        }
    }
}
