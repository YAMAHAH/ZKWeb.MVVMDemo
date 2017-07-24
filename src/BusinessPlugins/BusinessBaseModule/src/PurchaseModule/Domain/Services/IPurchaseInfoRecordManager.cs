using BusinessPlugins.PurchaseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.PurchaseModule.Domain.Services
{

    /// <summary>
    /// 采购信息记录管理
    /// </summary>
    public interface IPurchaseInfoRecordManager : IDomainService<PurchaseInfoRecord, Guid>
    {

    }
}
