using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 员工管理
    /// </summary>
    [ExportMany]
    public class EmployeeManager : DomainServiceBase<Employee, Guid>, IEmployeeManager
    {
    }
}
