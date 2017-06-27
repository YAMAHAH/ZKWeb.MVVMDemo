using AutoMapper;
using InfrastructurePlugins.TaskSchedulerModule.Application.Dtos;
using InfrastructurePlugins.TaskSchedulerModule.Domain.Entities;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.TaskSchedulerModule.Application.Mappers
{
    /// <summary>
    /// AutoMapper的配置
    /// </summary>
    [ExportMany]
    public class TaskSchedulerMapperProfile : Profile
    {
        public TaskSchedulerMapperProfile()
        {
            // 定时任务
            CreateMap<ScheduledTask, ScheduledTaskOutputDto>();

            // 定时任务记录
            CreateMap<ScheduledTaskLog, ScheduledTaskLogOutputDto>()
                .ForMember(t => t.TaskId, m => m.ResolveUsing(t => t.Task.Id));
        }
    }
}
