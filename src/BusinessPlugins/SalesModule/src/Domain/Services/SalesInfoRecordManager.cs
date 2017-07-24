using BusinessPlugins.SalesModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Services
{
    /// <summary>
    /// 销售信息记录
    /// </summary>
    [ExportMany, SingletonReuse]
    public class SalesInfoRecordManager : DomainServiceBase<SalesInfoRecord, Guid>, ISalesInfoRecordManager
    {
    }
}
