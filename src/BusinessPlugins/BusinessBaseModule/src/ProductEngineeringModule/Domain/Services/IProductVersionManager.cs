using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Services
{
    /// <summary>
    /// 产品版次管理
    /// </summary>
    public interface IProductVersionManager : IDomainService<ProductVersion, Guid>
    {
    }
}
