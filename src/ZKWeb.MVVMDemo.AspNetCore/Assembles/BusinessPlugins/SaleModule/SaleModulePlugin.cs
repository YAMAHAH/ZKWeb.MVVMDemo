using ZKWeb.MVVMDemo.AspNetCore.Assembles.InfrastructurePlugins.BaseModule;
using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.SalesModule
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BaseModulePlugin))]
    public class SalesModulePlugin : ModulePluginBase
    {
        /// <summary>
        /// 
        /// </summary>
        public SalesModulePlugin()
        {
            RootPath = PluginConfigInfo.BusinessPluginPath;
            Path = "SalesModule";
            EName = "SalesModule";
            CName = "销售管理模块";
            Version = "1.0";
            Description = "销售管理系统";
        }
    }
}
