using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities.Interfaces;

namespace BusinessPlugins.OrganizationModule.Domain
{
    public interface IFullAudit<TEntity, TPrimaryKey> : IEntity<TPrimaryKey>,
          IHaveCreateTime,
          IHaveUpdateTime,
          IHaveDeleted,
          IHaveOwnerTenant,
          IEntityMappingProvider<TEntity> where TEntity : class, IEntity, new()
    {
    }
}
