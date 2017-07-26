using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 工厂管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class PlantManager : DomainServiceBase<Plant, Guid>, IPlantManager
    {
    }
}
