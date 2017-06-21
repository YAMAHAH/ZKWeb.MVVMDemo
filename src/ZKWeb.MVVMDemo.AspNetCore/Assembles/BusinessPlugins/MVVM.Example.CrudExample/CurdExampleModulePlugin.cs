using ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.OrganizationModule;
using ZKWeb.MVVMDemo.AspNetCore.Assembles.InfrastructurePlugins.MVVM.Common.Base;
using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.CrudExampleModule
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BaseModulePlugin),typeof(OrganizationModulePlugin))]
    public class CRUDExampleModulePlugin : ModulePluginBase
    {
        /// <summary>
        /// 
        /// </summary>
        public CRUDExampleModulePlugin()
        {
            RootPath = PluginConfigInfo.BusinessPluginPath;
            Path = "CrudExampleModule";
            EName = "CrudExampleModule";
            CName = "增删改查实例模块";
            Version = "1.0";
            Description = "增删改查实例模块";
        }
    }
}
