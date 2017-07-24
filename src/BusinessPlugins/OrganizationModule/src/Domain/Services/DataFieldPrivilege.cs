using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 数据权限管理
    /// </summary>
    [ExportMany]
    public class DataFieldPrivilegeManager : DomainServiceBase<DataFieldPrivilege, Guid>, IDataFieldPrivilegeManager
    {
    }
}
