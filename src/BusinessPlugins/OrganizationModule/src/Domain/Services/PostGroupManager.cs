using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 岗位管理
    /// </summary>
    [ExportMany]
    public class PostGroupManager : DomainServiceBase<PostGroup, Guid>, IPostGroupManager
    {
    }
}
