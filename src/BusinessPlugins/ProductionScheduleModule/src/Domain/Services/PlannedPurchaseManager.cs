using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductionScheduleModule.Domain.Services
{
    /// <summary>
    /// 计划采购
    /// </summary>
    [ExportMany, SingletonReuse]
    public class PlannedPurchaseManager : DomainServiceBase<PlannedPurchase, Guid>, IPlannedPurchaseManager
    {
    }
}
