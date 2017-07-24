using BusinessPlugins.PurchaseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.PurchaseModule.Domain.Services
{
    /// <summary>
    /// 采购信息记录管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class PurchaseInfoRecordManager : DomainServiceBase<PurchaseInfoRecord, Guid>, IPurchaseInfoRecordManager
    {
    }
}
