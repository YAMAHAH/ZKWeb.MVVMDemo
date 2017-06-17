using ZKWeb.MVVMDemo.AspNetCore.Assembles.ZKWeb.MVVMPlugins.MVVM.Common.Base;
using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.ProductModule
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BaseModulePlugin))]
    public class ProductModulePlugin : ModulePluginBase
    {
        /// <summary>
        /// 
        /// </summary>
        public ProductModulePlugin()
        {
            RootPath = PluginConfigInfo.BusinessPluginPath;
            Path = "ProductModule";
            EName = "ProductModule";
            CName = "产品管理系统";
            Version = "1.0";
            Description = "产品管理系统";
        }
    }
}
