using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public interface IRoleManager : IDomainService<Roles, Guid>
    {
    }
}
