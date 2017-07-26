using BusinessPlugins.BasicModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain.Services;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    /// <summary>
    /// 模板管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class TemplateManager : DomainServiceBase<Template, Guid>, ITemplateManager
    {
        private ITemplatePrivilegeManager xTempPrivManager;
        private IEmployeeManager xEmpManager;
        private IEmployeePostGroupManager xEmpPostGrpManager;
        public TemplateManager(
            ITemplatePrivilegeManager tempPrivManager,
            IEmployeeManager empManager,
            IEmployeePostGroupManager empPostGrpManager)
        {
            xTempPrivManager = tempPrivManager;
            xEmpManager = empManager;
            xEmpPostGrpManager = empPostGrpManager;
        }
        public Dictionary<Guid, TemplateObject> GetTemplateObjects(Guid empId, Guid tempId)
        {
            //获取员工分配的所有岗位组ID
            var postGrpIds = xEmpPostGrpManager.GetPostGroups(empId).Select(e => e.Id).ToList();
            //获取用户分配的角色ID列表
            var roleIds = xEmpManager.GetAllRoles(empId).Select(r => r.Id).ToList();

            //获取模板的所有模板对象字典 
            var allTempObjects = GetTemplateObjects(tempId);

            //获取模板权限对象仓储
            var tempPrivRepository = UnitOfWork.GetUnitRepository<TemplatePrivilege, Guid>();
            //获取用户模板分配的权限对象字典
            var userTempPrivDicts = tempPrivRepository.FastQueryAsReadOnly()
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

        public Dictionary<Guid, TemplateObject> GetTemplateObjects(Guid templateId)
        {
            var tempObjRepository = UnitOfWork.GetUnitRepository<TemplateObject, Guid>();

            return tempObjRepository.FastQueryAsReadOnly()
                .Where(to => to.TempId == templateId)
                .Include(t => t.Template)
                .ToDictionary(t => t.Id);
        }
    }
}
