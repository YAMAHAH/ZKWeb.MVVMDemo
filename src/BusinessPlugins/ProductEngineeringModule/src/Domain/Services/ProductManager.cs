using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Services
{
    /// <summary>
    /// 产品管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ProductManager : DomainServiceBase<Product, Guid>,IProductManager
    {

    }
}
