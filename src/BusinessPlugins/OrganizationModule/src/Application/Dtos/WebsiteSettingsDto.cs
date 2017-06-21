using System.ComponentModel;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Components.AutoMapper;
using BusinessPlugins.OrganizationModule.Components.GenericConfigs;

namespace BusinessPlugins.OrganizationModule.Application.Dtos
{
    [AutoMap(typeof(WebsiteSettings))]
    [Description("网站设置")]
    public class WebsiteSettingsDto : IInputDto, IOutputDto
    {
        [Description("网站名称")]
        public string WebsiteName { get; set; }
    }
}
