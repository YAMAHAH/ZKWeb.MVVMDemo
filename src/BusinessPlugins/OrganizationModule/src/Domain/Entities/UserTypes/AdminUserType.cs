using ZKWeb.MVVMPlugins.OrganizationModule.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.OrganizationModulesrc.Domain.Entities.UserTypes
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
