using InfrastructurePlugins.BaseModule.Domain.Repositories.Bases;
using InfrastructurePlugins.TaskSchedulerModule.Domain.Entities;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.TaskSchedulerModule.Domain.Repositories
{
    /// <summary>
    /// 定时任务的仓储
    /// </summary>
    [ExportMany]
    public class ScheduledTaskRepository : RepositoryBase<ScheduledTask, string>
    {
    }
}
