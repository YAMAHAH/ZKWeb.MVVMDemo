using InfrastructurePlugins.BaseModule.Components.DtoToModelMap;
using InfrastructurePlugins.TaskSchedulerModule.Application.Dtos;
using InfrastructurePlugins.TaskSchedulerModule.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.TaskSchedulerModule.Application.Mappers
{
    [ExportMany]
    public class ScheduledTaskDtoProfile : DtoToModelMapProfile<ScheduledTask, ScheduledTaskOutputDto, string>
    {
        public ScheduledTaskDtoProfile()
        {
            FilterKeywordWith(t => new { t.Id })
            .ForMember(u => u.CreateTime, opt => opt.Map(m => m.CreateTime.ToString()))
            .ForMember(u => u.UpdateTime, opt => opt.Map(m => m.UpdateTime.ToString()))
            .ForMember(d => d.Id, (opt) => opt.Map(t => t.Id));
        }
    }
}
