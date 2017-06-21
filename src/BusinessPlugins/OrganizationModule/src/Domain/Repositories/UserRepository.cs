﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using InfrastructurePlugins.BaseModule.Domain.Repositories.Bases;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Repositories
{
    /// <summary>
    /// 用户的仓储
    /// </summary>
    [ExportMany, SingletonReuse]
    public class UserRepository : RepositoryBase<User, Guid>
    {
        /// <summary>
        /// 查询时包含关联数据
        /// </summary>
        /// <returns></returns>
        public override IQueryable<User> Query()
        {
            return base.Query()
                .Include(u => u.OwnerTenant)
                .Include(u => u.Employee)
                .Include(u => u.Roles).ThenInclude(r => r.To);
        }
    }
}
