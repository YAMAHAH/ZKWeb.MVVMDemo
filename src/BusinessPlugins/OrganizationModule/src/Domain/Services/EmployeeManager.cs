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
    [ExportMany, SingletonReuse]
    public class EmployeeManager : DomainServiceBase<Employee, Guid>, IEmployeeManager
    {
        private IEmployeePostGroupManager xEmpPostGrpManager;
        public EmployeeManager(IEmployeePostGroupManager empPostGrpManager)
        {
            xEmpPostGrpManager = empPostGrpManager;
        }

        public ICollection<Roles> GetAllRoles(Guid empId)
        {
            //获取员工分配的所有角色ID
            var empRoleIds = UnitOfWork.GetUnitRepository<EmployeeRole, Guid>()
                .FastQueryAsReadOnly()
                .Where(er => er.EmployeeId == empId)
                .Select(er => er.RoleId)
                .ToList();

            //获取岗位组分配的所有角色 GetAllPostGroups(empId)
            var empPostIds = xEmpPostGrpManager.GetPostGroups(empId).Select(p => p.Id).ToList();

            var postRoleIds = UnitOfWork.GetUnitRepository<PostGroupRole, Guid>()
                .FastQueryAsReadOnly()
                .Where(p => empPostIds.Contains(p.PostGroupId))
                .Select(pr => pr.RoleId)
                .ToList();
            //员工角色和岗位角色合并
            var allRoleIds = empRoleIds.Union(postRoleIds).ToList();
            //获取所有角色
            return UnitOfWork.GetUnitRepository<Roles, Guid>()
                .FastQueryAsReadOnly()
                .Where(r => allRoleIds.Contains(r.RootId))
                .ToList();
        }
    }
}
