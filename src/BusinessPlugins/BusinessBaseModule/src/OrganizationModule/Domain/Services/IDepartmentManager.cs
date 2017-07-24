using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 部门管理
    /// </summary>
    public interface IDepartmentManager : IDomainService<Department, Guid>
    {
    }
}
