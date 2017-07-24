using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 数据权限管理
    /// </summary>
    public interface IDataFieldPrivilegeManager : IDomainService<DataFieldPrivilege, Guid>
    {
    }
}
