using System;
using InfrastructurePlugins.BaseModule.Domain.Repositories.Bases;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Repositories
{
    /// <summary>
    /// 通用配置的仓储
    /// </summary>
    [ExportMany, SingletonReuse]
    public class GenericConfigRepository : RepositoryBase<GenericConfig, Guid> { }
}
