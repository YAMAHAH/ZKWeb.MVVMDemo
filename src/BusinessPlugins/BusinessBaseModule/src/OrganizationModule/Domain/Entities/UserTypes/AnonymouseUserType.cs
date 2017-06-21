using BusinessPlugins.OrganizationModule.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities.UserTypes
{
    /// <summary>
    /// 匿名用户
    /// </summary>
    [ExportMany]
    public class AnonymouseUserType : IAmAnonymouseUser
    {
        public string Type { get { return null; } }
    }
}
