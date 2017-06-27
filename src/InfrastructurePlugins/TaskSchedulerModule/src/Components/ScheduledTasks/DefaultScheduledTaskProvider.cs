using System;
using System.Collections.Generic;
using InfrastructurePlugins.TaskSchedulerModule.Components.ScheduledTasks.Interfaces;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.TaskSchedulerModule.Components.ScheduledTasks
{
    /// <summary>
    /// 默认的任务提供器
    /// </summary>
    [ExportMany]
    public class DefaultScheduledTaskProvider : IScheduledTaskProvider
    {
        /// <summary>
        /// 查找在Ioc注册的IScheduledTask
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IScheduledTask> GetTasks()
        {
            return ZKWeb.Application.Ioc.ResolveMany<IScheduledTask>();
        }
    }
}
