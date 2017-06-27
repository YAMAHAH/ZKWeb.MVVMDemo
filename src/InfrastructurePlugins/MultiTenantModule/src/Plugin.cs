using InfrastructurePlugins.MultiTenantModule.Domain.Services;
using ZKWeb.Plugin;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.MultiTenantModule.src {
    /// <summary>
    /// 插件载入时的处理
    /// </summary>
    [ExportMany]
    public class Plugin : IPlugin {
        /// <summary>
        /// 确保主租户存在
        /// </summary>
        public Plugin(TenantManager tenantManager) {
            tenantManager.EnsureMasterTenant();
        }
    }
}
