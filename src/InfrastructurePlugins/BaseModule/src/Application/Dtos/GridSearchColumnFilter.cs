using InfrastructurePlugins.BaseModule.Components.QueryBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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

        [Description("子表达式")]
        public bool IsChildExpress { get; set; }

        [Description("属性类型")]
        public Type ProperyType { get; set; }

        [Description("正则表达式")]
        public string RegExp { get; set; }

        [Description("属性名称")]
        public string PropertyName { get; set; }

        [Description("操作符")]
        public OpertionSymbol OpertionSymbol { get; set; }

        [Description("值1")]
        public object Value1 { get; set; }

        [Description("值2")]
        public object Value2 { get; set; }

        [Description("逻辑连接符")]
        public ConcatType Concat { get; set; }
        [Description("子表达式查询条件")]
        public List<GridSearchColumnFilter> Childs { get; set; } = new List<GridSearchColumnFilter>();
    }
}
