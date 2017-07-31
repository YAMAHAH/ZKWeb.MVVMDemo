using BusinessPlugins.OrganizationModule.Domain.Extensions;
using InfrastructurePlugins.BaseModule.Application.Services.Interfaces;
using InfrastructurePlugins.BaseModule.Domain.Uow.Interfaces;
using InfrastructurePlugins.SessionStateModule.Domain.Services;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Application.Components.Filters
{
    /// <summary>
    /// 工作单元服务过滤提供者
    /// </summary>
    [ExportMany]
    public class UnitofworkServiceFilter : IUnitofworkServiceFilter
    {
        public string[] Filters(Guid tempId)
        {
            var injector = ZKWeb.Application.Ioc;
            var sessionManager = injector.Resolve<SessionManager>();
            var session = sessionManager.GetSession();
            var user = session.GetUser();
            var empId = user.EmployeeId;
            //获取用户+模板所有的过滤器
            //全局过滤器 + 模板过滤器 +　自定义过滤器
            return new string[] { };
        }
    }
}
