using System;
using System.Linq.Expressions;

namespace InfrastructurePlugins.BaseModule.Application.Services.Interfaces
{
    /// <summary>
    /// 获取用户预设的模板过滤条件
    /// </summary>
    public interface IUserPresetTemplateFilterProvider
    {
        Expression<Func<TEntity, bool>> GetUserPresetFilter<TEntity>();
    }
}
