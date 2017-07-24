using BusinessPlugins.PurchaseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.PurchaseModule.Domain.Services
{
    /// <summary>
    /// 采购申请管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class PurchaseRequisitionManager : DomainServiceBase<PurchaseRequisition, Guid>, IPurchaseRequisitionManager
    {
    }
}
