using BusinessPlugins.ProductionModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.ProductionModule.Domain.Services
{
    /// <summary>
    /// 生产订单管理
    /// </summary>
    public interface IManufactureOrderManager : IDomainService<ManufactureOrder, Guid>
    {
    }
}
