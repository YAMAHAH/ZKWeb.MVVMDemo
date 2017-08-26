using System;
using System.Linq.Expressions;

namespace InfrastructurePlugins.BaseModule.Components.DtoToModelMap
{
    public interface IDtoToModelMapValueBase
    {
        LambdaExpression Expression { get; set; }
        Delegate ColumnFilterWrapper { get; set; }
    }
}
