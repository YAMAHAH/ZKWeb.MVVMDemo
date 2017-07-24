using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.ProductionScheduleModule.Domain.Services
{
    /// <summary>
    /// 主需求计划管理
    /// </summary>
    public interface IMdsManager : IDomainService<Mds, Guid>
    {
    }
}
