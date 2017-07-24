using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 合作伙伴管理
    /// </summary>
    public interface IPartnerManager : IDomainService<Partner, Guid>
    {
    }
}
