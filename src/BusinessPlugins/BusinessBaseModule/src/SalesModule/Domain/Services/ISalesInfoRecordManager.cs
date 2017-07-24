using BusinessPlugins.SalesModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.SalesModule.Domain.Services
{
    /// <summary>
    /// 销售价目管理
    /// </summary>
    public interface ISalesInfoRecordManager : IDomainService<SalesInfoRecord, Guid>
    {

    }
}
