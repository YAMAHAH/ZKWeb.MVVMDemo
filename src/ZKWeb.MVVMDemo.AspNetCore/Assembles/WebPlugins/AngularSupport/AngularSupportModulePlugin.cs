using ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.OrganizationModule;
using ZKWeb.MVVMDemo.AspNetCore.Assembles.ZKWeb.MVVMPlugins.MVVM.Common.Base;
using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles.WebPlugins.AngularSupport
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(
    typeof(BaseModulePlugin),
    typeof(OrganizationModulePlugin)
    )]
    public class AngularSupportModulePlugin : ModulePluginBase
    {
        /// <summary>
        /// 
        /// </summary>
        public AngularSupportModulePlugin()
        {
            RootPath = PluginConfigInfo.FrontEndPluginPath;
            Path = "AngularSupport";
            EName = "AngularSupport";
            CName = "Angular支持库";
            Version = "1.0";
            Description = "Angular辅助工具插件";
        }
    }
}
