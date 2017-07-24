using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 部门管理
    /// </summary>
    [ExportMany]
    public class DepartmentManager : DomainServiceBase<Department, Guid>, IDepartmentManager
    {
    }
}
