using BusinessPlugins.PurchaseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.PurchaseModule.Domain.Services
{
    /// <summary>
    /// 采购订单管理
    /// </summary>
    public interface IPurchaseOrderManager : IDomainService<PurchaseOrder, Guid>
    {

    }

}
