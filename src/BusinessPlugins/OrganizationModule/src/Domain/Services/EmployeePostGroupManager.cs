using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 员工岗位组管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class EmployeePostGroupManager : DomainServiceBase<EmployeePostGroup, Guid>, IEmployeePostGroupManager
    {
        public ICollection<PostGroup> GetPostGroups(Guid empId)
        {
            //获取员工分配岗位组的所有ID
            var empPostGroupIds = UnitRepository
                 .FastQueryAsReadOnly()
                 .Where(e => e.EmployeeId == empId)
                 .Select(e => e.PostGroupId)
                 .ToList();

            //获取岗位组及子岗位组的所有对象
            return UnitOfWork.GetUnitRepository<PostGroup, Guid>()
                .FastQueryAsReadOnly()
                .Where(p => empPostGroupIds.Contains(p.RootId))
                .ToList();
        }

        public ICollection<Employee> GetEmployees(Guid postGroupId)
        {
            //获取岗位组被分配的员工的所有ID
            var postGrpEmpIds = UnitRepository.FastQueryAsReadOnly()
                .Where(e => e.PostGroupId == postGroupId)
                .Select(e => e.EmployeeId)
                .ToList();
            //获取岗位组被分配的所有员工
            return UnitOfWork.GetUnitRepository<Employee, Guid>()
                .FastQueryAsReadOnly()
                .Where(e => postGrpEmpIds.Contains(e.Id))
                .ToList();
        }
    }
}
