using System.Collections.Generic;
using System.Reflection;
using InfrastructurePlugins.BaseModule.Application.Services.Structs;

namespace InfrastructurePlugins.BaseModule.Application.Services.Interfaces
{
    /// <summary>
    /// 应用服务的接口
    /// </summary>
    public interface IApplicationService
    {
        /// <summary>
        /// 获取Api函数列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<ApplicationServiceApiMethodInfo> GetApiMethods();
    }
}
