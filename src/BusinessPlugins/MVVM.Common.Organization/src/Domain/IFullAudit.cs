using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.Interfaces;

namespace BusinessPlugins.MVVM.Common.Organization.Domain
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
