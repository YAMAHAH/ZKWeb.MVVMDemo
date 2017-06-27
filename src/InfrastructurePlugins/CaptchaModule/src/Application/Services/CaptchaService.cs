using CoreLibModule.Utils;
using System;
using System.ComponentModel;
using System.DrawingCore.Imaging;
using System.IO;
using InfrastructurePlugins.BaseModule.Application.Services.Bases;
using InfrastructurePlugins.BaseModule.Components.Global;
using InfrastructurePlugins.CaptchaModule.Application.Dtos;
using InfrastructurePlugins.CaptchaModule.Managers;
using InfrastructurePlugins.SessionStateModule.Domain.Extensions;
using InfrastructurePlugins.SessionStateModule.Domain.Services;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.CaptchaModule.Application.Services
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
            IClientDataManager clientDataMan = ZKWeb.Application.Ioc.Resolve<IClientDataManager>();
            var MakeSessionAliveAtLeast = 30;
            //读取客户端密钥
            var aesKey = handshakeRequest.SecretKey;
            //使用RSA解密
            var aesSecretKey = RSAUtils.decryptData(aesKey, RSAUtils._privateKey, "UTF-8");
            //读取客户端公钥
            var publicKey = AESUtils.DecryptToUtf8String(aesSecretKey, handshakeRequest.PublicKey).Result;

            ClientData clientData = new ClientData() { PublickKey = publicKey, SecretKey = aesSecretKey };
            var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
            var session = sessionManager.GetSession();
            clientDataMan.SetData(session.Id.ToString(), clientData);
            session[AppConsts.ClientDataKey] = clientData;
            session.SetExpiresAtLeast(TimeSpan.FromMinutes(MakeSessionAliveAtLeast));
            sessionManager.SaveSession();

            //生成使用密钥加密的测试数据
            var testData = "Hello!";
            var secretKey = clientData.SecretKey;
            var result = AESUtils.EncryptToBase64String(secretKey, testData);
            return new HandshakeRequestOutput() { ProcessResult = result, TestData = testData };
        }
    }
}
