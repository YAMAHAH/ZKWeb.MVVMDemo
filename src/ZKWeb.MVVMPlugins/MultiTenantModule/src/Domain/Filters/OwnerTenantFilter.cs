﻿using System;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWeb.Localize;
using InfrastructurePlugins.BaseModule.Components.Exceptions;
using InfrastructurePlugins.BaseModule.Domain.Filters.Interfaces;
using InfrastructurePlugins.BaseModule.Domain.Repositories.Interfaces;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities.TypeTraits;
using InfrastructurePlugins.MultiTenantModule.Domain.Services;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.MultiTenantModule.Domain.Filters
{
    /// <summary>
    /// 根据数据所属租户过滤查询和操作
    /// </summary>
    [ExportMany]
    public class OwnerTenantFilter : IEntityQueryFilter, IEntityOperationFilter
    {
        /// <summary>
        /// 当前的租户
        /// </summary>
        public Tenant UsingTenant => _usingTenant.Value;
        protected Lazy<Tenant> _usingTenant;

        /// <summary>
        /// 初始化
        /// </summary>
        public OwnerTenantFilter()
        {
            var tenantManager = ZKWeb.Application.Ioc.Resolve<TenantManager>();
            _usingTenant = new Lazy<Tenant>(() => tenantManager.GetTenant());
        }

        /// <summary>
        /// 过滤查询
        /// </summary>
        IQueryable<TEntity> IEntityQueryFilter.FilterQuery<TEntity, TPrimaryKey>(
            IQueryable<TEntity> query)
        {
            if (!OwnerTenantTypeTrait<TEntity>.HaveOwnerTenant)
            {
                return query;
            }
            // 按租户过滤数据
            // 主租户也需要过滤数据
            if (UsingTenant == null)
            {
                return query.Where(e =>
                    ((IHaveOwnerTenant)e).OwnerTenant == null);
            }
            else
            {
                return query.Where(e =>
                   ((IHaveOwnerTenant)e).OwnerTenant != null &&
                   ((IHaveOwnerTenant)e).OwnerTenant.Id == UsingTenant.Id);
            }
        }

        /// <summary>
        /// 过滤查询条件
        /// </summary>
        Expression<Func<TEntity, bool>> IEntityQueryFilter.FilterPredicate<TEntity, TPrimaryKey>(
            Expression<Func<TEntity, bool>> predicate)
        {
            if (!OwnerTenantTypeTrait<TEntity>.HaveOwnerTenant)
            {
                return predicate;
            }
            // 按租户过滤数据
            // 主租户也需要过滤数据
            if (UsingTenant == null)
            {
                var paramExpr = predicate.Parameters[0];
                var ownerTanantExpr = Expression.Property(
                    paramExpr, nameof(IHaveOwnerTenant.OwnerTenant));
                var ownerTanantIdExpr = Expression.Property(
                    ownerTanantExpr, nameof(IEntity<Guid>.Id));
                var body = Expression.AndAlso(
                    predicate.Body,
                    Expression.Equal(ownerTanantExpr, Expression.Constant(null)));
                predicate = Expression.Lambda<Func<TEntity, bool>>(body, paramExpr);
            }
            else
            {
                var paramExpr = predicate.Parameters[0];
                var ownerTanantExpr = Expression.Property(
                    paramExpr, nameof(IHaveOwnerTenant.OwnerTenant));
                var ownerTanantIdExpr = Expression.Property(
                    ownerTanantExpr, nameof(IEntity<Guid>.Id));
                var body = Expression.AndAlso(
                    predicate.Body,
                    Expression.AndAlso(
                        Expression.NotEqual(ownerTanantExpr, Expression.Constant(null)),
                        Expression.Equal(ownerTanantIdExpr, Expression.Constant(UsingTenant.Id))));
                predicate = Expression.Lambda<Func<TEntity, bool>>(body, paramExpr);
            }
            return predicate;
        }

        /// <summary>
        /// 保存时检查所属租户
        /// </summary>
        void IEntityOperationFilter.FilterSave<TEntity, TPrimaryKey>(TEntity entity)
        {
            if (!OwnerTenantTypeTrait<TEntity>.HaveOwnerTenant)
            {
                return;
            }
            var e = ((IHaveOwnerTenant)entity);
            if (e.OwnerTenant == null)
            {
                // 设置数据的租户
                if (UsingTenant != null)
                {
                    var repository = ZKWeb.Application.Ioc.Resolve<IRepository<Tenant, Guid>>();
                    e.OwnerTenant = repository.Get(u => u.Id == UsingTenant.Id);
                }
            }
            else
            {
                // 检查数据的租户是否一致，主租户不要求一致
                if (UsingTenant == null ||
                    (!UsingTenant.IsMaster && UsingTenant.Id != e.OwnerTenant.Id))
                {
                    throw new ForbiddenException(
                    new T("Action require the tennat ownership of {0}: {1}",
                    new T(typeof(TEntity).Name), entity.Id));
                }
            }
        }

        /// <summary>
        /// 删除时检查所属租户
        /// </summary>
        void IEntityOperationFilter.FilterDelete<TEntity, TPrimaryKey>(TEntity entity)
        {
            if (!OwnerTenantTypeTrait<TEntity>.HaveOwnerTenant)
            {
                return;
            }
            var e = ((IHaveOwnerTenant)entity);
            if (e.OwnerTenant != null)
            {
                // 检查数据的租户是否一致，主租户不要求一致
                if (UsingTenant == null ||
                    (!UsingTenant.IsMaster && UsingTenant.Id != e.OwnerTenant.Id))
                {
                    throw new ForbiddenException(
                        new T("Action require the tennat ownership of {0}: {1}",
                        new T(typeof(TEntity).Name), entity.Id));
                }
            }
        }
    }
}
