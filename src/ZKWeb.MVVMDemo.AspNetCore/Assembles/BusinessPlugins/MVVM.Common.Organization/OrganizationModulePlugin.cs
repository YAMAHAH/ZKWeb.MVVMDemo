using ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.ProductModule;
using ZKWeb.MVVMDemo.AspNetCore.Assembles.ZKWeb.MVVMPlugins.MVVM.Common.Base;
using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.MVVM.Common.Organization
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BaseModulePlugin), typeof(ProductModulePlugin))]
    public class OrganizationModulePlugin : ModulePluginBase
    {
        /// <summary>
        /// 
        /// </summary>
        public OrganizationModulePlugin()
        {
            RootPath = PluginConfigInfo.BusinessPluginPath;
            Path = "MVVM.Common.Organization";
            EName = "MVVM.Common.Organization";
            CName = "Organization Structure For MVVM";
            Version = "1.0";
            Description = "Provide organization entities and domain services";
        }
    }
}
