using Newtonsoft.Json;
using SimpleEasy.Core.lib.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Captcha.src.Managers;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Services
{
    /// <summary>
    /// 用户登录服务
    /// </summary>
    [ExportMany, SingletonReuse, Description("用户登录服务")]
    public class UserLoginService : ApplicationServiceBase
    {
        private UserManager _userManager;
        private AdminManager _adminManager;
        private CaptchaManager _captchaManager;

        public UserLoginService(
            UserManager userManager,
            AdminManager adminManager,
            CaptchaManager captchaManager)
        {
            _userManager = userManager;
            _adminManager = adminManager;
            _captchaManager = captchaManager;
        }

        /// <summary>
        /// 登录用户
        /// </summary>
        /// <returns></returns>
        [Description("登录用户")]
        public virtual ActionResponseDto LoginUser(UserLoginRequestDto request)
        {
            // 检查验证码
            if (!_captchaManager.Check("UserLogin", request.Captcha))
            {
                throw new BadRequestException("Incorrect captcha");
            }
            // 登录用户
            _userManager.Login(
                request.Tenant,
                request.Username,
                request.Password,
                request.RememberLogin ?? true);
            return ActionResponseDto.CreateSuccess();
        }

        /// <summary>
        /// 登录管理员
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Description("登录管理员")]
        public virtual ActionResponseDto LoginAdmin(UserLoginRequestDto request)
        {
            // 检查验证码
            if (!_captchaManager.Check("AdminLogin", request.Captcha))
            {
                throw new BadRequestException("Incorrect captcha");
            }
            ////获取key
            // var aesKey = request.SecretKey;
            ////使用RSA解密
            //var aesSecretKey = RSAUtils.decryptData(aesKey, RSAUtils._privateKey, "UTF-8");
            ////解密publicKey
            //var publicKey = AESUtils.DecryptToUtf8String(aesSecretKey, request.PublicKey).Result;

            //var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
            //var session = sessionManager.GetSession();
            //session["clientPublicKey"] = publicKey;
            //session["clientSecretKey"] = aesSecretKey;           
            //sessionManager.SaveSession();
            // 登录用户
            _adminManager.Login(
                request.Tenant,
                request.Username,
                request.Password,
                request.RememberLogin ?? true);
            return ActionResponseDto.CreateSuccess();
        }
    }
}
