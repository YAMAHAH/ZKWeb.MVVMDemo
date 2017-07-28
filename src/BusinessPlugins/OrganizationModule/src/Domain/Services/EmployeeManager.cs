using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using ZKWebStandard.Ioc;
using BusinessPlugins.BasicModule.Domain.Entities;
using BusinessPlugins.BasicModule.Domain.Services;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 员工管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class EmployeeManager : DomainServiceBase<Employee, Guid>, IEmployeeManager
    {
        private ITemplateManager xTempManager;
        public EmployeeManager(ITemplateManager tempManager)
        {
            xTempManager = tempManager;
        }
        public ICollection<PostGroup> GetAllPostGroups(Guid empId)
        {
            //获取员工分配岗位组的所有ID
            var empPostGroupIds = GetEmployeePostGroupIds(empId);
            var postGrpRep = UnitOfWork.GetUnitRepository<PostGroup, Guid>();

            //获取所有组对象,并得出相应的rootid和id
            var Ids = postGrpRep.FastQueryAsReadOnly()
             .Where(p => empPostGroupIds.Contains(p.Id))
             .Select(p => new { p.Id, p.RootId });

            //根据rootid和id查询出组及子组的对象
            var nodeIds = Ids.Select(i => i.Id).Distinct().Cast<object>().ToList();
            var rootIds = Ids.Select(i => i.RootId).Distinct().Cast<object>().ToList();

            return postGrpRep.FastGetManyTreeNodes(nodeIds, rootIds);
        }
        public ICollection<Roles> GetAllRoles(Guid empId)
        {
            //获取员工分配的所有角色ID
            var empRoleIds = GetEmployeeRoleIds(empId);

            //获取岗位组分配的所有角色
            var empPostIds = GetAllPostGroups(empId).Select(p => p.Id).ToList();

            var postRoleIds = UnitOfWork.GetUnitRepository<PostGroupRole, Guid>()
                .FastQueryAsReadOnly()
                .Where(p => empPostIds.Contains(p.PostGroupId))
                .Select(p => p.RoleId)
                .ToList();
            //员工角色和岗位角色合并
            var allRoleIds = empRoleIds.Union(postRoleIds).ToList();
            //获取所有角色
            var roleRepository = UnitOfWork.GetUnitRepository<Roles, Guid>();
            //获取所有组对象,并得出相应的rootid和id

            var Ids = roleRepository.FastQueryAsReadOnly()
             .Where(p => allRoleIds.Contains(p.Id))
             .Select(p => new { p.Id, p.RootId });

            //根据rootid和id查询出角色及子角色的对象
            var nodeIds = Ids.Select(i => i.Id).Distinct().Cast<object>().ToList();
            var rootIds = Ids.Select(i => i.RootId).Distinct().Cast<object>().ToList();
            //获取所有角色
            return roleRepository.FastGetManyTreeNodes(nodeIds, rootIds);
        }
        public ICollection<PostGroup> GetPostGroups(Guid empId)
        {
            //获取员工分配岗位组的所有ID
            var empPostGroupIds = GetEmployeePostGroupIds(empId);
            //获取岗位组对象
            return UnitOfWork.GetUnitRepository<PostGroup, Guid>()
                .FastQueryAsReadOnly()
                .Where(p => empPostGroupIds.Contains(p.Id))
                .ToList();
        }
        /// <summary>
        /// 获取员工分配的所有角色ID
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        private ICollection<Guid> GetEmployeeRoleIds(Guid empId)
        {
            return UnitOfWork.GetUnitRepository<EmployeeRole, Guid>()
                 .FastQueryAsReadOnly()
                 .Where(er => er.EmployeeId == empId)
                 .Select(er => er.RoleId)
                 .ToList();
        }
        /// <summary>
        /// 获取员工分配岗位组的所有ID
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        private ICollection<Guid> GetEmployeePostGroupIds(Guid empId)
        {
            return UnitOfWork.GetUnitRepository<EmployeePostGroup, Guid>()
                 .FastQueryAsReadOnly()
                 .Where(e => e.EmployeeId == empId)
                 .Select(e => e.PostGroupId)
                 .ToList();
        }
        public ICollection<Roles> GetRoles(Guid empId)
        {
            //获取员工分配的所有角色ID
            var empRoleIds = GetEmployeeRoleIds(empId);
            //获取员工分配的所有角色
            return UnitOfWork.GetUnitRepository<Roles, Guid>()
                .FastQueryAsReadOnly()
                .Where(r => empRoleIds.Contains(r.Id))
                .ToList();
        }
        public Dictionary<Guid, TemplateObject> GetTemplateObjectPrivileges(Guid empId, Guid tempId)
        {
            //获取员工分配的所有岗位组ID
            var postGrpIds = GetAllPostGroups(empId).Select(e => e.Id).ToList();
            //获取用户分配的角色ID列表
            var roleIds = GetAllRoles(empId).Select(r => r.Id).ToList();

            //获取模板的所有模板对象字典 
            var allTempObjects = xTempManager.GetTemplateObjects(tempId);

            //获取模板权限对象仓储
            var tempPrivRep = UnitOfWork.GetUnitRepository<TemplatePrivilege, Guid>();
            //获取用户模板分配的权限对象字典
            var userTempPrivDicts = tempPrivRep.FastQueryAsReadOnly()
               .Where(p => (p.EmployeeId == empId || postGrpIds.Contains((Guid)p.PostGroupId) || roleIds.Contains((Guid)p.RoleId)) && p.TemplateId == tempId)
               .GroupBy(p => new { p.TemplateId, p.TemplateObjectId })
               .Select(p => new
               {
                   p.Key.TemplateId,
                   p.Key.TemplateObjectId,
                   Visible = p.Max(t => t.Visible),
                   Enable = p.Max(t => t.Enable),
                   Editable = p.Max(t => t.Editable),
                   Queryable = p.Max(t => t.Queryable)
               }).ToDictionary(t => t.TemplateObjectId);

            //把用户权限字典赋值给模板对象字典
            foreach (var tempPriv in userTempPrivDicts.Keys)
            {
                allTempObjects[tempPriv].Enable = userTempPrivDicts[tempPriv].Enable;
                allTempObjects[tempPriv].Visible = userTempPrivDicts[tempPriv].Visible;
                allTempObjects[tempPriv].Editable = userTempPrivDicts[tempPriv].Editable;
                allTempObjects[tempPriv].Queryable = userTempPrivDicts[tempPriv].Queryable;
            }
            return allTempObjects;
        }
    }
}
