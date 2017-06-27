using InfrastructurePlugins.MultiTenantModule.Components.TenantProviders;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.SessionStateModule.Domain.Extensions;
using InfrastructurePlugins.SessionStateModule.Domain.Services;
using ZKWeb;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.SessionStateModule.Components.TenantProviders
{
    /// <summary>
    /// 从会话提供当前租户
    /// </summary>
    [ExportMany]
    public class SessionTenantProvider : ITenantProvider
    {
        /// <summary>
        /// 获取租户
        /// </summary>
        /// <returns></returns>
        public Tenant GetTenant()
        {
            var sessionManager = Application.Ioc.Resolve<SessionManager>();
            var session = sessionManager.GetSession();
            return session.GetTenant();
        }
    }
}
