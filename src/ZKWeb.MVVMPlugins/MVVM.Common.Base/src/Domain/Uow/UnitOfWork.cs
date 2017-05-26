using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces;
using ZKWebStandard.Collections;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Uow
{
    /// <summary>
    /// 工作单元
    /// 工作单元用于在一个区域中共享数据库上下文和事务
    /// 工作单元支持过滤器，这是对框架中的数据事件的补充，但不同的是
    /// - 工作单元过滤器用于处理拥有某一特征(例如有创建时间)的所有实体，可以在一定的范围内启用和禁用
    /// - 数据事件用于处理某一类型的实体，全局一直有效且不能禁用
    /// </summary>
    [ExportMany, SingletonReuse]
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// 同一个工作单元区域使用的数据
        /// </summary>
        private class ScopeData : IDisposable, IActiveUnitOfWork, IUnitOfWorkCompleteHandler
        {
            public string Id { get; set; }

            /// <summary>
            /// 数据库上下文
            /// </summary>
            public IDatabaseContext Context { get; set; }
            /// <summary>
            /// 默认的查询过滤器
            /// </summary>
            public IList<IEntityQueryFilter> QueryFilters { get; set; }
            /// <summary>
            /// 默认的操作过滤器
            /// </summary>
            public IList<IEntityOperationFilter> OperationFilters { get; set; }

            private UnitOfWorkOptions _Options;
            public UnitOfWorkOptions Options => _Options;

            /// <summary>
            /// 初始化
            /// </summary>
            public ScopeData(UnitOfWorkOptions unitOfWorkOptions)
            {
                _Options = unitOfWorkOptions;
                Id = Guid.NewGuid().ToString();
                var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
                Context = databaseManager.CreateContext();
                if (unitOfWorkOptions.IsTransactional)
                {
                    Context.BeginTransaction(_Options.IsolationLevel);
                }

                QueryFilters = Application.Ioc.ResolveMany<IEntityQueryFilter>().ToList();
                OperationFilters = Application.Ioc.ResolveMany<IEntityOperationFilter>().ToList();
            }

            public ScopeData()
            {
                Id = Guid.NewGuid().ToString();
                var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
                Context = databaseManager.CreateContext();
                QueryFilters = Application.Ioc.ResolveMany<IEntityQueryFilter>().ToList();
                OperationFilters = Application.Ioc.ResolveMany<IEntityOperationFilter>().ToList();
            }
            /// <summary>
            /// 释放数据
            /// </summary>
            ~ScopeData()
            {
                Dispose();
            }

            /// <summary>
            /// 释放数据
            /// </summary>
            public void Dispose()
            {
                Context?.Dispose();
                Context = null;
                SimpleDisposable?.Dispose();
            }

            public SimpleDisposable SimpleDisposable;
            public void Complete()
            {
                Context.FinishTransaction();
            }

            public Task CompleteAsync()
            {
                this.Complete();
                return Task.FromResult(0);
            }

            public void SaveChanges()
            {
                ((DbContext)Context).SaveChanges();
            }

            public Task SaveChangesAsync()
            {

                return ((DbContext)Context).SaveChangesAsync();
            }
        }

        /// <summary>
        /// 同一个工作单元区域使用的数据
        /// </summary>
        private AsyncLocal<ScopeData> Data { get; set; }

        /// <summary>
        /// 当前的数据库上下文
        /// </summary>
        public IDatabaseContext Context
        {
            get
            {
                var context = Data.Value?.Context;
                if (context == null)
                {
                    throw new InvalidOperationException("Please call Scope() first");
                }
                return context;
            }
        }

        /// <summary>
        /// 当前的查询过滤器列表
        /// </summary>
        public IList<IEntityQueryFilter> QueryFilters
        {
            get
            {
                var filters = Data.Value?.QueryFilters;
                if (filters == null)
                {
                    throw new InvalidOperationException("Please call Scope() first");
                }
                return filters;
            }
            set
            {
                if (Data.Value == null)
                {
                    throw new InvalidOperationException("Please call Scope() first");
                }
                Data.Value.QueryFilters = value ?? throw new ArgumentNullException("value");
            }
        }

        /// <summary>
        /// 当前的保存过滤器列表
        /// </summary>
        public IList<IEntityOperationFilter> OperationFilters
        {
            get
            {
                var filters = Data.Value?.OperationFilters;
                if (filters == null)
                {
                    throw new InvalidOperationException("Please call Scope() first");
                }
                return filters;
            }
            set
            {
                if (Data.Value == null)
                {
                    throw new InvalidOperationException("Please call Scope() first");
                }
                Data.Value.OperationFilters = value ?? throw new ArgumentNullException("value");
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public UnitOfWork()
        {
            //线程内异步数据共享
            Data = new AsyncLocal<ScopeData>();  //ThreadLocal 同步线程本地数据
        }
        /// <summary>
        ///  在指定的范围内使用工作单元
        /// 最外层的工作单元负责创建和销毁数据 
        /// </summary>
        /// <param name="forceNewScope">强制创建新范围</param>
        /// <returns></returns>
        public IDisposable Scope(bool forceNewScope = false)
        {
            var isRootUow = Data.Value == null;
            if (isRootUow)
            {
                var data = new ScopeData();
                Data.Value = data;
                return new SimpleDisposable(() =>
                {
                    data.Dispose();
                    Data.Value = null;
                });
            }

            if (forceNewScope)
            {
                var oldData = Data.Value;
                var data = new ScopeData();
                Data.Value = data;
                return new SimpleDisposable(() =>
                {
                    data.Dispose();
                    if (isRootUow) Data.Value = null; else Data.Value = oldData;
                });
            }
            return new SimpleDisposable(() => { });
        }


        /// <summary>
        /// 根据工作单元配置选项创建范围内使用工作单元,
        /// 如配置选项为空则使用默认选项
        /// </summary>
        /// <param name="unitOfWorkOptions">工作单元选项</param>
        /// <returns></returns>
        public IUnitOfWorkCompleteHandler CreateTransactionScope(UnitOfWorkOptions unitOfWorkOptions = null)
        {
            if (unitOfWorkOptions == null) { unitOfWorkOptions = DefaultUnitOfWorkOptions(); }
            unitOfWorkOptions.IsTransactional = true;
            var isRootUow = Data.Value == null;
            //根工作单元为空则创建新的scopeData实例
            if (isRootUow)
            {
                var data = new ScopeData(unitOfWorkOptions);
                if (isRootUow) Data.Value = data;
                data.SimpleDisposable = new SimpleDisposable(() =>
                {
                    if (isRootUow) Data.Value = null;
                });
                return data;
            }
            //内嵌工作单元开启新的工作单元
            if (unitOfWorkOptions.IsStandalone)
            {
                var oldData = Data.Value;
                var data = new ScopeData(unitOfWorkOptions);
                Data.Value = data;
                data.SimpleDisposable = new SimpleDisposable(() =>
                {
                    if (isRootUow) Data.Value = null; else Data.Value = oldData;
                });
                return data;
            }

            return Data.Value;
        }

        /// <summary>
        /// 根据工作单元配置选项创建范围工作单元,
        /// 在调用传递的方法执行完毕,则提交事务,
        /// 如配置选项为空则使用默认选项
        /// </summary>
        /// <param name="postAction">包装的方法</param>
        /// <param name="unitOfWorkOptions">工作单元选项</param>
        public void CreateTransactionScope(Action postAction, UnitOfWorkOptions unitOfWorkOptions = null)
        {
            if (unitOfWorkOptions == null) { unitOfWorkOptions = DefaultUnitOfWorkOptions(); }
            var isRootUow = Data.Value == null;
            SimpleDisposable simpleDisposable = new SimpleDisposable(() => { });
            try
            {
                if (unitOfWorkOptions == null) { unitOfWorkOptions = DefaultUnitOfWorkOptions(); }
                unitOfWorkOptions.IsTransactional = true;
                if (isRootUow)
                {
                    var data = new ScopeData(unitOfWorkOptions);
                    Data.Value = data;
                    postAction();
                    simpleDisposable = new SimpleDisposable(() =>
                    {
                        data.Dispose();
                        Data.Value = null;
                    });
                }

                if (unitOfWorkOptions.IsStandalone)
                {
                    var oldData = Data.Value;
                    var data = new ScopeData(unitOfWorkOptions);
                    Data.Value = data;
                    postAction();
                    simpleDisposable = new SimpleDisposable(() =>
                    {
                        data.Dispose();
                        if (isRootUow) Data.Value = null; else Data.Value = oldData;
                    });
                }

                if (unitOfWorkOptions.IsTransactional)
                {
                    Data.Value.Context.FinishTransaction();
                }
            }
            catch (Exception ex)
            {
                throw new BadRequestException(new T("工作单元异常."));
            }
            finally
            {
                simpleDisposable?.Dispose();
            }
        }

        /// <summary>
        /// 获取当前线程下的ScopeData,如果一个线程下开启多个实例，则可能会出错
        /// </summary>

        public IActiveUnitOfWork Current => Data.Value;

        private Dictionary<Type, object> repositories;

        /// <summary>
        /// Gets the specified repository for the <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
        public IUnitOfWorkRepository<TEntity, TPrimaryKey> GetRepository<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new UnitOfWorkRepository<TEntity, TPrimaryKey>();
            }

            return (IUnitOfWorkRepository<TEntity, TPrimaryKey>)repositories[type];
        }


        /// <summary>
        /// 默认的工作单元选项
        /// </summary>
        /// <returns></returns>

        private UnitOfWorkOptions DefaultUnitOfWorkOptions()
        {
            return new UnitOfWorkOptions()
            {
                IsStandalone = false,
                Timeout = TimeSpan.FromSeconds(3),
                IsTransactional = true
            };
        }

    }
}


