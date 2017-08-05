using BusinessPlugins.BasicModule.Cache;
using BusinessPlugins.BasicModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain.Extensions;
using InfrastructurePlugins.BaseModule.Application.Extensions;
using InfrastructurePlugins.BaseModule.Application.Services.Interfaces;
using InfrastructurePlugins.SessionStateModule.Domain.Services;
using System;
using System.Linq;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace BusinessPlugins.OrganizationModule.Application.Components.Filters
{
    /// <summary>
    /// 工作单元服务过滤提供者
    /// </summary>
    [ExportMany, SingletonReuse]
    public class UserTemplateFilterProvider : IUserTemplateFilterProvider
    {
        public string[] AvailableFilter()
        {
            return GetFilter(t => t.ObjectType == TemplateObjectType.Filter && t.Enable);
        }

        public string[] DisabledFilter()
        {
            return GetFilter(t => t.ObjectType == TemplateObjectType.Filter && !t.Enable);
        }

        private string[] GetFilter(Func<TemplateObject, bool> filter)
        {
            var injector = ZKWeb.Application.Ioc;

            var sessionManager = injector.Resolve<SessionManager>();
            var session = sessionManager.GetSession();

            var user = session.GetUser();
            var empId = (Guid)user.EmployeeId;
            //
            var tempId = HttpManager.CurrentContext.GetApiMethodInfo().ServiceId;
            //获取全局服务ID
            var globalTempId = (Guid)injector.Resolve<IGlobalManageService>()?.ServiceId;
            //缓存管理
            var cacheMan = injector.Resolve<ICacheManager>();
            //获取模板
            var userTempObjDict = cacheMan.GetUserTemplateDictionary(empId, new Guid[] { tempId, globalTempId });

            //获取用户模板所有的过滤器
            var userFilters = userTempObjDict.Values
                .SelectMany(t => t.Values)
                .Where(filter)
                .Select(f => f.ObjectAlias)
                .ToArray();
            return userFilters;
        }
    }
}
