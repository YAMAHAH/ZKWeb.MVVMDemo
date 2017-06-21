using ZKWeb.Cache;
using InfrastructurePlugins.SessionStateModule.Domain.Services;
using ZKWebStandard.Ioc;
using ZKWeb;

namespace InfrastructurePlugins.SessionStateModule.Components.CacheIsolationPolicies
{
    /// <summary>
    /// 按当前租户隔离缓存
    /// </summary>
    [ExportMany(ContractKey = "Tenant")]
    public class CacheIsolateByTenant : ICacheIsolationPolicy
    {
        /// <summary>
        /// 获取隔离键
        /// </summary>
        /// <returns></returns>
        public object GetIsolationKey()
        {
            var sessionManager = Application.Ioc.Resolve<SessionManager>();
            return sessionManager.GetSession().TenantId;
        }
    }
}
