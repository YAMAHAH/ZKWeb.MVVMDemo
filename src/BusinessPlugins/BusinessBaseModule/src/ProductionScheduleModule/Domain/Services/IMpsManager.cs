using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.ProductionScheduleModule.Domain.Services
{
    /// <summary>
    /// 主生产计划管理
    /// </summary>
    public interface IMpsManager : IDomainService<Mps, Guid>
    {
    }
}
