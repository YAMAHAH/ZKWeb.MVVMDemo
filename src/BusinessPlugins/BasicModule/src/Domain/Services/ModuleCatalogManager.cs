using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using BusinessPlugins.BasicModule.Domain.Entities;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    /// <summary>
    /// 模块管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ModuleCatalogManager : DomainServiceBase<ModuleCatalog, Guid>, IModuleCatalogManager
    {
    }
}
