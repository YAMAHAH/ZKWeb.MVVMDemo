﻿using BusinessPlugins.WarehouseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.WarehouseModule.Domain.Services
{
    /// <summary>
    /// 库存盘点单管理
    /// </summary>
    public interface IInventoryCheckManager : IDomainService<InventoryCheck, Guid>
    {
    }
}
