using System;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using InfrastructurePlugins.TaskSchedulerModule.Domain.Entities;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.TaskSchedulerModule.Domain.Services
{
    /// <summary>
    /// 定时任务记录管理器
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ScheduledTaskLogManager : DomainServiceBase<ScheduledTaskLog, Guid>
    {
    }
}
