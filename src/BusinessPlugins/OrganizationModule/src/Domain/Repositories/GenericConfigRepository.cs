using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.MVVMPlugins.OrganizationModule.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.OrganizationModule.src.Domain.Repositories
{
    /// <summary>
    /// 通用配置的仓储
    /// </summary>
    [ExportMany, SingletonReuse]
    public class GenericConfigRepository : RepositoryBase<GenericConfig, Guid> { }
}
