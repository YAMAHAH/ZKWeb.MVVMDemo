using BusinessPlugins.WarehouseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.WarehouseModule.Domain.Services
{
    /// <summary>
    /// 转储申请管理
    /// </summary>
    public interface ITransferRequisitionManager : IDomainService<TransferRequisition, Guid>
    {
    }
}
