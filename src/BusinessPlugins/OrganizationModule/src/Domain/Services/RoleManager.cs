using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWeb.MVVMPlugins.OrganizationModule.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.OrganizationModule.src.Domain.Services
{
    /// <summary>
    /// 角色管理器
    /// </summary>
    [ExportMany, SingletonReuse]
    public class RoleManager : DomainServiceBase<Role, Guid> { }
}
