using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 功能权限管理
    /// </summary>
    [ExportMany]
    public class ActionPrivilegeManager : DomainServiceBase<ActionPrivilege, Guid>, IActionPrivilegeManager
    {
    }
}
