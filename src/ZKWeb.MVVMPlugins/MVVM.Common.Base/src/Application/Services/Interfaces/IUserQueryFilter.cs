using System;
using System.Collections.Generic;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Interfaces
{
    /// <summary>
    /// 获取用户查询过滤器
    /// </summary>
    public interface IUserQueryFilter
    {
        /// <summary>
        /// 用户所拥有的查询过滤器,规则过滤器，规则过滤器优先于查询过滤器
        /// </summary>
        /// <returns>按优先级排序的集合</returns>
        IList<IEntityQueryFilter> UserQueryFilters(string serviceId);
    }
}
