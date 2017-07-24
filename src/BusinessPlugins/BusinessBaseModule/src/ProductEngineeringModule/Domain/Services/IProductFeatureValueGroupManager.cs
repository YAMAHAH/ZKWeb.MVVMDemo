using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Services
{
    /// <summary>
    /// 产品特性值管理
    /// </summary>
    public interface IProductFeatureValueGroupManager : IDomainService<ProductFeatureValueGroup, Guid>
    {
    }
}
