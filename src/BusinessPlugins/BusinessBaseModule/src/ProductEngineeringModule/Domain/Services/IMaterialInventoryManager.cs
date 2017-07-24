using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Services
{
    /// <summary>
    /// 物料库存管理
    /// </summary>
    public interface IMaterialInventoryManager : IDomainService<MaterialInventory, Guid>
    {
    }
}
