using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Services
{
    /// <summary>
    /// 物料库存管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class MaterialInventoryManager : DomainServiceBase<MaterialInventory, Guid>, IMaterialInventoryManager
    {

    }
}
