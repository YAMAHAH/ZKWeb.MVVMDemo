using BusinessPlugins.OrganizationModule.Components.GenericConfigs.Attributes;

namespace BusinessPlugins.OrganizationModule.Components.GenericConfigs
{
    /// <summary>
    /// 网站设置
    /// </summary>
    [GenericConfig("Common.Organization.WebsiteSettings", CacheTime = 15)]
    public class WebsiteSettings
    {
        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebsiteName { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public WebsiteSettings()
        {
            WebsiteName = "ZKWeb MVVM Demo";
        }
    }
}
