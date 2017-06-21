using InfrastructurePlugins.BaseModule.Domain.Entities.Bases;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    /// <summary>
    /// 用户到角色的多对多关联表
    /// </summary>
    [ExportMany]
    public class UserToRole : ManyToManyEntityBase<User, Role, UserToRole>
    {
    }
}
