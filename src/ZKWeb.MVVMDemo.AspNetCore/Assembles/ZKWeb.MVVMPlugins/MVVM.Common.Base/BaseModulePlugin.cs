using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles.InfrastructurePlugins.MVVM.Common.Base
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
            Path = "MVVM.Common.Base";
            EName = "MVVM.Common.Base";
            CName = "基础模块";
            Version = "1.0";
            Description = "所有模块共用的基础库";
        }
    }
}
