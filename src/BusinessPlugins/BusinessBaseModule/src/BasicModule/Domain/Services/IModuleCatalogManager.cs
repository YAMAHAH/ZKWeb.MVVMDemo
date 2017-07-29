using BusinessPlugins.BasicModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    /// <summary>
    /// 模块管理
    /// </summary>
    public interface IModuleCatalogManager : IDomainService<ModuleCatalog, Guid>
    {

    }
}
