﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.BaseModule.Domain.Filters;
using InfrastructurePlugins.BaseModule.Domain.Repositories.Interfaces;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using InfrastructurePlugins.BaseModule.Domain.Uow;
using InfrastructurePlugins.BaseModule.Domain.Uow.Extensions;
using InfrastructurePlugins.BaseModule.Domain.Uow.Interfaces;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace InfrastructurePlugins.BaseModule.Domain.Services.Bases
{
    /// <summary>
    /// 领域服务的基础类
    /// </summary>
    public abstract class DomainServiceBase : IDomainService
    {
        /// <summary>
        /// 获取依赖注入器(容器)
        /// </summary>
        protected virtual IContainer Injector
        {
            get { return ZKWeb.Application.Ioc; }
        }
        /// <summary>
        /// 获取工作单元
        /// </summary>
        protected virtual IUnitOfWork UnitOfWork
        {
            get { return ZKWeb.Application.Ioc.Resolve<IUnitOfWork>(); }
        }
    }

    /// <summary>
    /// 领域服务的基础类
    /// 提供一系列基础功能
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class DomainServiceBase<TEntity, TPrimaryKey> :
        DomainServiceBase,
        IDomainService<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        /// <summary>
        /// 获取仓储
        /// </summary>
        protected virtual IRepository<TEntity, TPrimaryKey> Repository
        {
            get
            {
                return Injector.Resolve<IRepository<TEntity, TPrimaryKey>>(IfUnresolved.ReturnDefault) ??
                       UnitOfWork.GetRepository<TEntity, TPrimaryKey>();
            }
        }

        /// <summary>
        /// 获取仓储
        /// </summary>
        protected virtual IUnitOfWorkRepository<TEntity, TPrimaryKey> UnitRepository
        {
            get { return UnitOfWork.GetUnitRepository<TEntity, TPrimaryKey>(); }
        }

        /// <summary>
        /// 树型结构结点更新,子类必须实现自己的逻辑
        /// </summary>
        /// <param name="childId"></param>
        /// <param name="rootId"></param>
        protected virtual void UpdateNodeOrder(Guid childId, Guid rootId)
        {
            //UnitOfWork.GetRepository<TEntity, TPrimaryKey>();
            throw new NotImplementedException();
        }

        protected virtual NodeOrderInfo CalaOrder(TEntity treeNode)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        public virtual TEntity Get(TPrimaryKey id)
        {
            if (id.Equals(default(TPrimaryKey)))
            {
                return default(TEntity);
            }
            var expr = ExpressionUtils.MakeMemberEqualiventExpression<TEntity>("Id", id);
            using (UnitOfWork.Scope())
            {
                return Repository.Get(expr);
            }
        }

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            using (UnitOfWork.Scope())
            {
                return Repository.Get(predicate);
            }
        }

        /// <summary>
        /// 根据条件获取实体列表
        /// </summary>
        public virtual IList<TEntity> GetMany(
            Expression<Func<TEntity, bool>> predicate = null)
        {
            using (UnitOfWork.Scope())
            {
                var query = Repository.Query();
                if (predicate != null)
                {
                    query = query.Where(predicate);
                }
                return query.ToList();
            }
        }

        /// <summary>
        /// 根据过滤函数获取实体列表
        /// </summary>
        public virtual TResult GetMany<TResult>(
            Func<IQueryable<TEntity>, TResult> fetch)
        {
            using (UnitOfWork.Scope())
            {
                return fetch(Repository.Query());
            }
        }

        /// <summary>
        /// 计算符合条件的实体数量
        /// </summary>
        public long Count(Expression<Func<TEntity, bool>> predicate)
        {
            using (UnitOfWork.Scope())
            {
                return Repository.Count(predicate);
            }
        }

        /// <summary>
        /// 保存实体
        /// </summary>
        public virtual void Save(ref TEntity entity, Action<TEntity> update = null)
        {
            using (UnitOfWork.Scope())
            {
                Repository.Save(ref entity, update);
            }
        }

        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        public virtual bool Delete(TPrimaryKey id)
        {
            var expr = ExpressionUtils.MakeMemberEqualiventExpression<TEntity>("Id", id);
            using (UnitOfWork.Scope())
            {
                return Repository.BatchDelete(expr) > 0;
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        public virtual void Delete(TEntity entity)
        {
            using (UnitOfWork.Scope())
            {
                Repository.Delete(entity);
            }
        }

        public virtual void BatchSave(ref IEnumerable<TEntity> entities, Action<TEntity> update = null)
        {
            var uow = UnitOfWork;
            using (uow.Scope())
            {
                Repository.BatchSave(ref entities, update);
            }

        }
        /// <summary>
        /// 批量标记已删除或未删除
        /// 返回标记的数量，不会实际删除
        /// </summary>
        public virtual long BatchSetDeleted(IEnumerable<TPrimaryKey> ids, bool deleted)
        {
            var uow = UnitOfWork;
            using (uow.Scope())
            using (uow.DisableQueryFilter(typeof(DeletedFilter)))
            {
                var entities = Repository.Query().Where(e => ids.Contains(e.Id)).ToList();
                var entitiesRef = entities.AsEnumerable();
                Repository.BatchSave(ref entitiesRef, e => ((IHaveDeleted)e).Deleted = deleted);
                return entities.Count;
            }
        }

        /// <summary>
        /// 批量永久删除
        /// </summary>
        public virtual long BatchDeleteForever(IEnumerable<TPrimaryKey> ids)
        {
            var uow = UnitOfWork;
            using (uow.Scope())
            using (uow.DisableQueryFilter(typeof(DeletedFilter)))
            {
                return Repository.BatchDelete(e => ids.Contains(e.Id));
            }
        }
    }

}
