using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.PagedList;
using InfrastructurePlugins.BaseModule.Domain.Uow.Extensions;
using InfrastructurePlugins.BaseModule.Domain.Uow.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Domain.Uow
{
    public class UnitOfWorkRepository<TEntity, TPrimaryKey> : IUnitOfWorkRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>, new()
    {

        public UnitOfWorkRepository()
        {
        }

        private string xTableName;
        /// <summary>
        /// 实体对应的表名
        /// </summary>
        public string TableName
        {
            get
            {
                if (xTableName == null)
                {
                    xTableName = typeof(TEntity).Name;
                    var handlers = Injector.ResolveMany<IDatabaseInitializeHandler>();
                    handlers.ForEach(h => h.ConvertTableName(ref xTableName));
                    xTableName = xTableName.ToLower();
                }
                return xTableName;
            }
        }

        private IUnitOfWork _unitOfWork;
        private IUnitOfWork UnitOfWork
        {
            get
            {
                if (_unitOfWork == null) _unitOfWork = Injector.Resolve<IUnitOfWork>();
                return _unitOfWork;
            }
        }
        /// <summary>
        /// 获取依赖注入器(容器)
        /// </summary>
        protected virtual IContainer Injector
        {
            get { return ZKWeb.Application.Ioc; }
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
        public void UpdateMany<TDetail, TKey>(
            TEntity existEntity,
            TEntity nowEntity,
            Func<TEntity, List<TDetail>> getChilds,
            Func<TDetail, TKey> getCompareKey,
            Func<TDetail, TDetail, bool> getKey) where TDetail : class, IEntity<TPrimaryKey>, new()
        {
            //更新主体实体
            UpdateValues(existEntity, nowEntity);
            //实体差异比较
            var entityDiff = new EntityDiffer<TDetail, TKey>(getChilds(existEntity), getChilds(nowEntity), getCompareKey);
            //实体删除
            entityDiff.DeletedEntities.ForEach(p => UpdateState(p, EntityState.Deleted));
            //实体新增
            getChilds(existEntity).AddRange(entityDiff.AddedEntities);
            //实体修改
            foreach (var modEntity in entityDiff.ModifiedEntities)
            {
                var oldEntity = getChilds(existEntity).Where(p => getKey(modEntity, p)).FirstOrDefault();
                UpdateValues(oldEntity, modEntity);
            }
        }

        /// <summary>
        /// 更新集合实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="getExistLists">已存在的实体集合</param>
        /// <param name="getNowLists">最新的实体令集合</param>
        /// <param name="getCompareKey">集合实体比较的KEY</param>
        /// <param name="getKey">获取实体的KEY</param>
        public void UpdateMany<T, TKey>(
            List<T> getExistLists, List<T> getNowLists,
            Func<T, TKey> getCompareKey,
            Func<T, T, bool> getKey) where T : class, IEntity<TPrimaryKey>, new()
        {
            var entityDiff = new EntityDiffer<T, TKey>(getExistLists, getNowLists, getCompareKey);
            //删除实体
            entityDiff.DeletedEntities.ForEach(p => UpdateState(p, EntityState.Deleted));
            //新增实体
            getExistLists.AddRange(entityDiff.AddedEntities);
            //修改实体
            foreach (var modEntity in entityDiff.ModifiedEntities)
            {
                var oldEntity = getExistLists.Where(p => getKey(modEntity, p)).FirstOrDefault();
                UpdateValues(oldEntity, modEntity);
            }
        }
        /// <summary>
        /// 更新主从表实体
        /// </summary>
        /// <typeparam name="TMaster">主体类型</typeparam>
        /// <typeparam name="TDetail">明细实体类型</typeparam>
        /// <typeparam name="TKey">明细比较的键值类型</typeparam>
        /// <param name="existEntity">存在数据库的实体</param>
        /// <param name="nowEntity">最新实体</param>
        /// <param name="getLists">获取实体明细的回调函数</param>
        /// <param name="getListCompareKey">实体明细比较的KEY</param>
        /// <param name="getListKey">获取明细的键值</param>
        public void UpdateMany<TDetail, TKey>(
            TEntity existEntity,
            TEntity nowEntity,
            List<Func<TEntity, List<TDetail>>> getLists,
            List<Func<TDetail, TKey>> getListCompareKey,
            List<Func<TDetail, TDetail, bool>> getListKey)
            where TDetail : class, IEntity<TPrimaryKey>, new()
        {
            for (int i = 0; i < getLists.Count; i++)
            {
                UpdateMany(getLists[i](existEntity), getLists[i](nowEntity), getListCompareKey[i], getListKey[i]);
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
        public void UpdateTreeNode<TKey>(
            TEntity existNode,
            TEntity nowNode,
            Func<TEntity, List<TEntity>> getChilds,
            Func<TEntity, TKey> getCompareKey,
            Func<TEntity, TEntity, bool> getEntityKey,
            Func<TEntity, TEntity> getNewNode,
            Action<TEntity, TEntity> getConfig)
        {
            UpdateValues(existNode, nowNode);
            //比较实体
            var entityDiff = new EntityDiffer<TEntity, TKey>(getChilds(existNode), getChilds(nowNode), getCompareKey);
            //删除
            entityDiff.DeletedEntities.ForEach(node => DeleteTreeNode(node, getChilds));
            //新增
            entityDiff.AddedEntities.ForEach(node =>
            {
                var newChildNode = getNewNode(existNode);
                // getConfig(newChildNode, node);
                FastInsert(newChildNode);
                getChilds(existNode).Add(newChildNode);
                AddTreeNode(newChildNode, node, getChilds, getNewNode, getConfig);
            });
            //更新实体
            entityDiff.ModifiedEntities.ForEach(modNode =>
            {
                var existEntity = getChilds(existNode).Where(n => getEntityKey(modNode, n)).FirstOrDefault();
                UpdateTreeNode(existEntity, modNode, getChilds, getCompareKey, getEntityKey, getNewNode, getConfig);
            });
        }

        public void UpdateTreeNode<TDetail, TKey>(
            TEntity existNode,
            TEntity nowNode,
            Func<TEntity, List<TEntity>> getChilds,
            List<Func<TEntity, List<TDetail>>> getDetails,
            List<Func<TDetail, TKey>> getDetailCompareKey,
            Func<TEntity, TEntity, bool> getChildKey,
            List<Func<TDetail, TDetail, bool>> getDetailKey) where TDetail : class, IEntity<TPrimaryKey>, new()
        {
            for (int i = 0; i < getDetails.Count; i++)
            {
                UpdateMany(existNode, nowNode, getDetails[i], getDetailCompareKey[i], getDetailKey[i]);
            }
            foreach (var childNode in getChilds(existNode))
            {
                var newNode = getChilds(nowNode).Find(nd => getChildKey(childNode, nd));
                UpdateTreeNode(childNode, newNode, getChilds, getDetails, getDetailCompareKey, getChildKey, getDetailKey);
            }
        }

        public void AddTreeNode(
            TEntity existNode,
            TEntity nowNode,
            Func<TEntity, IList<TEntity>> getChilds,
            Func<TEntity, TEntity> getNewNode,
            Action<TEntity, TEntity> getConfig)
        {
            //排除主键
            getConfig(existNode, nowNode);
            UpdateValues(existNode, nowNode);

            foreach (var addNode in getChilds(nowNode))
            {
                var newChildNode = getNewNode(existNode);
                FastInsert(newChildNode);
                getChilds(existNode).Add(newChildNode);
                AddTreeNode(newChildNode, addNode, getChilds, getNewNode, getConfig);
            }
        }
        public void DeleteTreeNode(
            TEntity node,
            Func<TEntity,
            IEnumerable<TEntity>> getChilds)
        {
            foreach (var delNode in GetAllNodes(node, getChilds))
            {
                UpdateState(delNode, EntityState.Deleted);
            }
        }

        public List<TEntity> GetAllNodes(TEntity node, Func<TEntity, IEnumerable<TEntity>> getChilds)
        {
            var allNodes = new List<TEntity>();
            allNodes.Add(node);
            foreach (var childNode in getChilds(node))
            {
                allNodes.AddRange(GetAllNodes(childNode, getChilds));
            }
            return allNodes;
        }
        /// <summary>
        /// 获取指定结点ID和根结点ID的结点集合
        /// </summary>
        /// <param name="nodeId">结点ID</param>
        /// <param name="rootId">根结点ID</param>
        /// <returns></returns>
        public List<TEntity> GetTreeNodes(string nodeId, string rootId)
        {
            var allNodes = RawQuery("CALL getTreeNodes({0},{1},{2},{3})",
                TableName, "Id", nodeId, rootId)
                .ToList();
            return allNodes;
        }
        /// <summary>
        /// 获取指定结点所有子结点集合,包括根结点,性能差,未测试
        /// </summary>
        /// <param name="node"></param>
        /// <param name="getCollection"></param>
        /// <param name="getChilds"></param>
        /// <returns></returns>
        public List<TEntity> GetTreeNodes(
            TEntity node,
            Expression<Func<TEntity, IEnumerable<TEntity>>> getCollection,
            Func<TEntity, IEnumerable<TEntity>> getChilds)
        {
            List<TEntity> treeNodes = new List<TEntity>();
            DbContext.Entry(node).Collection<TEntity>(getCollection).Load();
            treeNodes.Add(node);
            foreach (var childNode in getChilds(node))
            {
                treeNodes.AddRange(GetTreeNodes(childNode, getCollection, getChilds));
            }
            return treeNodes;
        }
        public NodeOrderInfo CalaOrd(
            TEntity node,
            Action<TEntity, NodeOrderInfo> headerConfig,
            Action<TEntity, TEntity> middleConfig,
            Action<TEntity, int> footerConfig,
            Func<TEntity, IEnumerable<TEntity>> getChilds,
            NodeOrderInfo nodeOrderInfo)
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
        public void UpdateLocalNodeValues(
            TEntity node,
            Func<TEntity, List<TEntity>> getChilds,
            Action<TEntity> setPropertyValue)
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
        public DbSet<TEntity> EntitySet()
        {
            return DbContext.Set<TEntity>();
        }
        /// <summary>
        /// 从数据库上下文中获取实体入口引用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EntityEntry GetEntry(TEntity entity)
        {
            return DbContext.Entry(entity);
        }
        /// <summary>
        /// 从上下文中获取指定主键值且没有跟踪上下文的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getKey"></param>
        /// <returns></returns>
        public TEntity FastGetByKeyReadOnly(Func<TEntity, bool> getKey)
        {
            return DbContext.Set<TEntity>().AsNoTracking().FirstOrDefault(getKey);
        }
        /// <summary>
        /// 从上下文中获取指定主键值且没有跟踪上下文的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getKey"></param>
        /// <returns></returns>
        public TEntity GetByKeyReadOnly(Func<TEntity, bool> getKey)
        {
            return QueryAsReadOnly().FirstOrDefault(getKey);
        }
        /// <summary>
        /// 从上下文中获取指定主键的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">查询主键</param>
        /// <returns></returns>
        public TEntity FastGetByKey(object key)
        {
            return DbContext.Set<TEntity>().Find(key);
        }

        /// <summary>
        /// 从上下文中获取指定主键的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">查询主键</param>
        /// <returns></returns>
        public TEntity GetByKey(object key)
        {
            return (Query() as DbSet<TEntity>).Find(key);
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
        public void UpdateValues<T>(T existEntity, T nowEntity) where T : class, IEntity<TPrimaryKey>, new()
        {
            DbContext.Entry(existEntity).CurrentValues.SetValues(nowEntity);
        }
        /// <summary>
        /// 设置实体状态
        /// </summary>
        ///<param name="entity">设置的实体</param>
        ///<param name="entityState">设置实体的状态</param>
        /// <returns>Void</returns>
        public void UpdateState<T>(T entity, EntityState entityState) where T : class, IEntity<TPrimaryKey>, new()
        {
            DbContext.Entry(entity).State = entityState;
        }
        /// <summary>
        /// 新增实体到数据库上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">新增实体</param>
        /// <returns></returns>
        public EntityEntry<TEntity> FastInsert(TEntity entity)
        {
            var added = DbContext.Set<TEntity>().Add(entity);
            DbContext.SaveChanges();
            return added;
        }
        /// <summary>
        /// 批量新增实体类型到数据库上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">实体集合</param>
        public void FastInsert(params TEntity[] entities)
        {
            var dbCtx = DbContext;
            dbCtx.Set<TEntity>().AddRange(entities);
            dbCtx.SaveChanges();
        }
        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <typeparam name="T">删除实体类型</typeparam>
        /// <param name="entity">删除实体</param>
        /// <returns></returns>
        public EntityEntry<TEntity> FastDelete(TEntity entity)
        {
            var dbCtx = DbContext;
            var removed = DbContext.Set<TEntity>().Remove(entity);
            dbCtx.SaveChanges();
            return removed;
        }
        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">删除实体集合</param>
        public void FastDelete(params TEntity[] entities)
        {
            var dbCtx = DbContext;
            dbCtx.Set<TEntity>().RemoveRange(entities);
            dbCtx.SaveChanges();
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">更新实体</param>
        /// <returns></returns>
        public EntityEntry<TEntity> FastUpdate(TEntity entity)
        {
            var dbCtx = DbContext;
            var updated = DbContext.Set<TEntity>().Update(entity);
            dbCtx.SaveChanges();
            return updated;
        }
        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">更新实体类型集合</param>
        public void FastUpdate(params TEntity[] entities)
        {
            var dbCtx = DbContext;
            DbContext.UpdateRange(entities);
            dbCtx.SaveChanges();
        }

        /// <summary>
        /// 添加或更新实体
        /// 受这些过滤器的影响: 操作过滤器
        /// </summary>
        public virtual void Upsert(ref TEntity entity, Action<TEntity> update = null)
        {
            var uow = UnitOfWork;
            update = uow.WrapUpdateMethod<TEntity, TPrimaryKey>(update);
            uow.Context.Save(ref entity, update);
        }

        /// <summary>
        /// 删除实体
        /// 受这些过滤器的影响: 操作过滤器
        /// </summary>
        public virtual void Delete(TEntity entity)
        {
            var uow = UnitOfWork;
            uow.WrapBeforeDeleteMethod<TEntity, TPrimaryKey>(e => { })(entity);
            uow.Context.Delete(entity);
        }

        /// <summary>
        /// 快速批量保存实体,包含新增,更新实体
        /// </summary>
        /// <typeparam name="T">保存实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">主键类型</typeparam>
        /// <param name="entities">保存的实体集合</param>
        public virtual void FastUpsert(IEnumerable<TEntity> entities)
        {
            var uow = UnitOfWork;
            uow.Context.FastBatchSave<TEntity, TPrimaryKey>(entities);
        }

        /// <summary>
        /// 批量保存实体
        /// 受这些过滤器的影响: 操作过滤器
        /// </summary>
        public virtual void Upsert(
            ref IEnumerable<TEntity> entities, Action<TEntity> update = null)
        {
            var uow = UnitOfWork;
            update = uow.WrapUpdateMethod<TEntity, TPrimaryKey>(update);
            uow.Context.BatchSave(ref entities, update);
        }

        /// <summary>
        /// 批量更新实体
        /// 受这些过滤器的影响: 查询过滤器, 操作过滤器
        /// </summary>
        public virtual long Update(Expression<Func<TEntity, bool>> predicate, Action<TEntity> update)
        {
            var uow = UnitOfWork;
            predicate = uow.WrapPredicate<TEntity, TPrimaryKey>(predicate);
            update = uow.WrapUpdateMethod<TEntity, TPrimaryKey>(update);
            return uow.Context.BatchUpdate(predicate, update);
        }

        /// <summary>
        /// 整体更新实体
        /// </summary>
        /// <param name="entities"></param>
        public void Update(params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");

            foreach (TEntity entity in entities)
            {
                try
                {
                    var entry = GetEntry(entity);
                    if (entry.State == EntityState.Detached)
                    {
                        Attach(entity);
                        entry.State = EntityState.Modified;
                    }
                }
                catch (InvalidOperationException)
                {
                    TEntity oldEntity = GetByKey(entity.Id);
                    UpdateValues(oldEntity, entity);
                }
            }
        }

        /// <summary>
        /// 按需更新实体
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <param name="entities"></param>
        public void Update(Expression<Func<TEntity, object>> propertyExpression, params TEntity[] entities)
        {
            if (propertyExpression == null) throw new ArgumentNullException("propertyExpression");
            if (entities == null) throw new ArgumentNullException("entities");

            ReadOnlyCollection<MemberInfo> memberInfos = ((dynamic)propertyExpression.Body).Members;

            foreach (TEntity entity in entities)
            {
                try
                {
                    var entry = GetEntry(entity);
                    UpdateState(entity, EntityState.Unchanged);
                    foreach (var memberInfo in memberInfos)
                    {
                        entry.Property(memberInfo.Name).IsModified = true;
                    }
                }
                catch (InvalidOperationException)
                {
                    TEntity originalEntity = EntitySet().Local.Single(m => m.Id.Equals(entity.Id)); //获取数据库实体
                    var existEntry = GetEntry(originalEntity);

                    UpdateValues(originalEntity, entity);
                    UpdateState(originalEntity, EntityState.Unchanged);

                    foreach (var memberInfo in memberInfos)
                    {
                        existEntry.Property(memberInfo.Name).IsModified = true;
                    }
                }
            }
        }
        /// <summary>
        /// 快速批量删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>

        public long FastDelete(Expression<Func<TEntity, bool>> predicate)
        {
            var uow = UnitOfWork;
            return uow.Context.FastBatchDelete<TEntity, TPrimaryKey>(predicate);
        }

        /// <summary>
        /// 批量删除实体
        /// 受这些过滤器的影响: 查询过滤器, 操作过滤器
        /// </summary>
        public virtual long Delete(Expression<Func<TEntity, bool>> predicate, Action<TEntity> beforeDelete)
        {
            var uow = UnitOfWork;
            predicate = uow.WrapPredicate<TEntity, TPrimaryKey>(predicate);
            beforeDelete = uow.WrapBeforeDeleteMethod<TEntity, TPrimaryKey>(beforeDelete);
            return uow.Context.BatchDelete(predicate, beforeDelete);
        }
        /// <summary>
        /// 附加实体到数据库上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">附加的实体</param>
        /// <returns></returns>
        public EntityEntry<TEntity> Attach(TEntity entity)
        {
            return DbContext.Attach(entity);
        }
        /// <summary>
        /// 批量附加实体到数据库上下文
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体集合</param>
        public void Attach(params TEntity[] entities)
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
        public IEnumerable<TEntity> FastRawQuery(string query, params object[] parameters)

        {
            return FastQuery().FromSql(query, parameters);
        }

        /// <summary>
        /// 从SQL语句查询数据,受查询过滤器的影响
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> RawQuery(string query, params object[] parameters)

        {
            return Query().FromSql(query, parameters);
        }

        /// <summary>
        /// 计算符合条件的实体数量
        /// 受这些过滤器的影响: 查询过滤器
        /// </summary>
        public long FastCount(Expression<Func<TEntity, bool>> predicate)
        {
            return FastQuery().LongCount();
        }

        /// <summary>
        /// 计算符合条件的实体数量
        /// 受这些过滤器的影响: 查询过滤器
        /// </summary>
        public long Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Query().LongCount(predicate);
        }

        /// <summary>
        /// 获取符合条件的单个实体
        /// 受这些过滤器的影响: 查询过滤器
        /// </summary>
        public TEntity FastGet(Expression<Func<TEntity, bool>> predicate)
        {
            return FastQuery().FirstOrDefault(predicate);
        }
        /// <summary>
        /// 获取符合条件的单个实体
        /// 受这些过滤器的影响: 查询过滤器
        /// </summary>
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Query().FirstOrDefault(predicate);
        }

        /// <summary>
        /// 获取不跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> FastQueryAsReadOnly()
        {
            return DbContext.Set<TEntity>().AsNoTracking().AsQueryable();
        }
        /// <summary>
        /// 获取不跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> QueryAsReadOnly()
        {
            var uow = UnitOfWork;
            var query = uow.Context.Query<TEntity>().AsNoTracking();
            return uow.WrapQuery<TEntity, TPrimaryKey>(query);
        }
        /// <summary>
        /// 获取具有跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> FastQuery()
        {
            return DbContext.Set<TEntity>().AsQueryable();
        }
        /// <summary>
        /// 获取具有包装的查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> Query()
        {
            var uow = UnitOfWork;
            var query = uow.Context.Query<TEntity>();
            return uow.WrapQuery<TEntity, TPrimaryKey>(query);
        }
        /// <summary>
        /// 获取指定查询条件肯于没有跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterCondition">查询条件</param>
        /// <returns></returns>
        public IQueryable<TEntity> FastQuery(Expression<Func<TEntity, bool>> filterCondition)
        {
            return DbContext.Set<TEntity>().AsNoTracking().Where(filterCondition).AsQueryable();
        }
        /// <summary>
        /// 获取指定查询条件肯于没有跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterCondition">查询条件</param>
        /// <returns></returns>
        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filterCondition)
        {
            var uow = UnitOfWork;
            var query = uow.Context.Query<TEntity>().AsNoTracking().Where(filterCondition);
            return uow.WrapQuery<TEntity, TPrimaryKey>(query);
        }

        public PageInfo<object> Query<TOrderBy>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> where,
         Expression<Func<TEntity, TOrderBy>> orderby, Func<IQueryable<TEntity>, List<object>> selector)
          where TOrderBy : class
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

            IQueryable<TEntity> query = DbContext.Set<TEntity>();
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
        public List<object> Query<TOrderBy>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOrderBy>> orderby,
         Func<IQueryable<TEntity>, List<object>> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            IQueryable<TEntity> query = DbContext.Set<TEntity>();
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

        /// <summary>
        /// 获取页码数据
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderBy">排序函数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="disableTracking">禁用实体上下文追踪</param>
        /// <param name="include">包含的导航集合</param>
        /// <returns></returns>
        public IPagedList<TEntity> GetPagedList(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int pageIndex = 0, int pageSize = 20,
            bool disableTracking = true,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = Query();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedList(pageIndex, pageSize);
            }
            else
            {
                return query.ToPagedList(pageIndex, pageSize);
            }
        }

        /// <summary>
        /// 获取页码数据
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderBy">排序函数</param>
        /// <param name="include">包含的导航集合</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页的数量</param>
        /// <param name="disableTracking">实体上下文追踪</param>
        /// <param name="cancellationToken">任务取消</param>
        /// <returns></returns>
        public Task<IPagedList<TEntity>> GetPagedListAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int pageIndex = 0, int pageSize = 20,
            bool disableTracking = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<TEntity> query = Query();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
            else
            {
                return query.ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
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
