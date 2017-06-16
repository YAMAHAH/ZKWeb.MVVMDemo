using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Module;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base
{
    public class BaseModulePlugin : ModulePluginBase
    {
        public BaseModulePlugin()
        {
            EName = "MVVM.Common.Base";
            CName = "基础模块";
            Version = "1.0";
            Description = "所有模块共用的基础库";
        }
    }
}
