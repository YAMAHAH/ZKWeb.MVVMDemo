using BusinessPlugins.BasicModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    /// <summary>
    /// 模板管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class TemplateManager : DomainServiceBase<Template, Guid>, ITemplateManager
    {
    }
}
