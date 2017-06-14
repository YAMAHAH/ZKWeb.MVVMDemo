using BusinessPlugins.MVVM.Common.Organization.Domain.Entities;
using BusinessPlugins.MVVM.Common.Organization.Domain.Entities.TypeTraits;
using BusinessPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using System;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.MVVM.Common.Organization.src.Domain.Filters
{
    /// <summary>
    /// 
    /// </summary>
    [ExportMany]
    public class CreatorUpdatorFilter : IEntityOperationFilter
    {
        /// <summary>
        /// 数据应当属于的用户Id
        /// </summary>
        public Guid ExceptedOwnerId => _exceptedOwnerId.Value;
        protected Lazy<Guid> _exceptedOwnerId;

        public CreatorUpdatorFilter()
        {
            var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
            _exceptedOwnerId = new Lazy<Guid>(() => sessionManager.GetSession().UserId ?? Guid.Empty);
        }
        void IEntityOperationFilter.FilterDelete<TEntity, TPrimaryKey>(TEntity entity)
        {

        }

        void IEntityOperationFilter.FilterSave<TEntity, TPrimaryKey>(TEntity entity)
        {
            if (!HaveCreatorUpdatorTrait<TEntity>.HaveCreatorUpdator)
            {
                return;
            }
            var e = ((IHaveCreatorUpdator)entity);
            var repository = ZKWeb.Application.Ioc.Resolve<IUnitOfWork>().GetRepository<Employee, Guid>();
            if (e.Creator == null && ExceptedOwnerId == Guid.Empty)
            {
                // 未登陆用户保存数据，不需要设置
            }
            else if (e.Creator == null && ExceptedOwnerId != Guid.Empty)
            {
                // 已登陆用户保存数据，设置所属用户，注意这里会受查询过滤器的影响
                var user = repository.Get(u => u.Id == ExceptedOwnerId);
                if (user == null)
                {
                    throw new BadRequestException("Set entity creator and updator failed, user not found");
                }
                e.CreatorId = ExceptedOwnerId;
                e.Creator = user;
                e.UpdatorId = ExceptedOwnerId;
                e.Updator = user;
            }
            else if (e.Creator != null && e.Creator.Id != Guid.Empty)
            {
                var user = repository.Get(u => u.Id == ExceptedOwnerId);
                if (user == null)
                {
                    throw new BadRequestException("Set entity creator and updator failed, user not found");
                }
                e.UpdatorId = ExceptedOwnerId;
                e.Updator = user;
            }
        }
    }
}
