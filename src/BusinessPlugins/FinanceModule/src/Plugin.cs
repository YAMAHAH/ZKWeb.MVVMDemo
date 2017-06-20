using ZKWeb.Plugin;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.FinanceModule
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
