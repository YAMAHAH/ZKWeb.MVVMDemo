using AutoMapper;
using System.ComponentModel;
using InfrastructurePlugins.BaseModule.Application.Services.Bases;
using BusinessPlugins.OrganizationModule.Application.Dtos;
using BusinessPlugins.OrganizationModule.Domain.Extensions;
using InfrastructurePlugins.SessionStateModule.Domain.Services;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Application.Services
{
    /// <summary>
    /// 会话服务
    /// 用于获取当前登录的用户信息
    /// </summary>
    [ExportMany, SingletonReuse, Description("会话服务")]
    public class SessionService : ApplicationServiceBase
    {
        private SessionManager _sessionManager;

        public SessionService(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        /// <summary>
        /// 获取当前的会话信息
        /// </summary>
        /// <returns></returns>
        [Description("获取当前的会话信息")]
        public virtual SessionInfoDto GetSessionInfo()
        {
            var session = _sessionManager.GetSession();
            var user = session.GetUser();
            var result = new SessionInfoDto();
            if (user != null)
            {
                result.User = Mapper.Map<UserOutputDto>(user);
            }
            return result;
        }
    }
}
