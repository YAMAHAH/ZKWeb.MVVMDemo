using System.ComponentModel;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Captcha.src.Application.Dtos
{
    [Description("握手传出信息")]
    public class HandshakeRequestOutput : IOutputDto
    {
        [Description("测试数据")]
        public string TestData { get; set; }
        [Description("处理结果")]
        public string ProcessResult { get; set; }
        public HandshakeRequestOutput()
        {

        }
    }
}
