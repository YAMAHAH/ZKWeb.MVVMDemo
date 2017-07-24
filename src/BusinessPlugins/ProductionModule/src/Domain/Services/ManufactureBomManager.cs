using BusinessPlugins.ProductionModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductionModule.Domain.Services
{
    /// <summary>
    /// 制造BOM管理 
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ManufactureBomManager : DomainServiceBase<ManufactureBom, Guid>, IManufactureBomManager
    {
    }
}
