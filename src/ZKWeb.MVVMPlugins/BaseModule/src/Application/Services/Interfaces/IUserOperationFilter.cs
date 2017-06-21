using System;
using System.Collections.Generic;
using InfrastructurePlugins.BaseModule.Domain.Filters.Interfaces;

namespace InfrastructurePlugins.BaseModule.Application.Services.Interfaces
{
    /// <summary>
    /// 获取用户操作过滤器
    /// </summary>
    public interface IUserOperationFilter
    {
        /// <summary>
        /// 用户所拥有的操作过滤器,规则过滤器，规则过滤器优先于操作过滤器
        /// </summary>
        /// <returns>按优先级排序的集合</returns>
        IList<IEntityOperationFilter> UserOperationFilters(string serviceId);
    }
}
