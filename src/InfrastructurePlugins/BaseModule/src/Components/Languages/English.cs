using ZKWeb.Localize;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Components.Languages
{
    /// <summary>
    /// 英语
    /// </summary>
    [ExportMany]
    public class English : ILanguage
    {
        public string Name { get { return "en-US"; } }
    }
}
