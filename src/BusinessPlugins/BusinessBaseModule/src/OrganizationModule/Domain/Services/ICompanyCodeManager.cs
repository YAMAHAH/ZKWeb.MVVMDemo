using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.OrganizationModule.Domain.Services
{
    /// <summary>
    /// 公司代码管理
    /// </summary>
    public interface ICompanyCodeManager : IDomainService<CompanyCode, Guid>
    {
    }
}
