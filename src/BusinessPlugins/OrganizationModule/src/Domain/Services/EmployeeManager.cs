using BusinessPlugins.BasicModule.Domain.Entities;
using BusinessPlugins.BasicModule.Domain.Services;
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
        private ITemplateManager xTempManager;

        public EmployeeManager(ITemplateManager tempManager)
        {
            xTempManager = tempManager;
        }

        public ICollection<PostGroup> GetAllPostGroups(Guid[] empIds)
        {
            //获取员工分配岗位组的所有ID
            var empPostGroupIds = GetEmployeePostGroupIds(empIds);
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

        public ICollection<Roles> GetAllRoles(Guid[] empIds)
        {
            //获取员工分配的所有角色ID
            var empRoleIds = GetEmployeeRoleIds(empIds);

            //获取岗位组分配的所有角色
            var empPostIds = GetAllPostGroups(empIds).Select(p => p.Id).ToList();

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

        public ICollection<PostGroup> GetPostGroups(Guid[] empIds)
        {
            //获取员工分配岗位组的所有ID
            var empPostGroupIds = GetEmployeePostGroupIds(empIds);
            //获取岗位组对象
            return UnitOfWork.GetUnitRepository<PostGroup, Guid>()
                .FastQueryAsReadOnly()
                .Where(p => empPostGroupIds.Contains(p.Id))
                .ToList();
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
            return GetEmployeeRoleIds(e => e.EmployeeId == empId);
        }

        private ICollection<Guid> GetEmployeeRoleIds(Guid[] empIds)
        {
            return GetEmployeeRoleIds(e => empIds.Contains(e.EmployeeId));
        }

        private ICollection<Guid> GetEmployeeRoleIds(Func<EmployeeRole, bool> filter)
        {
            return UnitOfWork.GetUnitRepository<EmployeeRole, Guid>()
                 .FastQueryAsReadOnly()
                 .Where(filter)
                 .Select(e => e.RoleId)
                 .ToList();
        }

        /// <summary>
        /// 获取员工分配岗位组的所有ID
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        private ICollection<Guid> GetEmployeePostGroupIds(Guid empId)
        {
            return GetEmployeePostGroupIds(e => e.EmployeeId == empId);
        }

        private ICollection<Guid> GetEmployeePostGroupIds(Guid[] empIds)
        {
            return GetEmployeePostGroupIds(e => empIds.Contains(e.EmployeeId));
        }

        private ICollection<Guid> GetEmployeePostGroupIds(Func<EmployeePostGroup, bool> filter)
        {
            return UnitOfWork.GetUnitRepository<EmployeePostGroup, Guid>()
                 .FastQueryAsReadOnly()
                 .Where(filter)
                 .Select(e => e.PostGroupId)
                 .ToList();
        }

        public ICollection<Roles> GetRoles(Guid[] empIds)
        {
            //获取员工分配的所有角色ID
            var empRoleIds = GetEmployeeRoleIds(empIds);
            //获取员工分配的所有角色
            return UnitOfWork.GetUnitRepository<Roles, Guid>()
                .FastQueryAsReadOnly()
                .Where(r => empRoleIds.Contains(r.Id))
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
            var tempObjects = xTempManager.GetTemplateObjects(tempId);

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
                   Queryable = p.Max(t => t.Queryable),
                   SelectedStatus = p.Max(t => t.SelectedStatus)
               }).ToDictionary(t => t.TemplateObjectId);

            //把用户权限字典赋值给模板对象字典
            foreach (var privKey in userTempPrivDicts.Keys)
            {
                tempObjects[privKey].Enable = userTempPrivDicts[privKey].Enable;
                tempObjects[privKey].Visible = userTempPrivDicts[privKey].Visible;
                tempObjects[privKey].Editable = userTempPrivDicts[privKey].Editable;
                tempObjects[privKey].Queryable = userTempPrivDicts[privKey].Queryable;
                tempObjects[privKey].SelectedStatus = userTempPrivDicts[privKey].SelectedStatus;
            }
            return tempObjects;
        }

        public TemplateObject GetTemplateObjectPrivilege(Guid empId, Guid tempId, Guid objectId)
        {
            //获取员工分配的所有岗位组ID
            var postGrpIds = GetAllPostGroups(empId).Select(e => e.Id).ToList();
            //获取用户分配的角色ID列表
            var roleIds = GetAllRoles(empId).Select(r => r.Id).ToList();

            //获取模板的所有模板对象字典 
            var tempObject = xTempManager.GetTemplateObject(tempId, objectId);

            //获取模板权限对象仓储
            var tempPrivRep = UnitOfWork.GetUnitRepository<TemplatePrivilege, Guid>();
            //获取用户模板分配的权限对象字典
            var userTempPriv = tempPrivRep.FastQueryAsReadOnly()
               .Where(p => (p.EmployeeId == empId || postGrpIds.Contains((Guid)p.PostGroupId) || roleIds.Contains((Guid)p.RoleId)) && p.TemplateId == tempId)
               .GroupBy(p => new { p.TemplateId, p.TemplateObjectId })
               .Select(p => new
               {
                   p.Key.TemplateId,
                   p.Key.TemplateObjectId,
                   Visible = p.Max(t => t.Visible),
                   Enable = p.Max(t => t.Enable),
                   Editable = p.Max(t => t.Editable),
                   Queryable = p.Max(t => t.Queryable),
                   SelectedStatus = p.Max(t => t.SelectedStatus)
               })
               .Where(t => t.TemplateObjectId == objectId)
               .FirstOrDefault();
            //设置用户权限值
            tempObject.Enable = userTempPriv.Enable;
            tempObject.Visible = userTempPriv.Visible;
            tempObject.Editable = userTempPriv.Editable;
            tempObject.Queryable = userTempPriv.Queryable;
            tempObject.SelectedStatus = userTempPriv.SelectedStatus;

            return tempObject;
        }

        public Dictionary<string, Dictionary<Guid, TemplateObject>> GetTemplateObjectPrivileges(IList<KeyValuePair<Guid, Guid>> userTempIds)
        {
            //获取员工分配的所有岗位组ID
            var empIds = userTempIds.Select(u => u.Key).ToArray();
            var postGrpIds = GetAllPostGroups(empIds).Select(e => e.Id).ToList();
            //获取用户分配的角色ID列表
            var roleIds = GetAllRoles(empIds).Select(r => r.Id).ToList();

            //获取模板的所有模板对象字典 
            var tempObjects = xTempManager.GetTemplateObjects(userTempIds);
            var tempIds = userTempIds.Select(t => t.Value).ToArray();

            //获取模板权限对象仓储
            var tempPrivRep = UnitOfWork.GetUnitRepository<TemplatePrivilege, Guid>();
            //获取用户模板分配的权限对象字典
            var userTempPrivs = tempPrivRep.FastQueryAsReadOnly()
               .Where(p => (empIds.Contains((Guid)p.EmployeeId) || postGrpIds.Contains((Guid)p.PostGroupId) || roleIds.Contains((Guid)p.RoleId)) && tempIds.Contains(p.TemplateId))
               .ToList();

            foreach (var kv in userTempIds)
            {
                var empId = kv.Key;
                //获取员工分配的所有岗位
                var localPostGrpIds = postGrpIds;
                //获取员工分配的所有角色
                var localRoleIds = roleIds;
                var userTempPrivDict = userTempPrivs
               .Where(p => (p.EmployeeId == empId || localPostGrpIds.Contains((Guid)p.PostGroupId) || localRoleIds.Contains((Guid)p.RoleId)) && p.TemplateId == kv.Value)
               .GroupBy(p => new { p.TemplateId, p.TemplateObjectId })
               .Select(p => new
               {
                   p.Key.TemplateId,
                   p.Key.TemplateObjectId,
                   Visible = p.Max(t => t.Visible),
                   Enable = p.Max(t => t.Enable),
                   Editable = p.Max(t => t.Editable),
                   Queryable = p.Max(t => t.Queryable),
                   SelectedStatus = p.Max(t => t.SelectedStatus)
               }).ToDictionary(t => t.TemplateObjectId);
                //把用户权限字典赋值给模板对象字典
                var userTempIdKey = kv.Key.ToString() + "_" + kv.Value.ToString();
                foreach (var privKey in userTempPrivDict.Keys)
                {
                    tempObjects[userTempIdKey][privKey].Enable = userTempPrivDict[privKey].Enable;
                    tempObjects[userTempIdKey][privKey].Visible = userTempPrivDict[privKey].Visible;
                    tempObjects[userTempIdKey][privKey].Editable = userTempPrivDict[privKey].Editable;
                    tempObjects[userTempIdKey][privKey].Queryable = userTempPrivDict[privKey].Queryable;
                    tempObjects[userTempIdKey][privKey].SelectedStatus = userTempPrivDict[privKey].SelectedStatus;
                }
            }
            return tempObjects;
        }
    }
}
