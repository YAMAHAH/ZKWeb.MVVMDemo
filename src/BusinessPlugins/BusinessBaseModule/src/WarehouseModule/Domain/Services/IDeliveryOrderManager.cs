using BusinessPlugins.WarehouseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.WarehouseModule.Domain.Services
{
    /// <summary>
    /// 交货单管理
    /// </summary>
    public interface IDeliveryOrderManager : IDomainService<DeliveryOrder, Guid>
    {
    }
}
