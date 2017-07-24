using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.ProductionScheduleModule.Domain.Services
{
    /// <summary>
    /// 物料需求计划
    /// </summary>
    public interface IMrpManager : IDomainService<Mrp, Guid>
    {
    }
}
