using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces;
using ZKWebStandard.Extensions;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases
{
    public static class DomainServiceExtensions
    {
        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        private static DbContext DbContext
        {
            get { return (DbContext)ZKWeb.Application.Ioc.Resolve<IUnitOfWork>().Context; }
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
        public static void UpdateValues<T, T1, TKey>(this IUnitOfWork unitOfWork, T existEntity, T nowEntity, Func<T, List<T1>> getChilds,
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

        public static void UpdateValues<T, TKey>(this IUnitOfWork unitOfWork, List<T> getExistLists, List<T> getNowLists, Func<T, TKey> getCompareKey,
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
        public static void UpdateValues<TEntity, TPrimaryKey, T, TListEntity, TKey>(this IUnitOfWork unitOfWork, T existEntity, T nowEntity, List<Func<T, List<TListEntity>>> getLists,
         List<Func<TListEntity, TKey>> getListCompareKey, List<Func<TListEntity, TListEntity, bool>> getListKey)
         where T : class, IEntity where TListEntity : class, IEntity
            where TEntity : class
        {
            for (int i = 0; i < getLists.Count; i++)
            {
                unitOfWork.UpdateValues(getLists[i](existEntity), getLists[i](nowEntity), getListCompareKey[i], getListKey[i]);
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
        public static void UpdateNodeValues<T, TKey>(this IUnitOfWork unitOfWork, T existNode, T nowNode, Func<T, List<T>> getChilds, Func<T, TKey> getCompareKey,
         Func<T, T, bool> getEntityKey, Func<T, T> getNewNode, Action<T, T> getConfig) where T : class, IEntity
        {
            UpdateEntityValues(existNode, nowNode);
            var entityDiff = new EntityDiff<T, TKey>(getChilds(existNode), getChilds(nowNode), getCompareKey);
            //删除
            entityDiff.DeletedEntities.ForEach(node => unitOfWork.DeleteNode(node, getChilds));
            //新增
            entityDiff.AddedEntities.ForEach(node =>
            {
                var newChildNode = getNewNode(existNode);
                // getConfig(newChildNode, node);
                InsertEntity(newChildNode);
                getChilds(existNode).Add(newChildNode);
                unitOfWork.AddNodeValues(newChildNode, node, getChilds, getNewNode, getConfig);
            });
            //更新
            entityDiff.ModifiedEntities.ForEach(modNode =>
            {
                var existEntity = getChilds(existNode).Where(n => getEntityKey(modNode, n)).FirstOrDefault();
                unitOfWork.UpdateNodeValues(existEntity, modNode, getChilds, getCompareKey, getEntityKey, getNewNode, getConfig);
            });
        }

        public static void UpdateNodeDetailValues<T, TDetail, TKey>(this IUnitOfWork unitOfWork,T existNode, T nowNode, Func<T, List<T>> getChilds,
         List<Func<T, List<TDetail>>> getDetails, List<Func<TDetail, TKey>> getDetailCompareKey, Func<T, T, bool> getChildKey,
          List<Func<TDetail, TDetail, bool>> getDetailKey) where T : class, IEntity where TDetail : class, IEntity
        {
            for (int i = 0; i < getDetails.Count; i++)
            {
                unitOfWork.UpdateValues(existNode, nowNode, getDetails[i], getDetailCompareKey[i], getDetailKey[i]);
            }
            foreach (var childNode in getChilds(existNode))
            {
                var newNode = getChilds(nowNode).Find(nd => getChildKey(childNode, nd));
                unitOfWork.UpdateNodeDetailValues(childNode, newNode, getChilds, getDetails, getDetailCompareKey, getChildKey, getDetailKey);
            }
        }

        public static void AddNodeValues<T>(this IUnitOfWork unitOfWork,T existNode, T nowNode, Func<T, IList<T>> getChilds,
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
                unitOfWork.AddNodeValues(newChildNode, addNode, getChilds, getNewNode, getConfig);
            }
        }
        public static void DeleteNode<T>(this IUnitOfWork unitOfWork,T node, Func<T, IEnumerable<T>> getChilds) where T : class, IEntity
        {
            foreach (var delNode in unitOfWork.GetAllNodes(node, getChilds))
            {
                DbContext.Entry(delNode).State = EntityState.Deleted;
            }
        }

        public static List<T> GetAllNodes<T>(this IUnitOfWork unitOfWork,T node, Func<T, IEnumerable<T>> getChilds) where T : class, IEntity
        {
            var allNodes = new List<T>();
            allNodes.Add(node);
            foreach (var childNode in getChilds(node))
            {
                allNodes.AddRange(unitOfWork.GetAllNodes(childNode, getChilds));
            }
            return allNodes;
        }

        public static List<T> GetTreeNodes<T>(this IUnitOfWork unitOfWork,T node, Expression<Func<T, IEnumerable<T>>> getCollection, Func<T, IEnumerable<T>> getChilds)
            where T : class, IEntity
        {
            List<T> treeNodes = new List<T>();
            DbContext.Entry(node).Collection<T>(getCollection).Load();
            treeNodes.Add(node);
            foreach (var childNode in getChilds(node))
            {
                treeNodes.AddRange(unitOfWork.GetTreeNodes(childNode, getCollection, getChilds));
            }
            return treeNodes;
        }
        public static NodeOrderInfo CalaOrd<T>(this IUnitOfWork unitOfWork,T node, Action<T, NodeOrderInfo> headerConfig, Action<T, T> middleConfig,
        Action<T, int> footerConfig, Func<T, IEnumerable<T>> getChilds, NodeOrderInfo nodeOrderInfo)
        {
            headerConfig(node, nodeOrderInfo);
            getChilds(node).ForEach(nd =>
            {
                middleConfig(nd, node);
                nodeOrderInfo.Ord++;
                nodeOrderInfo.InitNum++;
                nodeOrderInfo = unitOfWork.CalaOrd(nd, headerConfig, middleConfig, footerConfig, getChilds, nodeOrderInfo);
            });
            nodeOrderInfo.InitNum++;
            footerConfig(node, nodeOrderInfo.InitNum);
            return nodeOrderInfo;
        }
        public static void UpdateLocalNodeValues<T>(this IUnitOfWork unitOfWork,T node, Func<T, List<T>> getChilds, Action<T> setPropertyValue)
        {
            setPropertyValue(node);
            foreach (var child in getChilds(node))
            {
               unitOfWork.UpdateLocalNodeValues(child, getChilds, setPropertyValue);
            }
        }
        #endregion

        #region 单个实体增删改查

        /// <summary>
        /// 获取能跟踪上下文的实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DbSet<T> EntitySet<T>() where T : class, IEntity
        {
            return DbContext.Set<T>();
        }
        /// <summary>
        /// 从上下文中获取指定主键值且没有跟踪上下文的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getKey"></param>
        /// <returns></returns>
        public static T GetByKeyReadOnly<T>(Func<T, bool> getKey) where T : class, IEntity
        {
            return DbContext.Set<T>().AsNoTracking().FirstOrDefault(getKey);
        }
        /// <summary>
        /// 从上下文中获取指定主键的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">查询主键</param>
        /// <returns></returns>
        public static T GetByKey<T>(object key) where T : class, IEntity
        {
            return DbContext.Set<T>().Find(key);
        }
        /// <summary>
        /// 查询指定类型和主键的实体
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public static object GetByKey(Type type, object key)
        {
            return DbContext.Find(type, key);
        }
        /// <summary>
        /// 使用当前实体值去更新数据库对应的实体值
        /// </summary>
        ///<param name="entity">数据库存在的实体</param>
        ///<param name="nowEntity">当前的实体</param>
        /// <returns>Void</returns>
        public static void UpdateEntityValues<T>(T existEntity, T nowEntity) where T : class, IEntity
        {
            DbContext.Entry(existEntity).CurrentValues.SetValues(nowEntity);
        }
        /// <summary>
        /// 设置实体状态
        /// </summary>
        ///<param name="entity">设置的实体</param>
        ///<param name="entityState">设置实体的状态</param>
        /// <returns>Void</returns>
        public static void UpdateEntityState<T>(T entity, EntityState entityState) where T : class, IEntity
        {
            DbContext.Entry(entity).State = entityState;
        }
        /// <summary>
        /// 新增实体到数据库上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">新增实体</param>
        /// <returns></returns>
        public static EntityEntry<T> InsertEntity<T>(T entity) where T : class, IEntity
        {
            return DbContext.Set<T>().Add(entity);
        }
        /// <summary>
        /// 批量新增实体类型到数据库上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">实体集合</param>
        public static void InsertEntities<T>(params T[] entities) where T : class, IEntity
        {
            DbContext.Set<T>().AddRange(entities);
        }
        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <typeparam name="T">删除实体类型</typeparam>
        /// <param name="entity">删除实体</param>
        /// <returns></returns>
        public static EntityEntry<T> DeleteEntity<T>(T entity) where T : class, IEntity
        {
            return DbContext.Set<T>().Remove(entity);
        }
        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">删除实体集合</param>
        public static void DeleteEntities<T>(params T[] entities) where T : class, IEntity
        {
            DbContext.Set<T>().RemoveRange(entities);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">更新实体</param>
        /// <returns></returns>
        public static EntityEntry<T> UpdateEntity<T>(T entity) where T : class, IEntity
        {
            return DbContext.Set<T>().Update(entity);
        }
        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">更新实体类型集合</param>
        public static void UpdateEntities<T>(params T[] entities) where T : IEntity
        {
            DbContext.UpdateRange(entities);
        }
        /// <summary>
        /// 附加实体到数据库上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">附加的实体</param>
        /// <returns></returns>
        public static EntityEntry<T> AttachEntity<T>(T entity) where T : class, IEntity
        {
            return DbContext.Attach(entity);
        }
        /// <summary>
        /// 批量附加实体到数据库上下文
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体集合</param>
        public static void AttachEntities<T>(params T[] entities) where T : IEntity
        {
            DbContext.AttachRange(entities);
        }
        /// <summary>
        /// 运行数据库命令
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns></returns>
        public static int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return DbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        /// <summary>
        /// 获取不跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public static IQueryable<T> QueryAsReadOnly<T>() where T : class, IEntity
        {
            return DbContext.Set<T>().AsNoTracking().AsQueryable();
        }
        /// <summary>
        /// 获取具有跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> Query<T>() where T : class, IEntity
        {
            return DbContext.Set<T>().AsQueryable();
        }
        /// <summary>
        /// 获取指定查询条件肯于没有跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterCondition">查询条件</param>
        /// <returns></returns>
        public static IQueryable<T> Query<T>(Expression<Func<T, bool>> filterCondition) where T : class, IEntity
        {
            return DbContext.Set<T>().AsNoTracking().Where(filterCondition).AsQueryable();
        }
        /// <summary>
        /// 从数据库上下文中获取实体引用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static EntityEntry GetEntry<T>(T entity) where T : class, IEntity
        {
            return DbContext.Entry(entity);
        }
        public static PageInfo<object> Query<T, TOrderBy>(int pageIndex, int pageSize, Expression<Func<T, bool>> where,
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
        public static List<object> Query<T, TOrderBy>(Expression<Func<T, bool>> where, Expression<Func<T, TOrderBy>> orderby,
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
