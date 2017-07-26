using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 模板权限管理
    /// </summary>
    public interface ITemplatePrivilegeManager : IDomainService<TemplatePrivilege, Guid>
    {

        /// <summary>
        /// 获取某个角色所拥有的权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        ICollection<TemplatePrivilege> GetRoleTemplatePrivileges(Guid roleId);
        /// <summary>
        /// 获取某一角色某一模板所拥有的角色权限集合
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="tempId">模板ID</param>
        /// <returns></returns>
        ICollection<TemplatePrivilege> GetRoleTemplatePrivileges(Guid roleId, Guid tempId);
        /// <summary>
        /// 获取角色列表的某一模板的权限数据
        /// </summary>
        /// <param name="roles">角色集合</param>
        /// <param name="tempId">模板ID</param>
        /// <returns></returns>
        ICollection<TemplatePrivilege> GetRoleTemplatePrivileges(ICollection<Roles> roles, Guid tempId);
        /// <summary>
        /// 获取某个员工所拥有的权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
     //   ICollection<TemplatePrivilege> GetEmployeeTemplatePrivileges(Guid EmployeeId);
        /// <summary>
        /// 获取某个员工某个模板所拥有的权限
        /// </summary>
        /// <param name="EmployeeId">员工ID</param>
        /// <param name="tempId">模板ID</param>
        /// <returns></returns>
      //  ICollection<TemplatePrivilege> GetEmployeeTemplatePrivileges(Guid EmployeeId, Guid tempId);
        /// <summary>
        /// 获取某个岗位所拥有的权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
      //  ICollection<TemplatePrivilege> GetPostGroupTemplatePrivileges(Guid postGroupId);
        /// <summary>
        /// 获取某个岗位某个模板所拥有的权限
        /// </summary>
        /// <param name="postGroupId">岗位ID</param>
        /// <param name="tempId">模板ID</param>
        /// <returns></returns>
      //  ICollection<TemplatePrivilege> GetPostGroupTemplatePrivileges(Guid postGroupId, Guid tempId);
        /// <summary>
        /// 获取某个用户某个模板所有的权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
      //  ICollection<TemplatePrivilege> GetTemplatePrivileges(Guid employeeId,Guid tempId);

        /// <summary>
        /// 获取条件表达式的角色权限集合
        /// </summary>
        /// <param name="filterExpression">条件表达式</param>
        /// <returns></returns>
        ICollection<TemplatePrivilege> GetTemplatePrivileges(Expression<Func<TemplatePrivilege, bool>> filterExpression);
        /// <summary>
        /// 获取某个用户某个模板的权限
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="tempId"></param>
        /// <returns></returns>
        ICollection<TemplatePrivilege> GetTemplatePrivileges(Guid employeeId,Guid tempId);
        /// <summary>
        /// 获取角色列表的权限数据
        /// </summary>
        /// <param name="roles">角色集合</param>
        /// <returns></returns>
      //  ICollection<TemplatePrivilege> GetTemplatePrivileges(ICollection<Roles> roles);
        /// <summary>
        /// 获取角色集合的某一模板的权限数据
        /// </summary>
        /// <param name="roles">权限集合</param>
        /// <param name="tempId">模板ID</param>
        /// <returns></returns>
       // Dictionary<Guid, TemplatePrivilege> GetTemplatePrivilegeDictionary(ICollection<Roles> roles, Guid tempId);
        /// <summary>
        /// 根据权限列表生成权限字典,合并数据
        /// </summary>
        /// <param name="userPrivilegies">权限数据集合</param>
        /// <returns>权限字典数据</returns>
        Dictionary<Guid, TemplatePrivilege> GetTemplatePrivilegeDictionary(ICollection<TemplatePrivilege> templatePrivilegies);
    }
}
