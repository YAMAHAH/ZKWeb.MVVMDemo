﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.TaskSchedulerModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Application.Attributes;

namespace InfrastructurePlugins.TaskSchedulerModule.Application.Dtos
{
    [Description("定时任务的传出信息")]
    [ModelTypeMapper(typeof(ScheduledTask))]
    public class ScheduledTaskOutputDto : IOutputDto
    {
        [Description("任务名")]
        public string Id { get; set; }
        [Description("首次运行时间")]
        public string CreateTime { get; set; }
        [Description("上次运行时间")]
        public string UpdateTime { get; set; }
    }
}
