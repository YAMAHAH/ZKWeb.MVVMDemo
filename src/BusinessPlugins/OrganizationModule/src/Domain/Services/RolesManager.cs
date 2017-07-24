using System;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 角色管理器
    /// </summary>
    [ExportMany, SingletonReuse]
    public class RolesManager : DomainServiceBase<Roles, Guid>, IRoleManager { }
}
