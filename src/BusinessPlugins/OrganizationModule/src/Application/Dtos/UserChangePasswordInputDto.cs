using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using InfrastructurePlugins.BaseModule.Application.Dtos;

namespace BusinessPlugins.OrganizationModule.Application.Dtos
{
    [Description("修改密码传入信息")]
    public class UserChangePasswordInputDto : IInputDto
    {
        [Description("原密码"), Required, StringLength(int.MaxValue, MinimumLength = 6)]
        public string OldPassword { get; set; }
        [Description("新密码"), Required, StringLength(int.MaxValue, MinimumLength = 6)]
        public string NewPassword { get; set; }
        [Description("确认信密码"), Required, StringLength(int.MaxValue, MinimumLength = 6)]
        public string ConfirmNewPassword { get; set; }

    }
}
