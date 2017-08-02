﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfrastructurePlugins.BaseModule.Components.QueryBuilder
{
    public class QueryCondition
    {
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
        public List<QueryCondition> Childs { get; set; } = new List<QueryCondition>();
        /// <summary>
        /// 条件表达式树
        /// </summary>
        public Expression Expression { get; set; }
    }
}