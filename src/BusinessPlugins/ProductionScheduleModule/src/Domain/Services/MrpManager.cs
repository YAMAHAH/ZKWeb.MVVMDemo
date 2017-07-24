using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductionScheduleModule.Domain.Services
{
    /// <summary>
    /// 物料需求计划
    /// </summary>
    [ExportMany, SingletonReuse]
    public class MrpManager : DomainServiceBase<Mrp, Guid>, IMrpManager
    {
    }
}
