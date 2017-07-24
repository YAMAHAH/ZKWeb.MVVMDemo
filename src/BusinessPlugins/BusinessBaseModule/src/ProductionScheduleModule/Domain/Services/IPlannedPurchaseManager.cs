using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.ProductionScheduleModule.Domain.Services
{
    /// <summary>
    /// 计划采购管理
    /// </summary>
    public interface IPlannedPurchaseManager : IDomainService<PlannedPurchase, Guid>
    {
    }
}
