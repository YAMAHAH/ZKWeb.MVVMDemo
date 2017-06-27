using System;
using InfrastructurePlugins.BaseModule.Domain.Repositories.Bases;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.MultiTenantModule.Domain.Repository
{
    /// <summary>
    /// 租户的仓储
    /// </summary>
    [ExportMany]
    public class TenantRepository : RepositoryBase<Tenant, Guid> { }
}
