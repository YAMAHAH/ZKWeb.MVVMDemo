using BusinessPlugins.BasicModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 员工管理
    /// </summary>
    public interface IEmployeeManager : IDomainService<Employee, Guid>
    {
        /// <summary>
        /// 获取员工分配的所有岗位组
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        ICollection<PostGroup> GetPostGroups(Guid empId);

        ICollection<PostGroup> GetPostGroups(Guid[] empIds);

        /// <summary>
        /// 获取员工分配的所有岗位组,包含子岗位组
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        ICollection<PostGroup> GetAllPostGroups(Guid empId);

        ICollection<PostGroup> GetAllPostGroups(Guid[] empIds);

        /// <summary>
        /// 获取所有用户分配的所有组和单个用户分配的组的ID字典
        /// </summary>
        /// <param name="userTempIds"></param>
        /// <returns></returns>
        Dictionary<string, List<Guid>> GetAllPostGroups(IList<KeyValuePair<Guid, Guid>> userTempIds);

        /// <summary>
        /// 获取员工所拥有的所有角色
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        ICollection<Roles> GetRoles(Guid empId);

        ICollection<Roles> GetRoles(Guid[] empIds);

        /// <summary>
        /// 获取员工所拥有的所有角色,包含子角色
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        ICollection<Roles> GetAllRoles(Guid empId);

        ICollection<Roles> GetAllRoles(Guid[] empId);
        Dictionary<string, List<Guid>> GetAllRoles(IList<KeyValuePair<Guid, Guid>> userTempIds);

        /// <summary>
        /// 获取某个员工某个模板权限的模板对象字典
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        Dictionary<Guid, TemplateObject> GetTemplateObjectPrivileges(Guid empId, Guid tempId);

        /// <summary>
        /// 获取多个员工多个模板权限模板对象字典的字典
        /// </summary>
        /// <param name="userTempIds"></param>
        /// <returns></returns>
        Dictionary<string, Dictionary<Guid, TemplateObject>> GetTemplateObjectPrivileges(IList<KeyValuePair<Guid, Guid>> userTempIds);

        /// <summary>
        /// 获取某个员工某个模板某个权限
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="tempId"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        TemplateObject GetTemplateObjectPrivilege(Guid empId, Guid tempId, Guid objectId);

        /// <summary>
        /// 获取某个用户拥有的所有模板
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        List<Template> GetTemplatePrivileges(Guid empId);

    }
}
