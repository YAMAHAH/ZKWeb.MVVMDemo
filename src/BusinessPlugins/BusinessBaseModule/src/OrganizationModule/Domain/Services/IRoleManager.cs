using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public interface IRoleManager : IDomainService<Roles, Guid>
    {
        /// <summary>
        /// 获取某一角色被分配的员工
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        ICollection<Employee> GetEmployees(Guid roleId);
        /// <summary>
        /// 获取某一角色被分配的岗位组
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        ICollection<PostGroup> GetPostGroups(Guid roleId);
    }
}
