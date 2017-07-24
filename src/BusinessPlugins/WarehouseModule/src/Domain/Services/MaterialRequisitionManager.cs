using BusinessPlugins.WarehouseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Services
{
    /// <summary>
    /// 交货管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class DeliveryOrderManager : DomainServiceBase<DeliveryOrder, Guid>, IDeliveryOrderManager
    {
    }
}
