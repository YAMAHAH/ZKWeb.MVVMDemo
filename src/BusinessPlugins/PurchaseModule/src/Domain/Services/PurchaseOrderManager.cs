using BusinessPlugins.PurchaseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.PurchaseModule.Domain.Services
{
    /// <summary>
    /// 采购订单管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class PurchaseOrderManager : DomainServiceBase<PurchaseOrder, Guid>, IPurchaseOrderManager
    {

    }
}
