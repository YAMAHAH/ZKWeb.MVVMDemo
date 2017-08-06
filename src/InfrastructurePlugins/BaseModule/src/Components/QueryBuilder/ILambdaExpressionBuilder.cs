using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfrastructurePlugins.BaseModule.Components.QueryBuilder
{
    public interface ILambdaExpressionBuilder<T>
    {
        string ParameterName { get; set; }
        ParameterExpression[] Parameters { get; set; }
        ICollection<QueryCondition> QueryCondtions { get; set; }
        /// <summary>
        /// 获取某个查询条件结点的表达式生成对应的表达式树
        /// 结点的结果由子结点拼接而成
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        Expression<Func<T, bool>> GetLambdaExpression(Expression body);
        /// <summary>
        /// 生成lambda表达式树
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        Expression<Func<T, bool>> GenerateLambdaExpression(QueryCondition root);
        /// <summary>
        /// 解析字符串生成表达式树
        /// ParameterExpression x = Expression.Parameter(typeof(int), "x");
        /// ParameterExpression y = Expression.Parameter(typeof(int), "y");
        /// LambdaExpression e = DynamicExpressionParser.ParseLambda(new ParameterExpression[] { x, y }, typeof(double), "(x + y) * 2");
        /// (x, y) => Convert(((x + y) * 2))
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="resultType"></param>
        /// <param name="expression"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        LambdaExpression ParseLambda(ParameterExpression[] parameters, Type resultType, string expression, params object[] values);
        /// <summary>
        /// 解析字符串生成表达式树
        /// Dictionary<string, object> symbols = new Dictionary<string, object>();
        /// symbols.Add("x", x); symbols.Add("y", y);
        /// Expression body = DynamicExpressionParser.ParseLambda(null, "(x + y) * 2", symbols);
        /// LambdaExpression e1 = Expression.Lambda(body, new ParameterExpression[] { x, y });
        /// (x, y) => () => ((x + y) * 2)
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="expression"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        LambdaExpression ParseLambda(Type resultType, string expression, params object[] values);
    }
}
