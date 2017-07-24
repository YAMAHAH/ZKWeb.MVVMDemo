using BusinessPlugins.WarehouseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.WarehouseModule.Domain.Services
{
    /// <summary>
    /// 转储订单管理
    /// </summary>
    public interface ITransferOrderManager : IDomainService<TransferOrder, Guid>
    {
    }
}
