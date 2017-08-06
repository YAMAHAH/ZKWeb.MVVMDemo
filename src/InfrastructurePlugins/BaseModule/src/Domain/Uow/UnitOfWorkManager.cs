using System;
using InfrastructurePlugins.BaseModule.Domain.Uow.Interfaces;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Domain.Uow
{
    /// <summary>
    /// 工作单元管理
    /// 使用方法
    /// var unitOfWorkManager = Application.Ioc.Resolve<IUnitOfWorkManager>();
    /// var service2 = Application.Ioc.Resolve<IDomainService<ExampleTable, long>>();
    /// using (var unitOfWork = unitOfWorkManager.Begin())
    /// {
    ////  var data = new ExampleTable() { Name = "name" };
    ///   service2.Save(ref data); //保存数据,但并没有提交到数据库
    ///   unitOfWork.Complete();
    ///  }
    /// </summary>
    [ExportMany, SingletonReuse]
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        public IActiveUnitOfWork Current => unitOfWorkProvider.Current;

        private IUnitOfWork unitOfWorkProvider => ZKWeb.Application.Ioc.Resolve<IUnitOfWork>();

        /// <summary>
        /// 工作单元开始事务,使用默认的工作单元选项
        /// </summary>
        /// <returns></returns>
        public IUnitOfWorkCompleteHandler Begin()
        {
            return Begin(null);
        }
        public IUnitOfWorkCompleteHandler Begin(UnitOfWorkOptions options)
        {
            var scope = unitOfWorkProvider.CreateTransactionScope(options);
            return scope;
        }
        public IDisposable CreateUnitOfWork(bool forceNewScope = false)
        {
            return unitOfWorkProvider.Scope(forceNewScope);
        }
        public void CreateUnitOfWorkScope(Action postAction, UnitOfWorkOptions unitOfWorkOptions = null)
        {
            unitOfWorkProvider.CreateTransactionScope(postAction, unitOfWorkOptions);
        }
    }
    //public IActionResult Uow()
    //{
    //    // insert data
    //    var uow = Application.Ioc.Resolve<IUnitOfWork>();
    //    var service = Application.Ioc.Resolve<IDomainService<ExampleTable, long>>();
    //    string name = RandomUtils.RandomString(5) + ":" + Thread.CurrentThread.ManagedThreadId.ToString();

    //    uow.CreateTransactionScope(() =>
    //    {
    //        var thid = Thread.CurrentThread.ManagedThreadId.ToString();
    //        var data = new ExampleTable() { Name = name + "新线程开启事务范围->1" };
    //        service.Save(ref data);
    //        Func<Task> act = async () =>
    //        {
    //            await RunAsync();
    //        };
    //        act().Wait();
    //        Thread t = new Thread(new ThreadStart(() =>
    //        {
    //            var uow2 = Application.Ioc.Resolve<IUnitOfWork>();
    //            var service2 = Application.Ioc.Resolve<IDomainService<ExampleTable, long>>();
    //            var thid2 = Thread.CurrentThread.ManagedThreadId.ToString();
    //            try
    //            {
    //                using (uow2.Scope(true))
    //                {
    //                    var data2 = new ExampleTable()
    //                    {
    //                        Name = name + "新线程开启无事务范围->4" +
    //                        Thread.CurrentThread.ManagedThreadId.ToString() + ":" +
    //                        object.ReferenceEquals(uow.Context, uow.Context).ToString()
    //                    };

    //                    service2.Save(ref data2);
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                throw;
    //            }
    //        }));
    //        t.IsBackground = true;
    //        t.Start();

    //    }, new UnitOfWorkOptions() { IsTransactional = true });

    //    using (uow.Scope())
    //    {
    //        var thid = Thread.CurrentThread.ManagedThreadId.ToString();
    //        var data = new ExampleTable() { Name = name + "->5" };
    //        service.Save(ref data);
    //        var unitOfWorkManager = Application.Ioc.Resolve<IUnitOfWorkManager>();
    //        var service2 = Application.Ioc.Resolve<IDomainService<ExampleTable, long>>();
    //        using (var unitOfWork = unitOfWorkManager.Begin(new UnitOfWorkOptions() { IsStandalone = true }))
    //        {
    //            var data3 = new ExampleTable() { Name = name + "非事务范围嵌套了事务->6" + Thread.CurrentThread.ManagedThreadId.ToString() };
    //            service2.Save(ref data3);
    //            unitOfWork.Complete();
    //        }

    //        Func<Task> act = async () =>
    //        {
    //            await RunAsync();
    //        };
    //        act().Wait();
    //        Thread t = new Thread(new ThreadStart(() =>
    //        {
    //            var uow2 = Application.Ioc.Resolve<IUnitOfWork>();
    //            var service3 = Application.Ioc.Resolve<IDomainService<ExampleTable, long>>();
    //            var thid2 = Thread.CurrentThread.ManagedThreadId.ToString();
    //            try
    //            {
    //                using (uow2.Scope(true))
    //                {
    //                    var data2 = new ExampleTable() { Name = name + "独立线程->9" + Thread.CurrentThread.ManagedThreadId.ToString() + ":" + object.ReferenceEquals(uow.Context, uow.Context).ToString() };

    //                    service3.Save(ref data2);
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                throw;
    //            }
    //        }));
    //        t.IsBackground = true;
    //        t.Start();

    //    }
    //    var task2 = Task.Factory.StartNew(() =>
    //    {
    //        var unitOfWorkManager = Application.Ioc.Resolve<IUnitOfWorkManager>();
    //        var service2 = Application.Ioc.Resolve<IDomainService<ExampleTable, long>>();
    //        using (var unitOfWork = unitOfWorkManager.Begin())
    //        {
    //            var data = new ExampleTable() { Name = name + "新线程开启新的事务->10" + Thread.CurrentThread.ManagedThreadId.ToString() };

    //            service2.Save(ref data);
    //            unitOfWork.Complete();
    //        }
    //    });

    //    // read inserted data
    //    using (uow.Scope())
    //    {
    //        var readData = service.GetMany().Reverse().Take(2).ToList();
    //        return new TemplateResult("test/hello.html", new { data = new { readData, threadId = Thread.CurrentThread.ManagedThreadId.ToString() } });
    //    }
    //}
    //async Task RunAsync()
    //{
    //    var uow = Application.Ioc.Resolve<IUnitOfWork>();
    //    var service = Application.Ioc.Resolve<IDomainService<ExampleTable, long>>();
    //    string name = RandomUtils.RandomString(5) + "::" + Thread.CurrentThread.ManagedThreadId.ToString();
    //    var aa = uow.Context;

    //    using (uow.Scope())
    //    {
    //        aa = uow.Context;
    //        var data = new ExampleTable() { Name = name + "事务线程内,开启新线程,开启无事务范围->2-7" };
    //        service.Save(ref data);
    //    }
    //    await Task.Delay(50);
    //    using (uow.Scope())
    //    {
    //        var data = new ExampleTable() { Name = name + "事务线程内,开启新线程,开启无事务范围->3-8" + Thread.CurrentThread.ManagedThreadId.ToString() + ":" + object.ReferenceEquals(uow.Context, aa).ToString() };
    //        service.Save(ref data);
    //    }
    //}


}
