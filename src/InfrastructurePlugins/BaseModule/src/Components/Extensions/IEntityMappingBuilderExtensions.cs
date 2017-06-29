using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWeb.ORM.EFCore;

namespace InfrastructurePlugins.BaseModule.Components.Extensions
{
    public static class IEntityMappingBuilderExtensions
    {
        public static EntityTypeBuilder<TEntity> GetNativeBuilder<TEntity>(this IEntityMappingBuilder<TEntity> builder)
            where TEntity : class, IEntity, new()
        {
            return ((EFCoreEntityMappingBuilder<TEntity>)builder).Builder;
        }
        /// <summary>
        /// 一对多单向引用
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TRef"></typeparam>
        /// <param name="builder"></param>
        /// <param name="oneRef"></param>
        /// <param name="foreignKey"></param>
        /// <param name="deleteBehavior"></param>
        public static void HasMany<TEntity, TRef>(
            this IEntityMappingBuilder<TEntity> builder,
            Expression<Func<TEntity, TRef>> oneRef,
            Expression<Func<TEntity, object>> foreignKey,
            DeleteBehavior deleteBehavior = DeleteBehavior.Restrict)
            where TEntity : class, IEntity, new()
            where TRef : class, IEntity, new()
        {
            builder.GetNativeBuilder<TEntity>().HasOne(oneRef)
                .WithMany()
                .HasForeignKey(foreignKey)
                .OnDelete(deleteBehavior);
        }
        /// <summary>
        /// 一对多双向引用
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TRef"></typeparam>
        /// <param name="builder"></param>
        /// <param name="oneRef"></param>
        /// <param name="ManyRef"></param>
        /// <param name="foreignKey"></param>
        /// <param name="deleteBehavior"></param>
        public static void HasMany<TEntity, TRef>(
           this IEntityMappingBuilder<TEntity> builder,
           Expression<Func<TEntity, TRef>> oneRef,
           Expression<Func<TRef, IEnumerable<TEntity>>> ManyRef,
           Expression<Func<TEntity, object>> foreignKey,
           DeleteBehavior deleteBehavior = DeleteBehavior.Cascade)
           where TEntity : class, IEntity, new()
           where TRef : class, IEntity, new()
        {
            builder.GetNativeBuilder<TEntity>().HasOne(oneRef)
                .WithMany(ManyRef)
                .HasForeignKey(foreignKey)
                .OnDelete(deleteBehavior);
        }
        /// <summary>
        /// 一对一单向引用
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TRef"></typeparam>
        /// <param name="builder"></param>
        /// <param name="oneRef"></param>
        /// <param name="foreignKey"></param>
        /// <param name="deleteBehavior"></param>
        public static void HasOne<TEntity, TRef>(
          this IEntityMappingBuilder<TEntity> builder,
          Expression<Func<TEntity, TRef>> oneRef,
          Expression<Func<TEntity, object>> foreignKey,
          DeleteBehavior deleteBehavior = DeleteBehavior.Restrict)
          where TEntity : class, IEntity, new()
          where TRef : class, IEntity, new()
        {
            builder.GetNativeBuilder<TEntity>().HasOne(oneRef)
                .WithOne()
                .HasForeignKey(foreignKey)
                .OnDelete(deleteBehavior);
        }
        /// <summary>
        /// 一对一双向引用
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TRef"></typeparam>
        /// <param name="builder"></param>
        /// <param name="oneRef"></param>
        /// <param name="otherRef"></param>
        /// <param name="foreignKey"></param>
        /// <param name="deleteBehavior"></param>
        public static void HasOne<TEntity, TRef>(
        this IEntityMappingBuilder<TEntity> builder,
        Expression<Func<TEntity, TRef>> oneRef,
        Expression<Func<TRef, TEntity>> otherRef,
        Expression<Func<TEntity, object>> foreignKey,
        DeleteBehavior deleteBehavior = DeleteBehavior.Restrict)
        where TEntity : class, IEntity, new()
        where TRef : class, IEntity, new()
        {
            builder.GetNativeBuilder<TEntity>().HasOne(oneRef)
                .WithOne(otherRef)
                .HasForeignKey(foreignKey)
                .OnDelete(deleteBehavior);
        }
    }
}
