using ZKWeb.Plugin;
using ZKWebStandard.Ioc;

namespace BusinessModulePlugins.SaleModule
{
    /// <summary>
    /// 插件载入时的处理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class Plugin : IPlugin
    {
        /// <summary>
        /// 插件载入时处理
        /// </summary>
        public Plugin()
        {
        }
    }
}
