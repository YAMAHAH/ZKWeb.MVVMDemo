using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.PagedList;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces
{
    public interface IUnitOfWorkRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, new()
    {
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
        void UpdateMany<TDetail, TKey>(
            TEntity existEntity,
            TEntity nowEntity,
            Func<TEntity, List<TDetail>> getChilds,
            Func<TDetail, TKey> getCompareKey,
            Func<TDetail, TDetail, bool> getKey)
            where TDetail : class, IEntity<TPrimaryKey>, new();

        /// <summary>
        /// 更新集合实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="getExistLists">已存在的实体集合</param>
        /// <param name="getNowLists">最新的实体令集合</param>
        /// <param name="getCompareKey">集合实体比较的KEY</param>
        /// <param name="getKey">获取实体的KEY</param>
        void UpdateMany<T, TKey>(
            List<T> getExistLists, List<T> getNowLists,
            Func<T, TKey> getCompareKey,
            Func<T, T, bool> getKey) where T : class, IEntity<TPrimaryKey>, new();

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
        void UpdateMany<TDetail, TKey>(
            TEntity existEntity,
            TEntity nowEntity,
            List<Func<TEntity, List<TDetail>>> getLists,
            List<Func<TDetail, TKey>> getListCompareKey,
            List<Func<TDetail, TDetail, bool>> getListKey)
            where TDetail : class, IEntity<TPrimaryKey>, new();

        #endregion
        #region 结点增删改查
        void UpdateTreeNode<TKey>(
            TEntity existNode,
            TEntity nowNode,
            Func<TEntity, List<TEntity>> getChilds,
            Func<TEntity, TKey> getCompareKey,
            Func<TEntity, TEntity, bool> getEntityKey,
            Func<TEntity, TEntity> getNewNode,
            Action<TEntity, TEntity> getConfig);

        void UpdateTreeNode<TDetail, TKey>(
           TEntity existNode,
           TEntity nowNode,
           Func<TEntity, List<TEntity>> getChilds,
           List<Func<TEntity, List<TDetail>>> getDetails,
           List<Func<TDetail, TKey>> getDetailCompareKey,
           Func<TEntity, TEntity, bool> getChildKey,
           List<Func<TDetail, TDetail, bool>> getDetailKey) where TDetail : class, IEntity<TPrimaryKey>, new();

        void AddTreeNode(TEntity existNode, TEntity nowNode,
                Func<TEntity, IList<TEntity>> getChilds,
                Func<TEntity, TEntity> getNewNode,
                Action<TEntity, TEntity> getConfig);
        void DeleteTreeNode(
            TEntity node,
            Func<TEntity,
            IEnumerable<TEntity>> getChilds);
        List<TEntity> GetAllNodes(TEntity node, Func<TEntity, IEnumerable<TEntity>> getChilds);
        List<TEntity> GetTreeNodes(
            TEntity node,
            Expression<Func<TEntity, IEnumerable<TEntity>>> getCollection,
            Func<TEntity, IEnumerable<TEntity>> getChilds);
        NodeOrderInfo CalaOrd(
            TEntity node,
            Action<TEntity, NodeOrderInfo> headerConfig,
            Action<TEntity, TEntity> middleConfig,
            Action<TEntity, int> footerConfig,
            Func<TEntity, IEnumerable<TEntity>> getChilds,
            NodeOrderInfo nodeOrderInfo);
        void UpdateLocalNodeValues(
            TEntity node,
            Func<TEntity, List<TEntity>> getChilds,
            Action<TEntity> setPropertyValue);
        #endregion

        #region 实体增删改查
        /// <summary>
        /// 获取能跟踪上下文的实体集合
        /// </summary>
        /// <returns></returns>
        DbSet<TEntity> EntitySet();
        /// <summary>
        /// 从上下文中获取指定主键值且没有跟踪上下文的实体
        /// </summary>
        /// <param name="getKey"></param>
        /// <returns></returns>
        TEntity FastGetByKeyReadOnly(Func<TEntity, bool> getKey);
        /// <summary>
        /// 从上下文中获取指定主键值且没有跟踪上下文的实体
        /// </summary>
        /// <param name="getKey"></param>
        /// <returns></returns>
        TEntity GetByKeyReadOnly(Func<TEntity, bool> getKey);
        /// <summary>
        /// 从上下文中获取指定主键的实体
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TEntity FastGetByKey(object key);
        /// <summary>
        /// 从上下文中获取指定主键的实体
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TEntity GetByKey(object key);
        /// <summary>
        /// 查询指定类型和主键的实体
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        object FastGetByKey(Type type, object key);
        /// <summary>
        /// 使用当前实体值去更新数据库对应的实体值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="existEntity"></param>
        /// <param name="nowEntity"></param>
        void UpdateValues<T>(T existEntity, T nowEntity) where T : class, IEntity<TPrimaryKey>, new();
        /// <summary>
        /// 设置实体状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="entityState"></param>
        void UpdateState<T>(T entity, EntityState entityState) where T : class, IEntity<TPrimaryKey>, new();
        /// <summary>
        /// 新增实体到数据库上下文
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityEntry<TEntity> FastInsert(TEntity entity);
        /// <summary>
        /// 批量新增实体类型到数据库上下文
        /// </summary>
        /// <param name="entities"></param>
        void FastInsert(params TEntity[] entities);
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityEntry<TEntity> FastDelete(TEntity entity);
        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities"></param>
        void FastDelete(params TEntity[] entities);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityEntry<TEntity> FastUpdate(TEntity entity);
        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities"></param>
        void FastUpdate(params TEntity[] entities);

        /// <summary>
        /// 添加或更新实体
        /// 受这些过滤器的影响: 操作过滤器
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="update"></param>
        void Upsert(ref TEntity entity, Action<TEntity> update = null);

        /// <summary>
        /// 删除实体
        /// 受这些过滤器的影响: 操作过滤器
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// 快速批量保存实体,包含新增,更新实体
        /// </summary>
        /// <param name="entities">保存的实体集合</param>
        void FastUpsert(IEnumerable<TEntity> entities);
        /// <summary>
        ///批量保存实体
        /// 受这些过滤器的影响: 操作过滤器
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="update"></param>
        void Upsert(ref IEnumerable<TEntity> entities, Action<TEntity> update = null);

        /// <summary>
        /// 批量更新实体
        /// 受这些过滤器的影响: 查询过滤器, 操作过滤器
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        long Update(Expression<Func<TEntity, bool>> predicate, Action<TEntity> update);

        /// <summary>
        /// 按需更新
        /// </summary>
        /// <param name="propertyExpression">更新的属性</param>
        /// <param name="entities">更新的实体</param>
        void Update(Expression<Func<TEntity, object>> propertyExpression, params TEntity[] entities);
        /// <summary>
        /// 整体更新
        /// </summary>
        /// <param name="entities">更新的实体</param>
        void Update(params TEntity[] entities);

        /// <summary>
        /// 快速批量删除实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        long FastDelete(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 批量删除实体
        /// 受这些过滤器的影响: 查询过滤器, 操作过滤器
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="beforeDelete"></param>
        /// <returns></returns>
        long Delete(Expression<Func<TEntity, bool>> predicate, Action<TEntity> beforeDelete);
        /// <summary>
        /// 附加实体到数据库上下文
        /// </summary>
        /// <param name="entity">附加实体</param>
        /// <returns>附加实体入口引用</returns>
        EntityEntry<TEntity> Attach(TEntity entity);
        /// <summary>
        /// 批量附加实体到数据库上下文
        /// </summary>
        /// <param name="entities">实体集合</param>
        void Attach(params TEntity[] entities);
        /// <summary>
        /// 运行数据库命令
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        /// <summary>
        /// 快速从SQL语句查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FastRawQuery(string query, params object[] parameters);

        /// <summary>
        /// 从SQL语句查询数据,受查询过滤器的影响
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<TEntity> RawQuery(string query, params object[] parameters);


        /// <summary>
        /// 计算符合条件的实体数量
        /// 受这些过滤器的影响: 查询过滤器
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        long FastCount(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 计算符合条件的实体数量
        /// 受这些过滤器的影响: 查询过滤器
        /// </summary>
        long Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 获取符合条件的单个实体
        /// 受这些过滤器的影响: 查询过滤器
        /// </summary>
        TEntity FastGet(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 获取符合条件的单个实体
        /// 受这些过滤器的影响: 查询过滤器
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 获取不跟踪上下文的查询
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> FastQueryAsReadOnly();
        /// <summary>
        /// 获取不跟踪上下文的查询
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> QueryAsReadOnly();
        /// <summary>
        /// 获取具有跟踪上下文的查询
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> FastQuery();
        /// <summary>
        /// 获取具有包装的查询
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> Query();
        /// <summary>
        /// 获取指定查询条件肯于没有跟踪上下文的查询
        /// </summary>
        /// <param name="filterCondition"></param>
        /// <returns></returns>
        IQueryable<TEntity> FastQuery(Expression<Func<TEntity, bool>> filterCondition);

        /// <summary>
        /// 获取指定查询条件肯于没有跟踪上下文的查询
        /// </summary>
        /// <param name="filterCondition"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filterCondition);

        /// <summary>
        /// 从数据库上下文中获取实体引用
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityEntry GetEntry(TEntity entity);

        #endregion
        #region 分页
        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderBy">排序函数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="disableTracking">ture禁用跟踪上下文,否则不跟踪</param>
        /// <param name="include">包含导航属性函数</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        IPagedList<TEntity> GetPagedList(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20,
            bool disableTracking = true,
            Func<IQueryable<TEntity>,
            IIncludableQueryable<TEntity, object>> include = null);

        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        Task<IPagedList<TEntity>> GetPagedListAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>,
                IIncludableQueryable<TEntity, object>> include = null,
            int pageIndex = 0,
            int pageSize = 20,
            bool disableTracking = true,
            CancellationToken cancellationToken = default(CancellationToken));
        #endregion
    }
}
