using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 岗位组角色管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class PostGroupRoleManager : DomainServiceBase<PostGroupRole, Guid>, IPostGroupRoleManager
    {
        public ICollection<PostGroup> GetPostGroups(Guid roleId)
        {
            //获取角色分配岗位组的所有ID
            var rolePostGroupIds = UnitRepository
                 .FastQueryAsReadOnly()
                 .Where(e => e.RoleId == roleId)
                 .Select(e => e.PostGroupId)
                 .ToList();

            //获取岗位组及子岗位组的所有对象
            return UnitOfWork.GetUnitRepository<PostGroup, Guid>()
                .FastQueryAsReadOnly()
                .Where(p => rolePostGroupIds.Contains(p.RootId))
                .ToList();
        }

        public ICollection<Roles> GetRoles(Guid postGroupId)
        {
            //获取岗位组分配的所有角色ID
            var postRoleIds = UnitRepository
                .FastQueryAsReadOnly()
                .Where(p => p.PostGroupId == postGroupId)
                .Select(p => p.RoleId)
                .ToList();
            //获取所有角色
            return UnitOfWork.GetUnitRepository<Roles, Guid>()
                .FastQueryAsReadOnly()
                .Where(r => postRoleIds.Contains(r.RootId))
                .ToList();
        }
    }
}
