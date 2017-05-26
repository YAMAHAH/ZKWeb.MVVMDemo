using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces
{
    public interface IUnitOfWorkRepository<TEntity1, TPrimaryKey1> where TEntity1 : class, IEntity<TPrimaryKey1>
    {
        /// <summary>
        /// 获取能跟踪上下文的实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        DbSet<T> EntitySet<T>() where T : class, IEntity;
        /// <summary>
        /// 从上下文中获取指定主键值且没有跟踪上下文的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getKey"></param>
        /// <returns></returns>
        T GetByKeyReadOnly<T>(Func<T, bool> getKey) where T : class, IEntity;
        /// <summary>
        /// 从上下文中获取指定主键的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">查询主键</param>
        /// <returns></returns>
        T GetByKey<T>(object key) where T : class, IEntity;

        /// <summary>
        /// 查询指定类型和主键的实体
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        object GetByKey(Type type, object key);

        /// <summary>
        /// 使用当前实体值去更新数据库对应的实体值
        /// </summary>
        ///<param name="entity">数据库存在的实体</param>
        ///<param name="nowEntity">当前的实体</param>
        /// <returns>Void</returns>
        void UpdateEntityValues<T>(T existEntity, T nowEntity) where T : class, IEntity;

        /// <summary>
        /// 设置实体状态
        /// </summary>
        ///<param name="entity">设置的实体</param>
        ///<param name="entityState">设置实体的状态</param>
        /// <returns>Void</returns>
        void UpdateEntityState<T>(T entity, EntityState entityState) where T : class, IEntity;

        /// <summary>
        /// 新增实体到数据库上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">新增实体</param>
        /// <returns></returns>
        EntityEntry<T> InsertEntity<T>(T entity) where T : class, IEntity;

        /// <summary>
        /// 批量新增实体类型到数据库上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">实体集合</param>
        void InsertEntities<T>(params T[] entities) where T : class, IEntity;

        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <typeparam name="T">删除实体类型</typeparam>
        /// <param name="entity">删除实体</param>
        /// <returns></returns>
        EntityEntry<T> DeleteEntity<T>(T entity) where T : class, IEntity;

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">删除实体集合</param>
        void DeleteEntities<T>(params T[] entities) where T : class, IEntity;

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">更新实体</param>
        /// <returns></returns>
        EntityEntry<T> UpdateEntity<T>(T entity) where T : class, IEntity;

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">更新实体类型集合</param>
        void UpdateEntities<T>(params T[] entities) where T : IEntity;

        /// <summary>
        /// 附加实体到数据库上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">附加的实体</param>
        /// <returns></returns>
        EntityEntry<T> AttachEntity<T>(T entity) where T : class, IEntity;

        /// <summary>
        /// 批量附加实体到数据库上下文
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体集合</param>
        void AttachEntities<T>(params T[] entities) where T : IEntity;

        /// <summary>
        /// 运行数据库命令
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        /// <summary>
        /// 获取不跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        IQueryable<T> QueryAsReadOnly<T>() where T : class, IEntity;

        /// <summary>
        /// 获取具有跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> Query<T>() where T : class, IEntity;

        /// <summary>
        /// 获取指定查询条件肯于没有跟踪上下文的查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterCondition">查询条件</param>
        /// <returns></returns>
        IQueryable<T> Query<T>(Expression<Func<T, bool>> filterCondition) where T : class, IEntity;

        /// <summary>
        /// 从数据库上下文中获取实体引用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityEntry GetEntry<T>(T entity) where T : class, IEntity;
    }
}
