using InfrastructurePlugins.BaseModule.Components.QueryBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace InfrastructurePlugins.BaseModule.Application.Dtos
{
    [Description("列过滤信息")]
    public class GridSearchColumnFilter
    {
        [Description("列名")]
        public string Column { get; set; }
        [Description("匹配模式")]
        public GridSearchColumnFilterMatchMode MatchMode { get; set; }
        [Description("过滤值")]
        public object Value { get; set; }

        /// <summary>
        /// 子表达式
        /// </summary>
        public bool IsChildExpress { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public Type ProperyType { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 操作符
        /// </summary>
        public OpertionSymbol OpertionSymbol { get; set; }
        /// <summary>
        /// 值1
        /// </summary>
        public object Value1 { get; set; }
        /// <summary>
        /// 值2
        /// </summary>
        public object Value2 { get; set; }
        /// <summary>
        /// 逻辑连接符
        /// </summary>
        public ConcatType Concat { get; set; }
        /// <summary>
        /// 子表达式查询条件
        /// </summary>
        public List<GridSearchColumnFilter> Childs { get; set; } = new List<GridSearchColumnFilter>();
        /// <summary>
        /// 条件表达式树
        /// </summary>
        public Expression Expression { get; set; }
    }
}
