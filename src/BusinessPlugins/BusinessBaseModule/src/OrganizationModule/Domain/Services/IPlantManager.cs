using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 工厂管理
    /// </summary>
    public interface IPlantManager : IDomainService<Plant, Guid>
    {
    }
}
