using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 岗位管理
    /// </summary>
    public interface IPostGroupManager : IDomainService<PostGroup, Guid>
    {
    }
}
