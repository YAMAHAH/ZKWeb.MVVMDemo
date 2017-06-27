using ZKWeb.MVVMDemo.AspNetCore.Assembles.InfrastructurePlugins.BaseModule;
using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.ProductEngineeringModule
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BaseModulePlugin))]
    public class ProductEngineeringModulePlugin : ModulePluginBase
    {
        /// <summary>
        /// 
        /// </summary>
        public ProductEngineeringModulePlugin()
        {
            RootPath = PluginConfigInfo.BusinessPluginPath;
            Path = "ProductEngineeringModule";
            EName = "ProductEngineeringModule";
            CName = "产品管理系统";
            Version = "1.0";
            Description = "产品管理系统";
        }
    }
}
