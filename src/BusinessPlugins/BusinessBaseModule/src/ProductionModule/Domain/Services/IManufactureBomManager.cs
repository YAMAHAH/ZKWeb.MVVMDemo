using BusinessPlugins.ProductionModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.ProductionModule.Domain.Services
{
    /// <summary>
    /// 生产BOM管理
    /// </summary>
    public interface IManufactureBomManager : IDomainService<ManufactureBom, Guid>
    {
    }
}
