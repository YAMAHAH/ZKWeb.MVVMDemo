using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using InfrastructurePlugins.BaseModule.Domain.Repositories.Bases;
using InfrastructurePlugins.TaskSchedulerModule.Domain.Entities;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.TaskSchedulerModule.Domain.Repositories
{
    /// <summary>
    /// 定时任务执行记录的仓储
    /// </summary>
    [ExportMany]
    public class ScheduledTaskLogRepository : RepositoryBase<ScheduledTaskLog, Guid>
    {
        /// <summary>
        /// 查询时包含关联数据
        /// </summary>
        /// <returns></returns>
        public override IQueryable<ScheduledTaskLog> Query()
        {
            return base.Query().Include(t => t.Task);
        }
    }
}
