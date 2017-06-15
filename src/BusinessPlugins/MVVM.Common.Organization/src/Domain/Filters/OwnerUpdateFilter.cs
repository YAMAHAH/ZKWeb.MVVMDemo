using BusinessPlugins.MVVM.Common.Organization.Domain.Entities;
using BusinessPlugins.MVVM.Common.Organization.Domain.Entities.TypeTraits;
using BusinessPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using System;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.MVVM.Common.Organization.src.Domain.Filters
{
    /// <summary>
    /// 
    /// </summary>
    [ExportMany]
    public class OwnerUpdateFilter : IEntityOperationFilter
    {
        /// <summary>
        /// 数据应当属于的用户Id
        /// </summary>
        public Guid ExceptedOwnerId => _exceptedOwnerId.Value;
        protected Lazy<Guid> _exceptedOwnerId;
        protected SessionManager sessionManager;

        public OwnerUpdateFilter()
        {
            sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
            _exceptedOwnerId = new Lazy<Guid>(() => sessionManager.GetSession().UserId ?? Guid.Empty);
        }
        void IEntityOperationFilter.FilterDelete<TEntity, TPrimaryKey>(TEntity entity) { }

        void IEntityOperationFilter.FilterSave<TEntity, TPrimaryKey>(TEntity entity)
        {
            if (!HaveCreatorUpdatorTrait<TEntity>.HaveCreatorUpdator)
            {
                return;
            }
            var e = ((IHaveCreatorUpdator)entity);
            var user = sessionManager.GetSession().GetUser();
            if (e.Creator == null && ExceptedOwnerId == Guid.Empty)
            {
                // 未登陆用户保存数据，不需要设置
            }
            else if (e.Creator == null && ExceptedOwnerId != Guid.Empty && user.Employee != null)
            {
                e.CreatorId = e.UpdatorId = user.Employee.Id;
                e.Creator = e.Updator = user.Employee;
            }
            else if (e.Creator != null && e.Creator.Id != Guid.Empty && ExceptedOwnerId != Guid.Empty && user.Employee != null)
            {
                e.UpdatorId = user.Employee.Id;
                e.Updator = user.Employee;
            }
        }
    }
}
