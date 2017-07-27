using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 岗位管理
    /// </summary>
    public interface IPostGroupManager : IDomainService<PostGroup, Guid>
    {
        /// <summary>
        /// 获取岗位组分配的角色
        /// </summary>
        /// <param name="postGroupId"></param>
        /// <returns></returns>
        ICollection<Roles> GetRoles(Guid postGroupId);
        /// <summary>
        /// 获取岗位组分配的角色,包含子角色
        /// </summary>
        /// <param name="postGroupId"></param>
        /// <returns></returns>
        ICollection<Roles> GetAllRoles(Guid postGroupId);
        /// <summary>
        /// 获取岗位组被分配的所有人员
        /// </summary>
        /// <param name="postGroupId"></param>
        /// <returns></returns>
        ICollection<Employee> GetEmployees(Guid postGroupId);
        /// <summary>
        /// 获取岗位组及子岗位组被分配的所有人员
        /// </summary>
        /// <param name="postGroupId"></param>
        /// <returns></returns>
        ICollection<Employee> GetAllEmployees(Guid postGroupId);
    }
}
