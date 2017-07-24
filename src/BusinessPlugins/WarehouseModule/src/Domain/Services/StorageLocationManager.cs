using BusinessPlugins.WarehouseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Services
{
    /// <summary>
    /// 库存地点管理
    /// 仓库/储位/分区
    /// </summary>
    [ExportMany, SingletonReuse]
    public class StorageLocationManager : DomainServiceBase<StorageLocation, Guid>, IStorageLocationManager
    {
    }
}
