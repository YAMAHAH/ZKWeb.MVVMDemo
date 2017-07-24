using BusinessPlugins.WarehouseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.WarehouseModule.Domain.Services
{
    /// <summary>
    /// 库存申请管理
    /// </summary>
    public interface IInventoryRequisitionManager : IDomainService<InventoryRequisition, Guid>
    {
    }
}
