using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles.InfrastructurePlugins.BaseModule
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseModulePlugin : ModulePluginBase
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseModulePlugin()
        {
            RootPath = PluginConfigInfo.BasePluginPath;
            Path = "BaseModule";
            EName = "BaseModule";
            CName = "基础模块";
            Version = "1.0";
            Description = "所有模块共用的基础库";
        }
    }
}
