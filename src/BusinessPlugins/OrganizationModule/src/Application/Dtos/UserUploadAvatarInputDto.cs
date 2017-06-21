using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using ZKWebStandard.Web;

namespace BusinessPlugins.OrganizationModule.Application.Dtos
{
    [Description("修改头像传入信息")]
    public class UserUploadAvatarInputDto : IInputDto
    {
        [Description("头像文件"), Required, CheckFile]
        public IHttpPostedFile Avatar { get; set; }
    }
}
