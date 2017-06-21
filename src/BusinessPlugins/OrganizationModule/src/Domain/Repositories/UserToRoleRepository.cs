using InfrastructurePlugins.BaseModule.Domain.Repositories.Bases;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Repositories
{
    /// <summary>
    /// 用户到角色的仓储
    /// </summary>
    [ExportMany]
    public class UserToRoleRepository : ManyToManyRepositoryBase<User, Role, UserToRole>
    {
    }
}
