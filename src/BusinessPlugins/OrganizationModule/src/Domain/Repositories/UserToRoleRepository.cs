using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.MVVMPlugins.OrganizationModule.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.OrganizationModule.src.Domain.Repositories
{
    /// <summary>
    /// 用户到角色的仓储
    /// </summary>
    [ExportMany]
    public class UserToRoleRepository : ManyToManyRepositoryBase<User, Role, UserToRole>
    {
    }
}
