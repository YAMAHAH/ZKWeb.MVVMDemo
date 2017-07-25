using BusinessPlugins.BasicModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    /// <summary>
    /// 货币管理
    /// </summary>
    public interface ICurrencyManager : IDomainService<Currency, Guid>
    {

    }
}
