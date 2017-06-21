using InfrastructurePlugins.MultiTenantModule.Domain.Entities;

namespace InfrastructurePlugins.MultiTenantModule.Components.TenantProviders
{
    /// <summary>
    /// 当前租户的提供器
    /// 可以注册多个，任意一个提供器返回租户时则使用该租户
    /// </summary>
    public interface ITenantProvider
    {
        /// <summary>
        /// 获取当前租户
        /// 获取不到时可以返回null
        /// </summary>
        /// <returns></returns>
        Tenant GetTenant();
    }
}
