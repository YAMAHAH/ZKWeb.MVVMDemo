using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 员工管理
    /// </summary>
    [ExportMany]
    public class EmployeeManager : DomainServiceBase<Employee, Guid>, IEmployeeManager
    {
        public ICollection<PostGroup> GetAllPostGroups(Guid empId)
        {
            //获取员工分配岗位组的所有ID
            var empPostGroupIds = UnitOfWork.GetUnitRepository<EmployeePostGroup, Guid>()
                 .FastQueryAsReadOnly()
                 .Where(ep => ep.EmployeeId == empId)
                 .Select(ep => ep.PostGroupId)
                 .ToList();
            //获取岗位组及子岗位组的所有对象
            return UnitOfWork.GetUnitRepository<PostGroup, Guid>()
                .FastQueryAsReadOnly()
                .Where(pg => empPostGroupIds.Contains(pg.RootId))
                .ToList();
        }

        public ICollection<Roles> GetAllRoles(Guid empId)
        {
            //获取员工分配的所有角色ID
            var empRoleIds = UnitOfWork.GetUnitRepository<EmployeeRole, Guid>()
                .FastQueryAsReadOnly()
                .Where(er => er.EmployeeId == empId)
                .Select(er => er.RoleId)
                .ToList();

            //获取岗位组分配的所有角色
            var empPostIds = GetAllPostGroups(empId).Select(p => p.Id).ToList();

            var postRoleIds = UnitOfWork.GetUnitRepository<PostGroupRole, Guid>()
                .FastQueryAsReadOnly()
                .Where(p => empPostIds.Contains(p.PostGroupId))
                .Select(pr => pr.RoleId)
                .ToList();
            //员工角色和岗位角色合并
            var allRoleIds = empRoleIds.Union(empPostIds).ToList();
            //获取所有角色
            return UnitOfWork.GetUnitRepository<Roles, Guid>()
                .FastQueryAsReadOnly()
                .Where(r => allRoleIds.Contains(r.RootId))
                .ToList();
        }
    }
}
