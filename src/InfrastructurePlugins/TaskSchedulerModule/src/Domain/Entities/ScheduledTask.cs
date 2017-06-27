﻿using System;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.TaskSchedulerModule.Domain.Entities
{
    /// <summary>
    /// 定时任务
    /// </summary>
    [ExportMany]
    public class ScheduledTask :
        IEntity<string>,
        IHaveCreateTime,
        IHaveUpdateTime,
        IEntityMappingProvider<ScheduledTask>
    {
        /// <summary>
        /// 主键，任务键名
        /// </summary>
        public virtual string Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后一次执行的时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }

        /// <summary>
        /// 配置数据库结构
        /// </summary>
        public virtual void Configure(IEntityMappingBuilder<ScheduledTask> builder)
        {
            builder.Id(t => t.Id);
            builder.Map(t => t.CreateTime);
            builder.Map(t => t.UpdateTime);
        }
    }
}
