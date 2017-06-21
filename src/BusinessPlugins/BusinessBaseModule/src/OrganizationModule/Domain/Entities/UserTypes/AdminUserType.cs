using BusinessPlugins.OrganizationModule.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities.UserTypes
{
    /// <summary>
    /// 用户类型: 管理员
    /// </summary>
    [ExportMany]
    public class AdminUserType : IAmAdmin
    {
        public const string ConstType = "Admin";
        public string Type { get { return ConstType; } }
    }
}
