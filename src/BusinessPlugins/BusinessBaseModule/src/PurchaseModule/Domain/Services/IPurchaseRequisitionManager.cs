using BusinessPlugins.PurchaseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.PurchaseModule.Domain.Services
{

    /// <summary>
    /// 采购申请管理
    /// </summary>
    public interface IPurchaseRequisitionManager : IDomainService<PurchaseRequisition, Guid>
    {

    }
}
