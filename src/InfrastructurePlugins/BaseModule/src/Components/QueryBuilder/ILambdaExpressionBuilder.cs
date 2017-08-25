using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfrastructurePlugins.BaseModule.Components.QueryBuilder
{
    public interface ILambdaExpressionBuilderBase
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        string ParameterName { get; set; }
        /// <summary>
        /// 生成器的参数表达式
        /// </summary>
        ParameterExpression[] Parameters { get; set; }
        /// <summary>
        /// 生成对象和基本类型的表达式
        /// </summary>
        /// <param name="c"></param>
        void GenerateExpression(ColumnQueryCondition c);
        /// <summary>
        /// 根据属性名称生成表达式
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        LambdaExpression CreatePropertyExpression(string propertyName);
        Expression GetMemberExpression(LambdaExpression property);
        /// <summary>
        /// 生成集合任意一行满足的表达式
        /// </summary>
        /// <param name="c"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Expression Any(ColumnQueryCondition c, Expression predicate);
        /// <summary>
        /// 生成集合所有行都满足的表达式
        /// </summary>
        /// <param name="c"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Expression All(ColumnQueryCondition c, Expression predicate);
        /// <summary>
        /// 生成集合计数表达式
        /// </summary>
        /// <param name="c"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Expression Count(ColumnQueryCondition c, Expression predicate);
        /// <summary>
        /// 生成集合最大值表达式
        /// </summary>
        /// <param name="c"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Expression Max(ColumnQueryCondition c, Expression predicate);
        /// <summary>
        /// 生成集合最小值表达式
        /// </summary>
        /// <param name="c"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Expression Min(ColumnQueryCondition c, Expression predicate);
        /// <summary>
        /// 生成集合平均值表达式
        /// </summary>
        /// <param name="c"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Expression Average(ColumnQueryCondition c, Expression predicate);
        /// <summary>
        /// 生成集合汇总表达式
        /// </summary>
        /// <param name="c"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Expression Sum(ColumnQueryCondition c, Expression predicate);
        /// <summary>
        /// 生成集合对象的表达式
        /// </summary>
        /// <param name="filterRequest"></param>
        /// <param name="predicateExpr"></param>
        /// <returns></returns>
        Expression GenerateListExpression(ColumnQueryCondition filterRequest, Expression predicateExpr);
    }

    public interface ILambdaExpressionBuilder<T> : ILambdaExpressionBuilderBase
    {

        ICollection<ColumnQueryCondition> QueryCondtions { get; set; }
        /// <summary>
        /// 获取某个查询条件结点的表达式生成对应的表达式树
        /// 结点的结果由子结点拼接而成
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        Expression<Func<T, bool>> MakeLambdaExpression(Expression body);
        /// <summary>
        /// 生成lambda表达式树
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        Expression<Func<T, bool>> GenerateLambdaExpression(ColumnQueryCondition root);
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
