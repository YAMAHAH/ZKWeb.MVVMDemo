using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductionScheduleModule.Domain.Services
{
    /// <summary>
    /// 计划订单管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class PlannedOrderManager : DomainServiceBase<PlannedOrder, Guid>, IPlannedOrderManager
    {
    }
}
