using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 员工角色管理
    /// </summary>
    public interface IEmployeeRoleManager : IDomainService<EmployeeRole, Guid>
    {
        /// <summary>
        /// 获取员工分配的所有角色
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        ICollection<Roles> GetRoles(Guid empId);
        /// <summary>
        /// 获取角色被分配的所有员工
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        ICollection<Employee> GetEmployees(Guid roleId);
    }
}
