using BusinessPlugins.OrganizationModule.Application.Dtos;
using FluentValidation;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Application.Validations
{
    /// <summary>
    /// 用户更改蜜码验证
    /// </summary>
    [ExportMany, SingletonReuse]
    public class UserChangePasswordValidation : AbstractValidator<UserChangePasswordInputDto>
    {
        public UserChangePasswordValidation()
        {
            RuleFor(c => c.OldPassword).NotEmpty().MaximumLength(int.MaxValue).MinimumLength(6);

            RuleFor(c => c.NewPassword).NotEmpty().MaximumLength(int.MaxValue).MinimumLength(6);

            RuleFor(c => c.ConfirmNewPassword).NotEmpty().MaximumLength(int.MaxValue).MinimumLength(6);
        }
    }
}
