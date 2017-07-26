using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 员工岗位组管理
    /// </summary>
    public interface IEmployeePostGroupManager : IDomainService<EmployeePostGroup, Guid>
    {
        /// <summary>
        /// 获取员工分配的所有岗位组
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        ICollection<PostGroup> GetPostGroups(Guid empId);
        /// <summary>
        /// 获取岗位组被分配的所有员工
        /// </summary>
        /// <param name="postGroupId"></param>
        /// <returns></returns>
        ICollection<Employee> GetEmployees(Guid postGroupId);
    }
}
