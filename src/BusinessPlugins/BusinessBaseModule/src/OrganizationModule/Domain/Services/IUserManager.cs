using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 角户管理
    /// </summary>
    public interface IUserManager : IDomainService<User, Guid>
    {
    }
}
