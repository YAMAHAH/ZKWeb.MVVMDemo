using BusinessPlugins.BasicModule.Cache;
using BusinessPlugins.OrganizationModule.Components.PrivilegeTranslators.Interfaces;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain.Entities.Interfaces;
using BusinessPlugins.OrganizationModule.Domain.Extensions;
using BusinessPlugins.OrganizationModule.Domain.Structs;
using InfrastructurePlugins.BaseModule.Application.Extensions;
using InfrastructurePlugins.BaseModule.Components.Exceptions;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using InfrastructurePlugins.SessionStateModule.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZKWeb.Localize;
using ZKWeb.Plugins.OrganizationModule.Components.PrivilegeProviders.Interfaces;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 权限管理器
    /// </summary>
    [ExportMany, SingletonReuse]
    public class PrivilegeManager : DomainServiceBase
    {
        /// <summary>
        /// 获取网站中注册的所有权限，并且去除重复项
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GetPrivileges()
        {
            var providers = ZKWeb.Application.Ioc.ResolveMany<IPrivilegesProvider>();
            var privileges = providers.SelectMany(p => p.GetPrivileges()).Distinct().ToList();
            return privileges;
        }

        /// <summary>
        /// 判断用户是否拥有指定的用户类型
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="userType">用户类型的接口或基础类</param>
        /// <returns></returns>
        public virtual bool HasUserType(User user, Type userType)
        {
            return userType.GetTypeInfo().IsAssignableFrom(user.GetUserType().GetType());
        }

        /// <summary>
        /// 判断用户是否拥有指定的权限
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="privileges">权限列表</param>
        /// <returns></returns>
        public virtual bool HasPrivileges(User user, params string[] privileges)
        {
            var userType = user?.GetUserType();
            if (userType is IAmSuperAdmin)
            {
                // 超级管理员拥有所有权限
                return true;
            }
            if (privileges != null && privileges.Length > 0)
            {
                var containsPrivileges = new HashSet<string>(
                    user.Roles.SelectMany(r => r.To.GetPrivileges()));
                foreach (var privilege in privileges)
                {
                    if (!containsPrivileges.Contains(privilege))
                    {
                        // 未包含指定的所有权限
                        return false;
                    }
                }
            }
            // 检查通过
            return true;
        }

        public bool HasConfigPrivilege(User user)
        {
            var empId = (Guid)user.EmployeeId;
            if (empId == Guid.Empty)
            {
                return false;
            }
            //获取服务ActionInfo
            var action = HttpManager.CurrentContext.GetApiMethodInfo();
            //从缓存中获取用户权限
            var cacheMan = Injector.Resolve<ICacheManager>();
            var tempObject = cacheMan.GetUserTemplateObject(empId, action.ServiceId, action.ActionId);
            return tempObject != null && tempObject.Enable;
        }
        /// <summary>
        /// 判定用户是否有某个模板某个对象的权限
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="serviceId"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public bool HasPrivilege(Guid empId, Guid serviceId, Guid actionId)
        {
            //从缓存中获取用户权限
            var cacheMan = Injector.Resolve<ICacheManager>();
            var tempObject = cacheMan.GetUserTemplateObject(empId, serviceId, actionId);
            return tempObject != null && tempObject.Enable;
        }
        /// <summary>
        ///判定用户是否有某个模板的权限
        /// </summary>
        /// <param name="empId">用户</param>
        /// <param name="serviceId">模板</param>
        /// <returns></returns>
        public bool HasPrivilege(Guid empId, Guid serviceId)
        {
            //从缓存中获取用户模板权限
            var cacheMan = Injector.Resolve<ICacheManager>();
            var tempObject = cacheMan.GetUserTemplateDictionary(empId, serviceId);
            return tempObject != null;
        }

        /// <summary>
        /// 检查用户是否满足指定的权限要求
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="requirement">权限要求</param>
        /// <param name="errorMessage">验证不通过时的信息</param>
        public virtual bool IsAuthorized(User user, AuthRequirement requirement, out string errorMessage)
        {
            // 要求主租户，但用户不属于主租户
            if (requirement.RequireMasterTenant &&
                (user == null || !user.OwnerTenant.IsMaster))
            {
                errorMessage = new T("Action require user under master tenant");
                return false;
            }
            // 用户类型不匹配
            if (requirement.RequireUserType != null &&
                (user == null || !HasUserType(user, requirement.RequireUserType)))
            {
                errorMessage = new T(
                    "Action require user to be '{0}'",
                    new T(requirement.RequireUserType.Name));
                return false;
            }
            // 未拥有所有要求的权限
            if (requirement.RequirePrivileges != null &&
                (user == null || !HasPrivileges(user, requirement.RequirePrivileges)))
            {
                var translator = ZKWeb.Application.Ioc.Resolve<IPrivilegeTranslator>();
                errorMessage = new T("Action require user to be '{0}', and have privileges '{1}'",
                    new T(requirement.RequireUserType?.Name),
                    string.Join(",", requirement.RequirePrivileges.Select(p => translator.Translate(p))));
                return false;
            }
            //验证用户配置权限
            if (requirement.RequireValidConfig && (user == null && !HasConfigPrivilege(user)))
            {
                var translator = ZKWeb.Application.Ioc.Resolve<IPrivilegeTranslator>();
                errorMessage = new T("Action require user to be '{0}', and have privileges '{1}'",
                    new T(requirement.RequireUserType?.Name),
                     translator.Translate(HttpManager.CurrentContext.GetApiMethodInfo().Name));
                return false;
            }
            errorMessage = null;
            return true;
        }

        /// <summary>
        /// 检查当前用户是否满足指定的权限要求
        /// 不满足时抛出403例外
        /// </summary>
        /// <param name="userType">用户类型，例如typeof(IAmAdmin)</param>
        /// <param name="privileges">要求的权限列表</param>
        public virtual void Check(AuthRequirement requirement)
        {
            var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
            var user = sessionManager.GetSession().GetUser();
            string errorMessage;
            if (!IsAuthorized(user, requirement, out errorMessage))
            {
                throw new ForbiddenException(errorMessage);
            }
        }
    }
}
