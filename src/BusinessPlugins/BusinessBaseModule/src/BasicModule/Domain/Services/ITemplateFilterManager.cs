using BusinessPlugins.BasicModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    /// <summary>
    /// 模板过滤器管理
    /// </summary>
    public interface ITemplateFilterManager : IDomainService<TemplateFilter, Guid>
    {
    }
}
