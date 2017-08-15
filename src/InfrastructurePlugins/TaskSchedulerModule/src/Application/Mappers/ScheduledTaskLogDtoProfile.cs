using InfrastructurePlugins.BaseModule.Components.DtoToModelMap;
using InfrastructurePlugins.TaskSchedulerModule.Application.Dtos;
using InfrastructurePlugins.TaskSchedulerModule.Domain.Entities;
using System;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.TaskSchedulerModule.Application.Mappers
{
    [ExportMany]
    public class ScheduledTaskLogDtoProfile : DtoToModelMapProfile<ScheduledTaskLog, ScheduledTaskLogOutputDto, Guid>
    {
        public ScheduledTaskLogDtoProfile()
        {
            FilterKeywordWith(t => new { t.Task.Id })
              .ForMember(u => u.CreateTime, opt => opt.Map(m => m.CreateTime.ToString()))
              .ForMember(u => u.Success, opt => opt.Map(m => m.Success))
              .ForMember(d => d.TaskId, opt => opt.Map(m => m.Task.Id))
              .ForMember(d => d.Error, opt => opt.Map(u => u.Error))
              .ForMember(d => d.Id, (opt) => opt.Map(t => t.Id));
        }
    }
}
