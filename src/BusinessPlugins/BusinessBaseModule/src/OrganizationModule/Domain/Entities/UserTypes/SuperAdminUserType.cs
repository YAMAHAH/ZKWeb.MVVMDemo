using BusinessPlugins.OrganizationModule.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities.UserTypes
{
    /// <summary>
    /// 用户类型: 超级管理员
    /// </summary>
    [ExportMany]
    public class SuperAdminUserType : IAmSuperAdmin
    {
        public const string ConstType = "SuperAdmin";
        public string Type { get { return ConstType; } }
    }
}
