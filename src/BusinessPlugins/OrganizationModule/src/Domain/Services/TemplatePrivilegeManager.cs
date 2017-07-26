using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 模板权限管理
    /// </summary>
    [ExportMany]
    public class TemplatePrivilegeManager : DomainServiceBase<TemplatePrivilege, Guid>, ITemplatePrivilegeManager
    {
        public ICollection<TemplatePrivilege> GetRoleTemplatePrivileges(Guid roleId)
        {
            return UnitRepository.FastQueryAsReadOnly()
                                         .Where(up => up.RoleId == roleId)
                                         .ToList();
        }

        public ICollection<TemplatePrivilege> GetRoleTemplatePrivileges(Guid roleId, Guid tempId)
        {
            return UnitRepository.FastQueryAsReadOnly()
                                 .Where(p => p.RoleId == roleId && p.TemplateId == tempId)
                                 .ToList();
        }

        public ICollection<TemplatePrivilege> GetRoleTemplatePrivileges(ICollection<Roles> roles, Guid tempId)
        {
            var roleIds = roles.Where(r => r.Id != Guid.Empty).Select(r => r.Id).ToList();
            return GetTemplatePrivileges(p => roleIds.Contains((Guid)p.RoleId) && p.TemplateId == tempId);
        }

        public Dictionary<Guid, TemplatePrivilege> GetTemplatePrivilegeDictionary(ICollection<TemplatePrivilege> templatePrivilegies)
        {
            Dictionary<Guid, TemplatePrivilege> userPrivilegeDicts = new Dictionary<Guid, TemplatePrivilege>();
            var tempPrivGroups = templatePrivilegies.GroupBy(p => new { p.TemplateId, p.TemplateObjectId });
            TemplatePrivilege xTempPriv = null;
            foreach (var tempPrivGroup in tempPrivGroups)
            {
                xTempPriv = null;
                foreach (var tempPriv in tempPrivGroup)
                {
                    if (xTempPriv == null)
                    {
                        xTempPriv = tempPriv;
                        continue;
                    }
                    ObjectOrOperation(tempPriv, xTempPriv);
                }
                userPrivilegeDicts.Add(xTempPriv.TemplateObjectId, xTempPriv);
            }
            return userPrivilegeDicts;
        }

        public ICollection<TemplatePrivilege> GetTemplatePrivileges(Expression<Func<TemplatePrivilege, bool>> filterExpression)
        {
            return UnitRepository.FastQueryAsReadOnly()
                                 .Where(filterExpression)
                                 .ToList();
        }

        public ICollection<TemplatePrivilege> GetTemplatePrivileges(Guid employeeId, Guid tempId)
        {
            //获取用户id拥有的组,角色

            //Guid[] postGroupIds;
            //Guid[] roleIds;
            UnitRepository.FastQueryAsReadOnly()
                .Where(p => (p.EmployeeId == employeeId) && p.TemplateId == tempId)
                .GroupBy(p => new { p.TemplateId, p.TemplateObjectId })
                .Select(p => new
                {
                    p.Key.TemplateId,
                    p.Key.TemplateObjectId,
                    Visible = p.Max(t => t.Visible),
                    Enable = p.Max(t => t.Enable),
                    Editable = p.Max(t => t.Editable),
                    Queryable = p.Max(t => t.Queryable)
                });
            //获取模板的权限字典
            //把分组的结果传递给权限字典

            //.Select(g => new
            //{
            //    g.TemplateId,
            //    g.TemplateObjectId,
            //    g.Visible,
            //    g.Enable,
            //    g.Editable,
            //    g.Queryable,
            //    BinVal = String.Concat(
            //        Convert.ToInt32(g.Visible),
            //        Convert.ToInt32(g.Enable),
            //        Convert.ToInt32(g.Editable),
            //        Convert.ToInt32(g.Queryable)),
            //    DelVal = Convert.ToInt32(String.Concat(Convert.ToInt32(g.Visible),
            //    Convert.ToInt32(g.Enable),
            //    Convert.ToInt32(g.Editable),
            //    Convert.ToInt32(g.Queryable), 10))
            //});
            return null;
        }

        /// <summary>
        /// 对象或运算
        /// </summary>
        /// <param name="newObject"></param>
        /// <param name="oldObject"></param>
        private void ObjectOrOperation(object newObject, object oldObject)
        {
            if (newObject.GetType() == oldObject.GetType())
            {
                PropertyInfo[] properties = newObject.GetType().GetProperties();
                foreach (var p in properties)
                {
                    var newValue = p.GetValue(newObject, null);
                    var oldValue = p.GetValue(oldObject, null);
                    if (newValue != oldValue && newValue != null)
                    {
                        if (p.PropertyType.Name == "Boolean")
                        {
                            p.SetValue(oldObject, (bool)oldValue || (bool)newValue, null);
                        }
                        else
                        {
                            p.SetValue(oldObject, newValue, null);
                        }
                    }
                }
            }
        }
    }
}
