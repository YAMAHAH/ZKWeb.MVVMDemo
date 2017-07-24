using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Services
{
    /// <summary>
    /// 产品工艺路线管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ProductProcessRouteManager : DomainServiceBase<ProductProcessRoute, Guid>,IProductProcessRouteManager
    {

    }
}
