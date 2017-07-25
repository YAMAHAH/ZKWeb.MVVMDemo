using BusinessPlugins.BasicModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    /// <summary>
    /// 计划边际管理
    /// </summary>
    public interface IScheduleMarginKeyManager : IDomainService<ScheduleMarginKey, Guid>
    {

    }
}
