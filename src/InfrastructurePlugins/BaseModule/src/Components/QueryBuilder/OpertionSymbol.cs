﻿namespace InfrastructurePlugins.BaseModule.Components.QueryBuilder
{
    public enum OpertionSymbol
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equals = 0,
        NotEquals = 1, // 不等于
        GreaterThan = 2, // 大于
        NotGreaterThan = 3, // 不大于
        GreaterThanOrEqueals = 4, // 大于等于
        LessThan = 5, //小于
        NotLessThan = 6, //不小于
        LessThanOrEqual = 7, //小于等于
        Like = 8, //包含
        NotLike = 9, //不包含
        StartsWith = 10, // 以...开头
        NotStartsWith = 11, // 不以...开头
        EndsWith = 12, //以...结尾
        NotEndsWith = 13, //不以...结尾
        In = 14, // 值在集合内
        NotIn = 15, // 值不在集合内
        Between = 16, //介于 A...B之间
        NotBetween = 17, //不介于 A...B之间
        Fuzzy = 18, //（支持：1,2,3 或 1-3；如果不符合前面规则，即认为模糊查询
        NotFuzzy = 19
    }
}