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

        public Dictionary<string, List<Guid>> GetAllPostGroups(IList<KeyValuePair<Guid, Guid>> userTempIds)
        {
            //获取所有用户的ID
            var empIds = userTempIds.Select(t => t.Key).ToArray();
            //获取用户分配的所有组
            var empPostGroups = GetEmployeePostGroups(p => empIds.Contains(p.EmployeeId));
            //获取用户分配的所有组ID
            var empPostGroupIds = empPostGroups.Select(p => p.PostGroupId);
            //获取组仓储
            var postGrpRep = UnitOfWork.GetUnitRepository<PostGroup, Guid>();
            //获取所有组对象
            var allRootPostGroups = postGrpRep.FastQueryAsReadOnly().Where(p => empPostGroupIds.Contains(p.Id)).ToList();
            //获取所有组对象rootid和id
            var postGrpIds = allRootPostGroups.Select(p => new { p.Id, p.RootId });
            //获取组的结点IDs
            var nodeIds = postGrpIds.Select(i => i.Id).Distinct().Cast<object>().ToList();
            //获取组的根结点IDs
            var rootIds = postGrpIds.Select(i => i.RootId).Distinct().Cast<object>().ToList();
            Dictionary<string, List<Guid>> userPostGrpIdDict = new Dictionary<string, List<Guid>>();
            //获取所有组及子组对象
            var allPostGroups = postGrpRep.FastGetManyTreeNodes(nodeIds, rootIds);
            //获取所有的组及子组对象的ID
            var allPostGroupIds = allPostGroups.Select(p => p.Id).ToList();
            //从本地数据获取某个用户某个模板的相关数据
            userPostGrpIdDict.Add("allEmpGrpIds", allPostGroupIds);
            foreach (var kv in userTempIds)
            {
                //从本地获取某个用户分配的组ID
                var localEmpPostGroupIds = empPostGroups.Where(p => p.EmployeeId == kv.Key).Select(p => p.PostGroupId);
                //从本地获取某个用户分配的组
                var localEmpPostRoots = allRootPostGroups.Where(p => localEmpPostGroupIds.Contains(p.Id)).Select(p => p.Id);
                //从本地获取某个用户分配的所有组
                List<PostGroup> localEmpAllPost = new List<PostGroup>();
                foreach (var rNode in allPostGroups.Where(p => localEmpPostRoots.Contains(p.Id)))
                {
                    localEmpAllPost.AddRange(postGrpRep.GetAllNodes(rNode, p => p.Childs));
                }
                userPostGrpIdDict.Add(string.Join("_", kv.Key, kv.Value), localEmpAllPost.Select(p => p.Id).ToList());
            }
            return userPostGrpIdDict;
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
        public Dictionary<string, List<Guid>> GetAllRoles(IList<KeyValuePair<Guid, Guid>> userTempIds)
        {
            //获取所有用户的ID
            var empIds = userTempIds.Select(t => t.Key).ToArray();
            var empRoles = GetEmployeeRoles(e => empIds.Contains(e.EmployeeId));
            //获取所有员工分配的所有角色ID
            var empRoleIds = empRoles.Select(e => e.RoleId);
            //获取所有员工分配的所有组字典
            var empPostIdDict = GetAllPostGroups(userTempIds);
            //获取所有用户分配的所有组ID
            var empPostIds = empPostIdDict["allEmpGrpIds"];
            //获取组分配的所有角色
            var postRoles = UnitOfWork.GetUnitRepository<PostGroupRole, Guid>()
                .FastQueryAsReadOnly()
                .Where(p => empPostIds.Contains(p.PostGroupId))
                .ToList();
            //获取组分配的所有角色ID
            var postRoleIds = postRoles
                .Select(p => p.RoleId)
                .ToList();
            //员工角色和岗位角色合并
            var allRoleIds = empRoleIds.Union(postRoleIds).ToList();
            //获取角色仓储
            var roleRep = UnitOfWork.GetUnitRepository<Roles, Guid>();
            //获取所有根角色
            var rootRoles = roleRep.FastQueryAsReadOnly()
            .Where(p => allRoleIds.Contains(p.Id))
            .ToList();
            //获取所有组对象,并得出相应的rootid和id
            var Ids = rootRoles
             .Select(p => new { p.Id, p.RootId });
            //根据rootid和id查询出角色及子角色的对象
            var nodeIds = Ids.Select(i => i.Id).Distinct().Cast<object>().ToList();
            var rootIds = Ids.Select(i => i.RootId).Distinct().Cast<object>().ToList();
            //获取用户分配的所有角色
            var localEmpRoles = roleRep.FastGetManyTreeNodes(nodeIds, rootIds);
            //获取用户分配的所有角色ID
            var allEmpRoleIds = localEmpRoles.Select(e => e.Id).ToList();
            Dictionary<string, List<Guid>> userRoleIdDict = new Dictionary<string, List<Guid>>();
            //从本地数据获取某个用户某个模板的相关数据
            userRoleIdDict.Add("allEmpRoleIds", allEmpRoleIds);
            foreach (var kv in userTempIds)
            {
                //从本地获取某个用户分配的角色ID
                var localEmpRoleIds = empRoles.Where(p => p.EmployeeId == kv.Key).Select(p => p.RoleId);
                //从本地获取某个用户分配组分配的所有的组ID
                var localEmpPostIds = empPostIdDict[string.Join("_", kv.Key, kv.Value)];
                //从本地获取某个用户分配组分配的所有的角色ID
                var localPostRoleIds = postRoles.Where(r => localEmpPostIds.Contains(r.PostGroupId)).Select(r => r.RoleId);
                //合并员工角色和岗位角色
                var localRoleIds = localEmpRoleIds.Union(localPostRoleIds).ToList();
                //从本地获取某个用户分配的所有组
                List<Roles> localEmpAllRole = new List<Roles>();
                foreach (var rNode in localEmpRoles.Where(r => localRoleIds.Contains(r.Id)))
                {
                    localEmpAllRole.AddRange(roleRep.GetAllNodes(rNode, p => p.Childs));
                }
                userRoleIdDict.Add(string.Join("_", kv.Key, kv.Value), localEmpAllRole.Select(p => p.Id).ToList());
            }
            return userRoleIdDict;
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

        private ICollection<EmployeeRole> GetEmployeeRoles(Func<EmployeeRole, bool> filter)
        {
            return UnitOfWork.GetUnitRepository<EmployeeRole, Guid>()
                 .FastQueryAsReadOnly()
                 .Where(filter)
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

        private ICollection<EmployeePostGroup> GetEmployeePostGroups(Func<EmployeePostGroup, bool> filter)
        {
            return UnitOfWork.GetUnitRepository<EmployeePostGroup, Guid>()
                 .FastQueryAsReadOnly()
                 .Where(filter)
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
            return userTempPrivDicts.Count() > 0 ? tempObjects : null;
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
            //获取所有用户ID
            var empIds = userTempIds.Select(u => u.Key).ToArray();
            //获取所有用户分配的所有组ID字典,包含单用户分配
            var postGrpIdDict = GetAllPostGroups(userTempIds);
            //获取用户分配的角色ID列表,包含单用户分配
            var roleIdDict = GetAllRoles(userTempIds);

            //获取模板的所有模板对象字典 
            var tempObjects = xTempManager.GetTemplateObjects(userTempIds);
            //获取所有的模板ID
            var tempIds = userTempIds.Select(t => t.Value).ToArray();

            //获取模板权限对象仓储
            var tempPrivRep = UnitOfWork.GetUnitRepository<TemplatePrivilege, Guid>();
            //获取所有用户的所有组ID
            var allPostGrpIds = postGrpIdDict["allEmpGrpIds"];
            //获取所有用户的所有角色ID
            var allRoleIds = roleIdDict["allEmpRoleIds"];
            //获取所有用户模板分配的的所有权限对象
            var userTempPrivs = tempPrivRep.FastQueryAsReadOnly()
               .Where(p => (empIds.Contains((Guid)p.EmployeeId) || allPostGrpIds.Contains((Guid)p.PostGroupId) || allRoleIds.Contains((Guid)p.RoleId)) && tempIds.Contains(p.TemplateId))
               .ToList();
            //单个用户单个模板权限对象处理
            var needRemoveKeys = new List<string>();
            foreach (var kv in userTempIds)
            {
                //获取字典的KEY
                var userTempIdKey = string.Join("_", kv.Key, kv.Value);
                var empId = kv.Key;
                //获取员工分配的所有岗位
                var localPostGrpIds = postGrpIdDict[userTempIdKey];
                //获取员工分配的所有角色
                var localRoleIds = roleIdDict[userTempIdKey];
                //获取用户模板分配的权限对象字典
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
                foreach (var privKey in userTempPrivDict.Keys)
                {
                    tempObjects[userTempIdKey][privKey].Enable = userTempPrivDict[privKey].Enable;
                    tempObjects[userTempIdKey][privKey].Visible = userTempPrivDict[privKey].Visible;
                    tempObjects[userTempIdKey][privKey].Editable = userTempPrivDict[privKey].Editable;
                    tempObjects[userTempIdKey][privKey].Queryable = userTempPrivDict[privKey].Queryable;
                    tempObjects[userTempIdKey][privKey].SelectedStatus = userTempPrivDict[privKey].SelectedStatus;
                }
                if (!(userTempPrivDict.Count() > 0))
                {
                    needRemoveKeys.Add(userTempIdKey);
                }
            }
            //删除没有权限的对象
            foreach (var key in needRemoveKeys)
            {
                tempObjects.Remove(key);
            }
            return tempObjects;
        }

        public List<Template> GetTemplatePrivileges(Guid empId)
        {
            //获取员工分配的所有岗位组ID
            var postGrpIds = GetAllPostGroups(empId).Select(e => e.Id).ToList();
            //获取用户分配的角色ID列表
            var roleIds = GetAllRoles(empId).Select(r => r.Id).ToList();
            //获取模板权限对象仓储
            var tempPrivRep = UnitOfWork.GetUnitRepository<TemplatePrivilege, Guid>();
            //获取用户分配的模板ID
            var userTempIds = tempPrivRep.FastQueryAsReadOnly()
               .Where(p => (p.EmployeeId == empId || postGrpIds.Contains((Guid)p.PostGroupId) || roleIds.Contains((Guid)p.RoleId)))
               .GroupBy(p => p.TemplateId)
               .Select(t => t.Key);
            //获取模板仓储
            var tempRep = UnitOfWork.GetUnitRepository<Template, Guid>();
            //获取用户拥有的所有模板
            var userTemps = tempRep.FastQueryAsReadOnly()
                .Where(t => userTempIds.Contains(t.Id))
                .ToList();
            return userTemps.Count() > 0 ? userTemps : null;
        }
    }
}
