using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Services
{
    /// <summary>
    /// 客户物料信息管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ProductFeatureValueGroupManager : DomainServiceBase<ProductFeatureValueGroup, Guid>, IProductFeatureValueGroupManager
    {

    }
}
