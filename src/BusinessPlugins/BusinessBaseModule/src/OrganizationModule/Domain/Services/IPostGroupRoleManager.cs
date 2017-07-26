using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 岗位组角色管理
    /// </summary>
    public interface IPostGroupRoleManager : IDomainService<PostGroupRole, Guid>
    {
        /// <summary>
        /// 获取岗位组分配的所有角色
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        ICollection<Roles> GetRoles(Guid postGroupId);
        /// <summary>
        /// 获取角色被分配的所有岗位组
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        ICollection<PostGroup> GetPostGroups(Guid roleId);
    }
}
