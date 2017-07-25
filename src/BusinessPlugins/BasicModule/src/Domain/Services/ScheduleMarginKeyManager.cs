using BusinessPlugins.BasicModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    /// <summary>
    /// 计划边际管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ScheduleMarginKeyManager : DomainServiceBase<ScheduleMarginKey, Guid>, IScheduleMarginKeyManager
    {
    }
}
