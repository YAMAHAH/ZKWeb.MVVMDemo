﻿using System.Reflection;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities.Interfaces;

namespace InfrastructurePlugins.MultiTenantModule.Domain.Entities.TypeTraits
{
    /// <summary>
    /// 判断类型是否有所属的租户
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public static class OwnerTenantTypeTrait<TEntity>
    {
        /// <summary>
        /// 判断结果
        /// </summary>
        public readonly static bool HaveOwnerTenant =
            typeof(IHaveOwnerTenant).GetTypeInfo().IsAssignableFrom(typeof(TEntity));
    }
}
