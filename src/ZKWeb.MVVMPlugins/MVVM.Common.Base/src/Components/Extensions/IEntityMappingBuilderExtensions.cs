using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKWeb.Database;
using ZKWeb.ORM.EFCore;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions
{
    public static class IEntityMappingBuilderExtensions
    {
        public static EntityTypeBuilder<TEntity> GetNativeBuilder<TEntity>(this IEntityMappingBuilder<TEntity> builder) where TEntity : class, IEntity, new()
        {
           return ((EFCoreEntityMappingBuilder<TEntity>)builder).Builder;
        }
    }
}
