using System;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using ZKWebStandard.Ioc;
using System.Collections.Generic;
using System.Linq;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 角色管理器
    /// </summary>
    [ExportMany, SingletonReuse]
    public class RolesManager : DomainServiceBase<Roles, Guid>, IRoleManager
    {
        /// <summary>
        /// 获取角色被分配的所有员工ID
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private ICollection<Guid> GetEmployeeIds(Guid roleId)
        {
            return UnitOfWork.GetUnitRepository<EmployeeRole, Guid>()
                 .FastQueryAsReadOnly()
                 .Where(e => e.RoleId == roleId)
                 .Select(e => e.EmployeeId)
                 .ToList();
        }
        public ICollection<Employee> GetEmployees(Guid roleId)
        {
            //获取角色被分配的所有员工ID
            var roleEmpIds = GetEmployeeIds(roleId);
            //获取角色被分配的所有员工
            return UnitOfWork.GetUnitRepository<Employee, Guid>()
                .FastQueryAsReadOnly()
                .Where(e => roleEmpIds.Contains(e.Id))
                .ToList();
        }

        public ICollection<PostGroup> GetPostGroups(Guid roleId)
        {
            //获取角色分配岗位组的所有ID
            var rolePostGroupIds = UnitOfWork.GetUnitRepository<PostGroupRole, Guid>()
                 .FastQueryAsReadOnly()
                 .Where(e => e.RoleId == roleId)
                 .Select(e => e.PostGroupId)
                 .ToList();

            //获取岗位组及子岗位组的所有对象
            return UnitOfWork.GetUnitRepository<PostGroup, Guid>()
                .FastQueryAsReadOnly()
                .Where(p => rolePostGroupIds.Contains(p.Id))
                .ToList();
        }
    }
}
