using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 员工管理
    /// </summary>
    public interface IEmployeeManager : IDomainService<Employee, Guid>
    {
        /// <summary>
        /// 获取员工所拥有的所有岗位组
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        ICollection<PostGroup> GetAllPostGroups(Guid empId);
        /// <summary>
        /// 获取员工所拥有的所有角色
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        ICollection<Roles> GetAllRoles(Guid empId);
    }
}
