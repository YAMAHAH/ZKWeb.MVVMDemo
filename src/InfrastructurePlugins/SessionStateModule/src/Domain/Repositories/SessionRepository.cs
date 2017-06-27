using System;
using InfrastructurePlugins.BaseModule.Domain.Repositories.Bases;
using InfrastructurePlugins.SessionStateModule.Domain.Entities;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.SessionStateModule.Domain.Repositories
{
    /// <summary>
    /// 会话的仓储
    /// </summary>
    [ExportMany, SingletonReuse]
    public class SessionRepository : RepositoryBase<Session, Guid> { }
}
