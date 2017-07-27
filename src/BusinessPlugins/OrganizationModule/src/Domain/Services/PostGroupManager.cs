using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 岗位管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class PostGroupManager : DomainServiceBase<PostGroup, Guid>, IPostGroupManager
    {
        private ICollection<Guid> GetRoleIds(Guid postGroupId)
        {
            return UnitOfWork.GetUnitRepository<PostGroupRole, Guid>()
              .FastQueryAsReadOnly()
              .Where(p => p.PostGroupId == postGroupId)
              .Select(p => p.RoleId)
              .ToList();
        }
        public ICollection<Roles> GetAllRoles(Guid postGroupId)
        {
            //获取岗位组分配的所有角色ID
            var postRoleIds = GetRoleIds(postGroupId);
            //角色仓储
            var roleRepository = UnitOfWork.GetUnitRepository<Roles, Guid>();
            var Ids = roleRepository
                .FastQueryAsReadOnly()
                .Where(p => postRoleIds.Contains(p.Id))
                .Select(p => new { p.Id, p.RootId });

            //根据rootid和id查询出角色及子角色的对象
            var nodeIds = Ids.Select(i => i.Id).Distinct().Cast<object>().ToList();
            var rootIds = Ids.Select(i => i.RootId).Distinct().Cast<object>().ToList();

            //获取所有角色,包含子角色
            return roleRepository.FastGetManyTreeNodes(nodeIds, rootIds);
        }

        public ICollection<Roles> GetRoles(Guid postGroupId)
        {
            //获取岗位组分配的所有角色ID
            var postRoleIds = GetRoleIds(postGroupId);
            //获取所有角色,包含子角色
            return UnitOfWork.GetUnitRepository<Roles, Guid>()
                .FastQueryAsReadOnly()
                .Where(r => postRoleIds.Contains(r.Id))
                .ToList();
        }

        public ICollection<Employee> GetEmployees(Guid postGroupId)
        {
            //获取岗位组被分配的员工的所有ID
            var postGrpEmpIds = UnitOfWork.GetUnitRepository<EmployeePostGroup, Guid>()
                .FastQueryAsReadOnly()
                .Where(e => e.PostGroupId == postGroupId)
                .Select(e => e.EmployeeId)
                .ToList();
            //获取岗位组被分配的所有员工
            return UnitOfWork.GetUnitRepository<Employee, Guid>()
                .FastQueryAsReadOnly()
                .Where(e => postGrpEmpIds.Contains(e.Id))
                .ToList();
        }

        public ICollection<Employee> GetAllEmployees(Guid postGroupId)
        {
            var postGrpRep = UnitOfWork.GetUnitRepository<PostGroup, Guid>();
            var rootId = postGrpRep
                .FastQueryAsReadOnly()
                .Where(p => p.Id == postGroupId)
                .Select(p => p.RootId);

            var postGroupIds = postGrpRep
                .FastGetTreeNodes(postGroupId, rootId)
                .Select(p => p.Id)
                .ToList();

            //获取岗位组被分配的员工的所有ID
            var postGrpEmpIds = UnitOfWork.GetUnitRepository<EmployeePostGroup, Guid>()
                .FastQueryAsReadOnly()
                .Where(e => postGroupIds.Contains(e.PostGroupId))
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
