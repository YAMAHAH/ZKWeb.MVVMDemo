using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Domain.Filters.Interfaces;

namespace InfrastructurePlugins.BaseModule.Domain.Uow.Interfaces
{
    /// <summary>
    /// 工作单元的接口
    /// </summary>
    public interface IUnitOfWork : IRepositoryFactory,IDomainServiceFactory
    {
        /// <summary>
        /// 当前的数据库上下文
        /// 不存在时抛出错误
        /// </summary>
        IDatabaseContext Context { get; }

        /// <summary>
        /// 获取当前活动的UnitOfWork
        /// </summary>
        IActiveUnitOfWork Current { get; }
        /// <summary>
        /// 当前的查询过滤器列表
        /// 不存在时抛出错误
        /// </summary>
        IList<IEntityQueryFilter> QueryFilters { get; set; }

        /// <summary>
        /// 当前的操作过滤器列表
        /// 不存在时抛出错误
        /// </summary>
        IList<IEntityOperationFilter> OperationFilters { get; set; }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> if sayve changes ensure auto record the change history.</param>
        /// <returns>The number of state entries written to the database.</returns>
        int SaveChanges(bool ensureAutoHistory = false);

        /// <summary>
        /// Asynchronously saves all changes made in this unit of work to the database.
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
        Task<int> SaveChangesAsync(bool ensureAutoHistory = false);

        /// <summary>
        /// Executes the specified raw SQL command.
        /// </summary>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of state entities written to database.</returns>
        long ExecuteSqlCommand(string sql, params object[] parameters);

        /// <summary>
        /// Uses raw SQL queries to fetch the specified <typeparamref name="TEntity"/> data.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>An <see cref="IQueryable{T}"/> that contains elements that satisfy the condition specified by raw SQL.</returns>
        IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class, IEntity, new();

        /// <summary>
        /// 在指定的范围内使用工作单元
        /// 工作单元中可以使用相同的上下文和过滤器，并且和其他工作单元隔离
        /// 这个函数可以嵌套使用，嵌套使用时都使用最上层的数据库上下文
        /// </summary>
        /// <returns></returns>
        IDisposable Scope(bool forceNewScope = false);

        void CreateTransactionScope(Action postAction, UnitOfWorkOptions unitOfWorkOptions = null);
        IUnitOfWorkCompleteHandler CreateTransactionScope(UnitOfWorkOptions unitOfWorkOptions = null);


    }
}
