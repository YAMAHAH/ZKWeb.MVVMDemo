using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 功能权限管理
    /// </summary>
    public interface IActionPrivilegeManager : IDomainService<ActionPrivilege, Guid>
    {
    }
}
