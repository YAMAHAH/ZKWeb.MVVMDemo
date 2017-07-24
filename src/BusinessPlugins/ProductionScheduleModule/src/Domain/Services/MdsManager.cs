using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductionScheduleModule.Domain.Services
{
    /// <summary>
    /// 主需求计划管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class MdsManager : DomainServiceBase<Mds, Guid>, IMdsManager
    {
    }
}
