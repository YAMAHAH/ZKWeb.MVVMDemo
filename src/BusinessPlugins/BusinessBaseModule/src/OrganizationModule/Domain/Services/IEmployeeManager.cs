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
        /// <summary>
        /// 获取员工分配的所有岗位组,包含子岗位组
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        ICollection<PostGroup> GetAllPostGroups(Guid empId);
        /// <summary>
        /// 获取员工所拥有的所有角色
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        ICollection<Roles> GetRoles(Guid empId);
        /// <summary>
        /// 获取员工所拥有的所有角色,包含子角色
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        ICollection<Roles> GetAllRoles(Guid empId);
        /// <summary>
        /// 获取某个员工某个模板权限的模板对象字典
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        Dictionary<Guid, TemplateObject> GetTemplateObjectPrivileges(Guid empId, Guid tempId);
    }
}
