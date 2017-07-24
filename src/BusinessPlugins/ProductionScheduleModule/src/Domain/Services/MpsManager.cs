using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductionScheduleModule.Domain.Services
{
    /// <summary>
    /// 主生产计划
    /// </summary>
    [ExportMany, SingletonReuse]
    public class MpsManager : DomainServiceBase<Mps, Guid>, IMpsManager
    {
    }
}
