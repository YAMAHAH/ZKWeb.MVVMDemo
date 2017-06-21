using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain.Entities.TypeTraits;
using BusinessPlugins.OrganizationModule.Domain.Entities.Interfaces;
using System;
using ZKWeb.Localize;
using InfrastructurePlugins.BaseModule.Components.Exceptions;
using InfrastructurePlugins.BaseModule.Domain.Filters.Interfaces;
using InfrastructurePlugins.BaseModule.Domain.Uow.Interfaces;
using BusinessPlugins.OrganizationModule.Domain.Extensions;
using InfrastructurePlugins.SessionStateModule.Domain.Services;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Filters
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
