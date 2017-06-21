﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using InfrastructurePlugins.BaseModule.Domain.Repositories.Bases;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Repositories
{
    /// <summary>
    /// 用户角色的仓储
    /// </summary>
    [ExportMany, SingletonReuse]
    public class RoleRepository : RepositoryBase<Role, Guid>
    {
        /// <summary>
        /// 查询时包含关联数据
        /// </summary>
        /// <returns></returns>
        public override IQueryable<Role> Query()
        {
            return base.Query()
                .Include(r => r.OwnerTenant)
                .Include(r => r.Users).ThenInclude(r => r.From);
        }
    }
}
