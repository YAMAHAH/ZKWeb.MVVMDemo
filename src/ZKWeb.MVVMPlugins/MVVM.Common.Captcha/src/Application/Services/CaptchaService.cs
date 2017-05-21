using SimpleEasy.Core.lib;
using SimpleEasy.Core.lib.Utils;
using System;
using System.ComponentModel;
using System.DrawingCore.Imaging;
using System.IO;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Captcha.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Captcha.src.Managers;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Captcha.src.Application.Services
{
    /// <summary>
    /// 验证码服务
    /// </summary>
    [ExportMany, SingletonReuse, Description("验证码服务")]
    public class CaptchaService : ApplicationServiceBase
    {
        private CaptchaManager _captchaManager;

        public CaptchaService(CaptchaManager captchaManager)
        {
            _captchaManager = captchaManager;
        }

        /// <summary>
        /// 获取验证码图片的Base64
        /// </summary>
        /// <param name="key">使用的键名</param>
        /// <returns></returns>
        [Description("获取验证码图片的Base64")]
        public string GetCaptchaImageBase64(string key)
        {
            using (var stream = new MemoryStream())
            using (var image = _captchaManager.Generate(key))
            {
                image.Save(stream, ImageFormat.Png);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        /// <summary>
        /// 客户端与服务端第一次连接的握手请求
        /// </summary>
        /// <param name="handshakeRequest">请求对象</param>
        /// <returns></returns>
        [Description("客户端与服务端第一次连接握手请求")]
        public HandshakeRequestOutput HandshakeRequest(HandshakeRequestInput handshakeRequest)
        {
            var MakeSessionAliveAtLeast = 30;
            //读取客户端密钥
            var aesKey = handshakeRequest.SecretKey;
            //使用RSA解密
            var aesSecretKey = RSAUtils.decryptData(aesKey, RSAUtils._privateKey, "UTF-8");
            //读取客户端公钥
            var publicKey = AESUtils.DecryptToUtf8String(aesSecretKey, handshakeRequest.PublicKey).Result;

            var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
            var session = sessionManager.GetSession();
            session[AppConts.ClientPublicKey] = publicKey;
            session[AppConts.ClientSecretKey] = aesSecretKey;
            ClientData clientData = new ClientData() { PublickKey = publicKey, SecretKey = aesSecretKey };
            ClientDataManager.SetData(session.Id.ToString(), clientData);
            session.SetExpiresAtLeast(TimeSpan.FromMinutes(MakeSessionAliveAtLeast));
            sessionManager.SaveSession();
            //生成使用密钥加密的测试数据
            var testData = "Hello!";
            var getSecretKey = sessionManager.GetSession()[AppConts.ClientSecretKey] as string;
            var result = AESUtils.EncryptToBase64String(getSecretKey, testData);
            return new HandshakeRequestOutput() { ProcessResult = result, TestData = testData };
        }
    }
}
