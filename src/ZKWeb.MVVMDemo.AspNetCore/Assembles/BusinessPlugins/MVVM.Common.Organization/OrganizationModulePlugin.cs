using ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.ProductEngineeringModule;
using ZKWeb.MVVMDemo.AspNetCore.Assembles.InfrastructurePlugins.BaseModule;
using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.OrganizationModule
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BaseModulePlugin), typeof(ProductEngineeringModulePlugin))]
    public class OrganizationModulePlugin : ModulePluginBase
    {
        /// <summary>
        /// 
        /// </summary>
        public OrganizationModulePlugin()
        {
            RootPath = PluginConfigInfo.BusinessPluginPath;
            Path = "OrganizationModule";
            EName = "OrganizationModule";
            CName = "Organization Structure For MVVM";
            Version = "1.0";
            Description = "Provide organization entities and domain services";
        }
    }
}
