using ZKWeb.MVVMDemo.AspNetCore.Assembles.ZKWeb.MVVMPlugins.MVVM.Common.Base;
using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.SaleModule
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BaseModulePlugin))]
    public class SaleModulePlugin : ModulePluginBase
    {
        /// <summary>
        /// 
        /// </summary>
        public SaleModulePlugin()
        {
            RootPath = PluginConfigInfo.BusinessPluginPath;
            Path = "SaleModule";
            EName = "SaleModule";
            CName = "销售管理模块";
            Version = "1.0";
            Description = "销售管理系统";
        }
    }
}
