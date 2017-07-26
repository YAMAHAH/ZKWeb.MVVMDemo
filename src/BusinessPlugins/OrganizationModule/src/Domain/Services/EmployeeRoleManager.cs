using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 员工角色管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class EmployeeRoleManager : DomainServiceBase<EmployeeRole, Guid>, IEmployeeRoleManager
    {
        public ICollection<Roles> GetRoles(Guid empId)
        {
            //获取员工分配的所有角色ID
            var empRoleIds = UnitRepository
                .FastQueryAsReadOnly()
                .Where(er => er.EmployeeId == empId)
                .Select(er => er.RoleId)
                .ToList();
            //获取员工分配的所有角色
            return UnitOfWork.GetUnitRepository<Roles, Guid>()
                .FastQueryAsReadOnly()
                .Where(r => empRoleIds.Contains(r.RootId))
                .ToList();
        }

        public ICollection<Employee> GetEmployees(Guid roleId)
        {
            //获取角色被分配的所有员工ID
            var roleEmpIds = UnitRepository.FastQueryAsReadOnly()
                 .Where(e => e.RoleId == roleId)
                 .Select(e => e.EmployeeId)
                 .ToList();
            //获取角色被分配的所有员工
            return UnitOfWork.GetUnitRepository<Employee, Guid>()
                .FastQueryAsReadOnly()
                .Where(e => roleEmpIds.Contains(e.Id))
                .ToList();
        }
    }
}
