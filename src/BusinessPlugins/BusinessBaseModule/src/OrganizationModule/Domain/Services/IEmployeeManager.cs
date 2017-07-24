using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 员工管理
    /// </summary>
    public interface IEmployeeManager : IDomainService<Employee, Guid>
    {
    }
}
