using BusinessPlugins.ProductionModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductionModule.Domain.Services
{
    /// <summary>
    /// 生产订单管理 
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ManufactureOrderManager : DomainServiceBase<ManufactureOrder, Guid>, IManufactureOrderManager
    {
    }
}
