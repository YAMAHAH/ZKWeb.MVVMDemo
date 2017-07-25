using BusinessPlugins.BasicModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    /// <summary>
    /// 模板类管理
    /// </summary>
    public interface ITemplateClassManager : IDomainService<TemplateClass, Guid>
    {

    }
}
