using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Services
{
    /// <summary>
    /// 预留管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ReservationManager : DomainServiceBase<Reservation, Guid>, IReservationManager
    {

    }
}
