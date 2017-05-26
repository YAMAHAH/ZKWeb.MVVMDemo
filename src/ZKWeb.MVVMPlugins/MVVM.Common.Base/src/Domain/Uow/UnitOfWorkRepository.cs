using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow
{
    [ExportMany, SingletonReuse]
    public class UnitOfWorkRepository
    {
        private IUnitOfWork _unitOfWork;
        private IUnitOfWork UnitOfWork
        {
            get
            {
                if (_unitOfWork == null) _unitOfWork = ZKWeb.Application.Ioc.Resolve<IUnitOfWork>();
                return _unitOfWork;
            }
        }

        private IDatabaseContext context
        {
            get
            {
                return UnitOfWork.Context;
            }
        }
        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        private DbContext DbContext
        {
            get { return (DbContext)UnitOfWork?.Context; }
        }
        #region 一对多实体增删改查
        /// <summary>
        /// 更新1对多关系的实体值
        /// </summary>
        ///<param name="existEntity">数据库存在的实体</param>
        ///<param name="nowEntity">当前存在的实体</param>
        ///<param name="getChilds">获取1对多关系的实体回调</param>
        ///<param name="getCompareKey">实体差异比较条件回调</param>
        ///<param name="getKey">实体更新获取数据库实体的条件回调</param>
        /// <returns>VOID/returns>
        public void UpdateValues<T, T1, TKey>(T existEntity, T nowEntity, Func<T, List<T1>> getChilds,
         Func<T1, TKey> getCompareKey, Func<T1, T1, bool> getKey) where T : class, IEntity where T1 : class, IEntity
        {
            UpdateEntityValues(existEntity, nowEntity);
            var entityDiff = new EntityDiff<T1, TKey>(getChilds(existEntity), getChilds(nowEntity), getCompareKey);
            entityDiff.DeletedEntities.ForEach(p => UpdateEntityState(p, EntityState.Deleted));
            getChilds(existEntity).AddRange(entityDiff.AddedEntities);
            foreach (var modEntity in entityDiff.ModifiedEntities)
            {
                var oldEntity = getChilds(existEntity).Where(p => getKey(modEntity, p)).FirstOrDefault();
                UpdateEntityValues(oldEntity, modEntity);
            }
        }

        public void UpdateValues<T, TKey>(List<T> getExistLists, List<T> getNowLists, Func<T, TKey> getCompareKey,
         Func<T, T, bool> getKey) where T : class, IEntity
        {
            var entityDiff = new EntityDiff<T, TKey>(getExistLists, getNowLists, getCompareKey);
            entityDiff.DeletedEntities.ForEach(p => UpdateEntityState(p, EntityState.Deleted));
            getExistLists.AddRange(entityDiff.AddedEntities);
            foreach (var modEntity in entityDiff.ModifiedEntities)
            {
                var oldEntity = getExistLists.Where(p => getKey(modEntity, p)).FirstOrDefault();
                UpdateEntityValues(oldEntity, modEntity);
            }
        }
        public void UpdateValues<TEntity, TPrimaryKey, T, TListEntity, TKey>(T existEntity, T nowEntity, List<Func<T, List<TListEntity>>> getLists,
         List<Func<TListEntity, TKey>> getListCompareKey, List<Func<TListEntity, TListEntity, bool>> getListKey)
         where T : class, IEntity where TListEntity : class, IEntity
            where TEntity : class
        {
            for (int i = 0; i < getLists.Count; i++)
            {
                UpdateValues(getLists[i](existEntity), getLists[i](nowEntity), getListCompareKey[i], getListKey[i]);
            }
        }

        #endregion

        #region 结点增删改查

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="existNode"></param>
        /// <param name="nowNode"></param>
        /// <param name="getChilds"></param>
        /// <param name="getCompareKey"></param>
        /// <param name="getEntityKey"></param>
        /// <param name="getNewNode"></param>
        /// <param name="getConfig"></param>
        public void UpdateNodeValues<T, TKey>(T existNode, T nowNode, Func<T, List<T>> getChilds, Func<T, TKey> getCompareKey,
         Func<T, T, bool> getEntityKey, Func<T, T> getNewNode, Action<T, T> getConfig) where T : class, IEntity
        {
            UpdateEntityValues(existNode, nowNode);
            var entityDiff = new EntityDiff<T, TKey>(getChilds(existNode), getChilds(nowNode), getCompareKey);
            //删除
            entityDiff.DeletedEntities.ForEach(node => DeleteNode(node, getChilds));
            //新增
            entityDiff.AddedEntities.ForEach(node =>
            {
                var newChildNode = getNewNode(existNode);
                // getConfig(newChildNode, node);
                InsertEntity(newChildNode);
                getChilds(existNode).Add(newChildNode);
                AddNodeValues(newChildNode, node, getChilds, getNewNode, getConfig);
            });
            //更新
            entityDiff.ModifiedEntities.ForEach(modNode =>
            {
                var existEntity = getChilds(existNode).Where(n => getEntityKey(modNode, n)).FirstOrDefault();
                UpdateNodeValues(existEntity, modNode, getChilds, getCompareKey, getEntityKey, getNewNode, getConfig);
            });
        }

        public void UpdateNodeDetailValues<T, TDetail, TKey>(T existNode, T nowNode, Func<T, List<T>> getChilds,
         List<Func<T, List<TDetail>>> getDetails, List<Func<TDetail, TKey>> getDetailCompareKey, Func<T, T, bool> getChildKey,
          List<Func<TDetail, TDetail, bool>> getDetailKey) where T : class, IEntity where TDetail : class, IEntity
        {
            for (int i = 0; i < getDetails.Count; i++)
            {
                UpdateValues(existNode, nowNode, getDetails[i], getDetailCompareKey[i], getDetailKey[i]);
            }
            foreach (var childNode in getChilds(existNode))
            {
                var newNode = getChilds(nowNode).Find(nd => getChildKey(childNode, nd));
                UpdateNodeDetailValues(childNode, newNode, getChilds, getDetails, getDetailCompareKey, getChildKey, getDetailKey);
            }
        }

        public void AddNodeValues<T>(T existNode, T nowNode, Func<T, IList<T>> getChilds,
         Func<T, T> getNewNode, Action<T, T> getConfig) where T : class, IEntity
        {
            //排除主键
            getConfig(existNode, nowNode);
            UpdateEntityValues(existNode, nowNode);

            foreach (var addNode in getChilds(nowNode))
            {
                var newChildNode = getNewNode(existNode);
                InsertEntity(newChildNode);
                getChilds(existNode).Add(newChildNode);
                AddNodeValues(newChildNode, addNode, getChilds, getNewNode, getConfig);
            }
        }
        public void DeleteNode<T>(T node, Func<T, IEnumerable<T>> getChilds) where T : class, IEntity
        {
            foreach (var delNode in GetAllNodes(node, getChilds))
            {
                UpdateEntityState(delNode, EntityState.Deleted);
                //DbContext.Entry(delNode).State = EntityState.Deleted;
            }
        }

        public List<T> GetAllNodes<T>(T node, Func<T, IEnumerable<T>> getChilds) where T : class, IEntity
        {
            var allNodes = new List<T>();
            allNodes.Add(node);
            foreach (var childNode in getChilds(node))
            {
                allNodes.AddRange(GetAllNodes(childNode, getChilds));
            }
            return allNodes;
        }

        public List<T> GetTreeNodes<T>(T node, Expression<Func<T, IEnumerable<T>>> getCollection, Func<T, IEnumerable<T>> getChilds)
            where T : class, IEntity
        {
            List<T> treeNodes = new List<T>();
            DbContext.Entry(node).Collection<T>(getCollection).Load();
            treeNodes.Add(node);
            foreach (var childNode in getChilds(node))
            {
                treeNodes.AddRange(GetTreeNodes(childNode, getCollection, getChilds));
            }
            return treeNodes;
        }
        public NodeOrderInfo CalaOrd<T>(T node, Action<T, NodeOrderInfo> headerConfig, Action<T, T> middleConfig,
        Action<T, int> footerConfig, Func<T, IEnumerable<T>> getChilds, NodeOrderInfo nodeOrderInfo)
        {
            headerConfig(node, nodeOrderInfo);
            getChilds(node).ForEach(nd =>
            {
                middleConfig(nd, node);
                nodeOrderInfo.Ord++;
                nodeOrderInfo.InitNum++;
                nodeOrderInfo = CalaOrd(nd, headerConfig, middleConfig, footerConfig, getChilds, nodeOrderInfo);
            });
            nodeOrderInfo.InitNum++;
            footerConfig(node, nodeOrderInfo.InitNum);
            return nodeOrderInfo;
        }
        public void UpdateLocalNodeValues<T>(T node, Func<T, List<T>> getChilds, Action<T> setPropertyValue)
        {
            setPropertyValue(node);
            foreach (var child in getChilds(node))
            {
                UpdateLocalNodeValues(child, getChilds, setPropertyValue);
            }
        }
        #endregion

        #region 单个实体增删改查

        /// <summary>
        /// 获取能跟踪上下文的实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DbSet<T> EntitySet<T>() where T : class, IEntity
        {
            return DbContext.Set<T>();
        }
        /// <summary>
        /// 从上下文中获取指定主键值且没有跟踪上下文的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getKey"></param>
        /// <returns></returns>
        public T FastGetByKeyReadOnly<T>(Func<T, bool> getKey) where T : class, IEntity
        {
            return DbContext.Set<T>().AsNoTracking().FirstOrDefault(getKey);
        }
        /// <summary>
        /// 从上下文中获取指定主键值且没有跟踪上下文的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getKey"></param>
        /// <returns></returns>
        public T GetByKeyReadOnly<T, TPrimaryKey>(Func<T, bool> getKey) where T : class, IEntity<TPrimaryKey>
        {
            return QueryAsReadOnly<T, TPrimaryKey>().FirstOrDefault(getKey);
        }
        /// <summary>
        /// 从上下文中获取指定主键的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">查询主键</param>
        /// <returns></returns>
        public T FastGetByKey<T>(object key) where T : class, IEntity
        {
            return DbContext.Set<T>().Find(key);
        }

        /// <summary>
        /// 从上下文中获取指定主键的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">查询主键</param>
        /// <returns></returns>
        public T GetByKey<T, TPrimaryKey>(object key) where T : class, IEntity<TPrimaryKey>
        {
            return (Query<T, TPrimaryKey>() as DbSet<T>).Find(key);
        }
        /// <summary>
        /// 查询指定类型和主键的实体
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public object FastGetByKey(Type type, object key)
        {
            return DbContext.Find(type, key);
        }
        /// <summary>
        /// 使用当前实体值去更新数据库对应的实体值
        /// </summary>
        ///<param name="entity">数据库存在的实体</param>
        ///<param name="nowEntity">当前的实体</param>
        /// <returns>Void</returns>
        public void UpdateEntityValues<T>(T existEntity, T nowEntity) where T : class, IEntity
        {
            DbContext.Entry(existEntity).CurrentValues.SetValues(nowEntity);
        }
        /// <summary>
        /// 设置实体状态
        /// </summary>
        ///<param name="entity">设置的实体</param>
        ///<param name="entityState">设置实体的状态</param>
        /// <returns>Void</returns>
        public void UpdateEntityState<T>(T entity, EntityState entityState) where T : class, IEntity
        {
            DbContext.Entry(entity).State = entityState;
        }
        /// <summary>
        /// 新增实体到数据库上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">新增实体</param>
        /// <returns></returns>
        public EntityEntry<T> InsertEntity<T>(T entity) where T : class, IEntity
        {
            return DbContext.Set<T>().Add(entity);
        }
        /// <summary>
        /// 批量新增实体类型到数据库上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">实体集合</param>
        public void InsertEntities<T>(params T[] entities) where T : class, IEntity
        {
            DbContext.Set<T>().AddRange(entities);
        }
        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <typeparam name="T">删除实体类型</typeparam>
        /// <param name="entity">删除实体</param>
        /// <returns></returns>
        public EntityEntry<T> DeleteEntity<T>(T entity) where T : class, IEntity
        {
            return DbContext.Set<T>().Remove(entity);
        }
        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">删除实体集合</param>
        public void DeleteEntities<T>(params T[] entities) where T : class, IEntity
        {
            DbContext.Set<T>().RemoveRange(entities);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">更新实体</param>
        /// <returns></returns>
        public EntityEntry<T> UpdateEntity<T>(T entity) where T : class, IEntity
        {
            return DbContext.Set<T>().Update(entity);
        }
        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">更新实体类型集合</param>
        public void UpdateEntities<T>(params T[] entities) where T : IEntity
        {
            DbContext.UpdateRange(entities);
        }

        /// <summary>
        /// 添加或更新实体
        /// 受这些过滤器的影响: 操作过滤器
        /// </summary>
        public virtual void Save<T, TPrimaryKey>(ref T entity, Action<T> update = null) where T : class, IEntity<TPrimaryKey>
        {
            var uow = UnitOfWork;
            update = uow.WrapUpdateMethod<T, TPrimaryKey>(update);
            uow.Context.Save(ref entity, update);
        }

        /// <summary>
        /// 删除实体
        /// 受这些过滤器的影响: 操作过滤器
        /// </summary>
        public virtual void Delete<T, TPrimaryKey>(T entity) where T : class, IEntity<TPrimaryKey>
        {
            var uow = UnitOfWork;
            uow.WrapBeforeDeleteMethod<T, TPrimaryKey>(e => { })(entity);
            uow.Context.Delete(entity);
        }

        /// <summary>
        /// 快速批量保存实体,包含新增,更新实体
        /// </summary>
        /// <typeparam name="T">保存实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">主键类型</typeparam>
        /// <param name="entities">保存的实体集合</param>
        public virtual void FastBatchSave<T, TPrimaryKey>(IEnumerable<T> entities)
            where T : class, IEntity<TPrimaryKey>
        {
            var uow = UnitOfWork;
            uow.Context.FastBatchSave<T, TPrimaryKey>(entities);
        }

        /// <summary>
        /// 批量保存实体
        /// 受这些过滤器的影响: 操作过滤器
        /// </summary>
        public virtual void BatchSave<T, TPrimaryKey>(
            ref IEnumerable<T> entities, Action<T> update = null) where T : class, IEntity<TPrimaryKey>
        {
            var uow = UnitOfWork;
            update = uow.WrapUpdateMethod<T, TPrimaryKey>(update);
            uow.Context.BatchSave(ref entities, update);
        }

        /// <summary>
        /// 批量更新实体
        /// 受这些过滤器的影响: 查询过滤器, 操作过滤器
        /// </summary>
        public virtual long BatchUpdate<T, TPrimaryKey>(
            Expression<Func<T, bool>> predicate, Action<T> update) where T : class, IEntity<TPrimaryKey>
        {
            var uow = UnitOfWork;
            predicate = uow.WrapPredicate<T, TPrimaryKey>(predicate);
            update = uow.WrapUpdateMethod<T, TPrimaryKey>(update);
            return uow.Context.BatchUpdate(predicate, update);
        }

        /// <summary>
        /// 快速批量删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>

        public long FastBatchDelete<T, TPrimaryKey>(Expression<Func<T, bool>> predicate)
            where T : class, IEntity<TPrimaryKey>, new()
        {
            var uow = UnitOfWork;
            return uow.Context.FastBatchDelete<T, TPrimaryKey>(predicate);
        }

        /// <summary>
        /// 批量删除实体
        /// 受这些过滤器的影响: 查询过滤器, 操作过滤器
        /// </summary>
        public virtual long BatchDelete<T, TPrimaryKey>(
            Expression<Func<T, bool>> predicate, Action<T> beforeDelete) where T : class, IEntity<TPrimaryKey>
        {
            var uow = UnitOfWork;
            predicate = uow.WrapPredicate<T, TPrimaryKey>(predicate);
            beforeDelete = uow.WrapBeforeDeleteMethod<T, TPrimaryKey>(beforeDelete);
            return uow.Context.BatchDelete(predicate, beforeDelete);
        }
        /// <summary>
        /// 附加实体到数据库上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">附加的实体</param>
        /// <returns></returns>
        public EntityEntry<T> AttachEntity<T>(T entity) where T : class, IEntity
        {
            return DbContext.Attach(entity);
        }
        /// <summary>
        /// 批量附加实体到数据库上下文
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体集合</param>
        public void AttachEntities<T>(params T[] entities) where T : IEntity
        {
            DbContext.AttachRange(entities);
        }
        /// <summary>
        /// 运行数据库命令
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns></returns>
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return DbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        /// <summary>
        /// 快速从SQL语句查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> FastRawQuery<T>(string query, params object[] parameters)
            where T : class, IEntity
        {
            return FastQuery<T>().FromSql(query, parameters);
        }

        /// <summary>
        /// 从SQL语句查询数据,受查询过滤器的影响
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> RawQuery<T, TPrimaryKey>(string query, params object[] parameters)
            where T : class, IEntity<TPrimaryKey>
        {
            return Query<T, TPrimaryKey>().FromSql(query, parameters);
        }

        /// <summary>
        /// 计算符合条件的实体数量
        /// 受这些过滤器的影响: 查询过滤器
        /// </summary>
        public long FastCount<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            return FastQuery<T>().LongCount();
        }

        /// <summary>
        /// 计算符合条件的实体数量
        /// 受这些过滤器的影响: 查询过滤器
        /// </summary>
        public long Count<T, TPrimaryKey>(Expression<Func<T, bool>> predicate) where T : class, IEntity<TPrimaryKey>
        {
            return Query<T, TPrimaryKey>().LongCount(predicate);
        }

        /// <summary>
        /// 获取符合条件的单个实体
        /// 受这些过滤器的影响: 查询过滤器
        /// </summary>
        public T FastGet<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            return FastQuery<T>().FirstOrDefault(predicate);
        }
        /// <summary>
        /// 获取符合条件的单个实体
        /// 受这些过滤器的影响: 查询过滤器
        /// </summary>
        public T Get<T, TPrimaryKey>(Expression<Func<T, bool>> predicate) where T : class, IEntity<TPrimaryKey>
        {
            return Query<T, TPrimaryKey>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// 获取不跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public IQueryable<T> FastQueryAsReadOnly<T>() where T : class, IEntity
        {
            return DbContext.Set<T>().AsNoTracking().AsQueryable();
        }
        /// <summary>
        /// 获取不跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public IQueryable<T> QueryAsReadOnly<T, TPrimaryKey>() where T : class, IEntity<TPrimaryKey>
        {
            var uow = UnitOfWork;
            var query = uow.Context.Query<T>().AsNoTracking();
            return uow.WrapQuery<T, TPrimaryKey>(query);
        }
        /// <summary>
        /// 获取具有跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> FastQuery<T>() where T : class, IEntity
        {
            return DbContext.Set<T>().AsQueryable();
        }
        /// <summary>
        /// 获取具有包装的查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <returns></returns>
        public IQueryable<T> Query<T, TPrimaryKey>() where T : class, IEntity<TPrimaryKey>
        {
            var uow = UnitOfWork;
            var query = uow.Context.Query<T>();
            return uow.WrapQuery<T, TPrimaryKey>(query);
        }
        /// <summary>
        /// 获取指定查询条件肯于没有跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterCondition">查询条件</param>
        /// <returns></returns>
        public IQueryable<T> FastQuery<T>(Expression<Func<T, bool>> filterCondition) where T : class, IEntity
        {
            return DbContext.Set<T>().AsNoTracking().Where(filterCondition).AsQueryable();
        }
        /// <summary>
        /// 获取指定查询条件肯于没有跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterCondition">查询条件</param>
        /// <returns></returns>
        public IQueryable<T> Query<T, TPrimaryKey>(Expression<Func<T, bool>> filterCondition) where T : class, IEntity<TPrimaryKey>
        {
            var uow = UnitOfWork;
            var query = uow.Context.Query<T>().AsNoTracking().Where(filterCondition);
            return uow.WrapQuery<T, TPrimaryKey>(query);
        }
        /// <summary>
        /// 从数据库上下文中获取实体引用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EntityEntry GetEntry<T>(T entity) where T : class, IEntity
        {
            return DbContext.Entry(entity);
        }
        public PageInfo<object> Query<T, TOrderBy>(int pageIndex, int pageSize, Expression<Func<T, bool>> where,
         Expression<Func<T, TOrderBy>> orderby, Func<IQueryable<T>, List<object>> selector)
         where T : class, IEntity where TOrderBy : class
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            IQueryable<T> query = DbContext.Set<T>();
            if (where != null)
            {
                query = query.Where(where);
            }
            int count = query.Count();

            if (pageIndex * pageSize > count)
            {
                pageIndex = count / pageSize;
            }
            if (count % pageSize > 0)
            {
                pageIndex++;
            }

            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            if (orderby != null)
            {
                query = query.OrderBy(orderby);
            }
            return new PageInfo<object>(pageIndex, pageSize, count, selector(query));
        }
        public List<object> Query<T, TOrderBy>(Expression<Func<T, bool>> where, Expression<Func<T, TOrderBy>> orderby,
         Func<IQueryable<T>, List<object>> selector) where T : class, IEntity
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            IQueryable<T> query = DbContext.Set<T>();
            if (where != null)
            {
                query = query.Where(where);
            }
            if (orderby != null)
            {
                query = query.OrderBy(orderby);
            }
            return selector(query);
        }
        #endregion
    }

    public class NodeOrderInfo
    {
        public int InitNum { get; set; } = 1;
        public int Ord { get; set; } = 1;
    }
    public class PageInfo<TEntity> where TEntity : class
    {
        public PageInfo(int index, int pageSize, int count, List<TEntity> list)
        {
            Index = index;
            PageSize = pageSize;
            Count = count;
            List = list;
        }
        public int Index { get; private set; }
        public int PageSize { get; private set; }
        public int Count { get; private set; }
        public List<TEntity> List { get; private set; }
    }
}
