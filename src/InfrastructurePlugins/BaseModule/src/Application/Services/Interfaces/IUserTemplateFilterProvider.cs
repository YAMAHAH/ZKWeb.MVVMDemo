using System;

namespace InfrastructurePlugins.BaseModule.Application.Services.Interfaces
{
    /// <summary>
    /// 获取用户模板的不可用的过滤器,包括查询/操作过滤器
    /// </summary>
    public interface IUserTemplateFilterProvider
    {
        /// <summary>
        /// 获取当前会话模板不可用过滤器名称
        /// </summary>
        /// <param name="tempId">模板(应用服务类)</param>
        /// <returns>应用服务类对应的FullName</returns>
        string[] DisabledFilter();
        /// <summary>
        /// 获取用户模板可用的过滤器名称
        /// </summary>
        /// <returns></returns>
        string[] AvailableFilter();
    }
}
