namespace InfrastructurePlugins.BaseModule.Components.QueryBuilder
{
    /// <summary>
    /// 查询条件操作符
    /// </summary>
    public enum OpertionSymbol
    {
        None = 0,
        /// <summary>
        /// 等于
        /// </summary>
        Equals = 1,
        /// <summary>
        /// 不等于
        /// </summary>
        NotEquals = 2,
        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan = 3,
        /// <summary>
        ///  不大于
        /// </summary>
        NotGreaterThan = 4,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterThanOrEqueals = 5,
        /// <summary>
        /// 小于
        /// </summary>
        LessThan = 6,
        /// <summary>
        /// 不小于
        /// </summary>
        NotLessThan = 7,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessThanOrEqual = 8,
        /// <summary>
        /// 包含
        /// </summary>
        Like = 9,
        /// <summary>
        /// 不包含
        /// </summary>
        NotLike = 10,
        /// <summary>
        /// 以...开头
        /// </summary>
        StartsWith = 11,
        /// <summary>
        ///  不以...开头
        /// </summary>
        NotStartsWith = 12,
        /// <summary>
        /// /以...结尾
        /// </summary>
        EndsWith = 13,
        /// <summary>
        /// 不以...结尾
        /// </summary>
        NotEndsWith = 14,
        /// <summary>
        /// 值在集合内
        /// </summary>
        In = 15,
        /// <summary>
        /// 值不在集合内
        /// </summary>
        NotIn = 16,
        /// <summary>
        /// 介于 A...B之间
        /// </summary>
        Between = 17,
        /// <summary>
        /// 不介于 A...B之间
        /// </summary>
        NotBetween = 18,
        /// <summary>
        /// （支持：1,2,3 或 1-3；如果不符合前面规则，即认为模糊查询
        /// </summary>
        Fuzzy = 19,
        /// <summary>
        /// 
        /// </summary>
        NotFuzzy = 20,
        /// <summary>
        /// 正则
        /// </summary>
        RegExp = 21
    }
}
