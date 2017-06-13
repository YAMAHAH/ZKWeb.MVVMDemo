using System.ComponentModel;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.AutoMapper;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.GenericConfigs;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos
{
    [AutoMap(typeof(WebsiteSettings))]
    [Description("网站设置")]
    public class WebsiteSettingsDto : IInputDto, IOutputDto
    {
        [Description("网站名称")]
        public string WebsiteName { get; set; }
    }
}
