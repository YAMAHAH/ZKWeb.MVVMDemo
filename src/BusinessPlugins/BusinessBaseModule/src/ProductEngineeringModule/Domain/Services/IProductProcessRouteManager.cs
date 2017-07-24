using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Services
{
    /// <summary>
    /// 产品工艺路线管理
    /// </summary>
    public interface IProductProcessRouteManager : IDomainService<ProductProcessRoute, Guid>
    {
    }
}
