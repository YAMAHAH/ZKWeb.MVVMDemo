using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfrastructurePlugins.BaseModule.Components.QueryBuilder
{
    public class ColumnQueryCondition
    {
        /// <summary>
        /// 自定义列过滤条件
        /// </summary>
        public bool IsCustomColumnFilter { get; set; }
        /// <summary>
        /// 子表达式
        /// </summary>
        public bool IsChildExpress { get; set; }

        /// <summary>
        /// 子查询
        /// </summary>
        public bool IsChildQuery { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public Type ProperyType { get; set; } = typeof(object);
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
        public List<ColumnQueryCondition> Childs { get; set; } = new List<ColumnQueryCondition>();
        /// <summary>
        /// 条件表达式树
        /// </summary>
        public Expression Expression { get; set; }

        /// <summary>
        /// 源条件表达式树
        /// </summary>
        public LambdaExpression SrcExpression { get; set; }
        /// <summary>
        /// 正则表达式
        /// </summary>
        public string RegExp { get; set; }
    }
}
