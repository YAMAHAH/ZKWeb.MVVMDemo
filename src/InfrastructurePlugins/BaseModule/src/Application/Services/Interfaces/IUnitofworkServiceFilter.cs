using System;

namespace InfrastructurePlugins.BaseModule.Application.Services.Interfaces
{
    /// <summary>
    /// 获取用户模板的过滤器,包括查询/操作过滤器
    /// </summary>
    public interface IUnitofworkServiceFilter
    {
        /// <summary>
        /// 获取当前会话模板过滤器名称
        /// </summary>
        /// <param name="tempId">模板(应用服务类)</param>
        /// <returns>应用服务类对应的FullName</returns>
        string[] Filters(Guid tempId);
    }
}
