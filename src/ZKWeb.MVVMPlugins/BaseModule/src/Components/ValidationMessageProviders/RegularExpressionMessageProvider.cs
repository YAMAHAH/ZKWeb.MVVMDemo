using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using InfrastructurePlugins.BaseModule.Components.ValidationMessageProviders.Interfaces;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Components.ValidationMessageProviders
{
    /// <summary>
    /// 提供验证正则表达式的错误信息
    /// </summary>
    [ExportMany]
    public class RegularExpressionMessageProvider : IValidationMessageProvider<RegularExpressionAttribute>
    {
        public string FormatErrorMessage(RegularExpressionAttribute attribute, string name)
        {
            return new T("Format of {0} is incorrect", name);
        }
    }
}
