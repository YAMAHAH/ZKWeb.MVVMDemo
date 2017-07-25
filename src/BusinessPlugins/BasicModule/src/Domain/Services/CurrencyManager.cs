using BusinessPlugins.BasicModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    /// <summary>
    /// 货币管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class CurrencyManager : DomainServiceBase<Currency, Guid>, ICurrencyManager
    {
    }
}
