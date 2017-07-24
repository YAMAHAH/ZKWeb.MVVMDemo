using BusinessPlugins.WarehouseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.WarehouseModule.Domain.Services
{
    /// <summary>
    /// 库存地点管理
    /// 包含仓库/储位/分区管理
    /// </summary>
    public interface IStorageLocationManager : IDomainService<StorageLocation, Guid>
    {
    }
}
