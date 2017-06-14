using BusinessPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using System.Reflection;

namespace BusinessPlugins.MVVM.Common.Organization.Domain.Entities.TypeTraits
{

    /// <summary>
    /// 判断类型是否有创建的用户
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public static class HaveCreatorUpdatorTrait<TEntity>
    {
        /// <summary>
        /// 判断结果
        /// </summary>
        public readonly static bool HaveCreatorUpdator =
            typeof(IHaveCreatorUpdator).GetTypeInfo().IsAssignableFrom(typeof(TEntity));
    }
}
