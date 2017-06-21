using System;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using BusinessPlugins.CrudExampleModule.Domain.Entities;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.CrudExampleModule.Domain.Services
{
    /// <summary>
    /// 示例数据管理器
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ExampleDataManager : DomainServiceBase<ExampleData, Guid>
    {
    }
}
