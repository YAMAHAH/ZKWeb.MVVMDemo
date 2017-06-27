using System;
using ZKWeb.Logging;
using InfrastructurePlugins.SessionStateModule.Domain.Services;
using InfrastructurePlugins.TaskSchedulerModule.Components.ScheduledTasks.Interfaces;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.SessionStateModule.Components.ScheduledTasks
{
    /// <summary>
    /// 会话清理器
    /// 每小时删除一次过期的会话
    /// </summary>
    [ExportMany, SingletonReuse]
    public class SessionCleaner : IScheduledTask
    {
        /// <summary>
        /// 任务键名
        /// </summary>
        public string Key { get { return "SessionCleaner"; } }

        /// <summary>
        /// 每小时执行一次
        /// </summary>
        public bool ShouldExecuteNow(DateTime lastExecuted)
        {
            return ((DateTime.UtcNow - lastExecuted).TotalHours > 1.0);
        }

        /// <summary>
        /// 删除过期的会话
        /// </summary>
        public void Execute()
        {
            var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
            var count = sessionManager.RemoveExpiredSessions();
            var logManager = ZKWeb.Application.Ioc.Resolve<LogManager>();
            logManager.LogInfo(string.Format(
                "SessionCleaner executed, {0} sessions removed", count));
        }
    }
}
