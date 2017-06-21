using System.ComponentModel;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using BusinessPlugins.OrganizationModule.Application.Dtos;

namespace BusinessPlugins.OrganizationModule.Application.Dtos
{
    [Description("当前会话信息")]
    public class SessionInfoDto : IOutputDto
    {
        [Description("用户信息")]
        public UserOutputDto User { get; set; }
    }
}
