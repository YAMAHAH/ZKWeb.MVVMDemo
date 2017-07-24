using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.ProductionScheduleModule.Domain.Services
{
    /// <summary>
    /// 计划订单管理
    /// </summary>
    public interface IPlannedOrderManager : IDomainService<PlannedOrder, Guid>
    {
    }
}
