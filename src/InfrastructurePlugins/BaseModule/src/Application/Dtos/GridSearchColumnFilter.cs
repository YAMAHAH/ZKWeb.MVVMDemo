using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
    }
}
