using InfrastructurePlugins.BaseModule.Application.Services.Interfaces;
using InfrastructurePlugins.BaseModule.Domain.Uow.Interfaces;
using System;
using System.Linq;
using System.Data;
using ZKWeb.Web;

namespace InfrastructurePlugins.BaseModule.Application.Services.Attributes
{
    /// <summary>
    /// 控制应用服务如何使用工作单元，标记在应用服务的函数上
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class UnitOfWorkAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 是否禁用工作单元，默认是false
        /// </summary>
        public bool IsDisabled { get; set; }
        /// <summary>
        /// 是否开启事务。默认是false
        /// </summary>
        public bool IsTransactional { get; set; }
        /// <summary>
        /// 事务的隔离等级，默认是null
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }
        /// <summary>
        /// 应用服务ID
        /// </summary>
        public Guid ServiceId { get; set; }
        /// <summary>
        /// 开启工作单元
        /// </summary>
        public override Func<IActionResult> Filter(Func<IActionResult> action)
        {
            if (IsDisabled)
            {
                // 不使用工作单元
                return action;
            }
            return new Func<IActionResult>(() =>
            {
                var injector = ZKWeb.Application.Ioc;
                var filterProviders = injector.ResolveMany<IUnitofworkServiceFilter>();
                var filters = filterProviders.SelectMany(f => f.Filters(ServiceId)).ToArray();
                //  var uow = injector.Resolve<IUnitOfWork>();
                var uowMan = injector.Resolve<IUnitOfWorkManager>();
                //获取用户+模板的过滤器

                //启用过滤器
                if (IsTransactional)
                {
                    using (var unitMan = uowMan.Begin())
                    {
                        using (uowMan.Current.DisableFilter(filters))
                        {
                            var result = action();
                            unitMan.Complete();
                            return result;
                        }
                    }
                }
                else
                {
                    using (uowMan.CreateUnitOfWork())
                    {
                        using (uowMan.Current.DisableFilter(filters))
                        {
                            var result = action();
                            return result;
                        }
                    }
                }
                //using (uow.Scope())
                //// 使用工作单元
                //using (uow.DisableFilter())
                //{
                //    // 并且使用事务
                //    if (IsTransactional)
                //    {
                //        uow.Context.BeginTransaction(IsolationLevel);
                //    }
                //    var result = action();
                //    if (IsTransactional)
                //    {
                //        uow.Context.FinishTransaction();
                //    }
                //    return result;
                //}
            });
        }
    }
}
