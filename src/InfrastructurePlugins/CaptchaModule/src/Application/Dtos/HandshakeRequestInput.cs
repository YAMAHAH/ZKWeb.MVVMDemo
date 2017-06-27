using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using System.ComponentModel;

namespace InfrastructurePlugins.CaptchaModule.Application.Dtos
{
    /// <summary>
    /// 服务器握手请求
    /// </summary>
    [Description("握手请求")]
    [ExportSingle(ExportType =typeof(IHandshakeRequestInput))]
    public class HandshakeRequestInput : IInputDto, IHandshakeRequestInput
    {
        [Description("数据加密私钥")]
        public string SecretKey { get; set; }
        [Description("客户RSA公私")]
        public string PublicKey { get; set; }
    }
}
