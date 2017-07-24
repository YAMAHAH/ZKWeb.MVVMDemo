using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 合作伙伴管理
    /// </summary>
    [ExportMany]
    public class PartnerManager : DomainServiceBase<Partner, Guid>, IPartnerManager
    {
    }
}
