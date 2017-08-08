using InfrastructurePlugins.BaseModule.Components.CORSExposeHeaders.Interfaces;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Application.CorsExposeHeader
{
    /// <summary>
    /// ExposeHeader提供者
    /// </summary>
    [ExportMany]
    public class CorsExposeHeaderProvider : ICORSExposeHeader
    {
        public string ExposeHeader => "x-set-zkweb-sessionid";
    }
}
