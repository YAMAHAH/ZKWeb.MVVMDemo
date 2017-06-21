using System.Collections.Generic;
using System.Linq;
using InfrastructurePlugins.BaseModule.Application.Services.Interfaces;
using InfrastructurePlugins.BaseModule.Module;
using BusinessPlugins.OrganizationModule.Components.ActionFilters;
using ZKWeb.Plugins.OrganizationModule.Components.PrivilegeProviders.Interfaces;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Components.PrivilegeProviders
{
    /// <summary>
    /// 默认的权限提供器
    /// </summary>
    [ExportMany]
    public class DefaultPrivilegeProvider : IPrivilegesProvider
    {
        public IContainer Injector { get { return ZKWeb.Application.Ioc; } }
        /// <summary>
        /// 查找应用服务中的权限并返回
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetPrivileges()
        {
            var applicationServices = Injector.ResolveMany<IApplicationService>();
            return applicationServices
                .SelectMany(a => a.GetApiMethods())
                .SelectMany(m => m.Attributes.OfType<CheckPrivilegeAttribute>())
                .SelectMany(a => a.RequirePrivileges)
                .Distinct();
        }
        /// <summary>
        /// 获取所有模块的模板对象信息
        /// </summary>
        /// <returns></returns>
        public List<ComponentClassInfo> GetModuleComponentClassInfos()
        {
            var modules = Injector.ResolveMany<IAngularModule>();
            return modules.SelectMany(m => m.GetComponentClassInfoes()).ToList();
        }
    }
}
