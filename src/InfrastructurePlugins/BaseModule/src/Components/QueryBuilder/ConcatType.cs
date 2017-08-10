namespace InfrastructurePlugins.BaseModule.Components.QueryBuilder
{
    /// <summary>
    /// 条件连接符
    /// </summary>
    public enum ConcatType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 与
        /// </summary>
        AndAlso = 1,
        /// <summary>
        /// 或
        /// </summary>
        OrElse = 2,
        /// <summary>
        /// 非
        /// </summary>
        Not = 3
    }
}
