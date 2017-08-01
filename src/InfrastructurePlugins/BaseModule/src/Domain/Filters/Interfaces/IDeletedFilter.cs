namespace InfrastructurePlugins.BaseModule.Domain.Filters.Interfaces
{
    /// <summary>
    /// 根据删除状态过滤查询
    /// 字段没有删除状态时返回原查询
    /// </summary>
    public interface IDeletedFilter : IEntityQueryFilter
    {
    }
}
