using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Captcha.src.Application.Dtos
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
