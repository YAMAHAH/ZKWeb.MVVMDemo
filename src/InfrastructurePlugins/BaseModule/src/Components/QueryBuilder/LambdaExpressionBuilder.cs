using InfrastructurePlugins.BaseModule.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace InfrastructurePlugins.BaseModule.Components.QueryBuilder
{
    public class LambdaExpressionBuilder<T> : ILambdaExpressionBuilder<T>
    {
        public LambdaExpressionBuilder()
        {
            ParameterName = "p";
        }
        /// <summary>
        /// 暂时不用
        /// </summary>
        /// <param name="paraTypes"></param>
        public LambdaExpressionBuilder(params Type[] paraTypes)
        {
            IList<ParameterExpression> parameters = new List<ParameterExpression>();
            for (int i = 0; i < paraTypes.Length; i++)
            {
                parameters.Add(Expression.Parameter(paraTypes[i].GetType(), paraTypes[i].GetType().Name.ToLower() + i.ToString()));
            }
        }
        public LambdaExpressionBuilder(string paraName)
        {
            if (string.IsNullOrEmpty(paraName))
            {
                ParameterName = "p";
            }
            else
            {
                ParameterName = paraName;
            }
        }
        public string ParameterName { get; set; }
        public ParameterExpression[] Parameters { get; set; }
        public ICollection<ColumnQueryCondition> QueryCondtions { get; set; }

        public Expression<Func<T, bool>> GenerateLambdaExpression(ColumnQueryCondition root)
        {
            RecursionGenerateExpression(root);
            return GetLambdaExpression(root.Expression);
        }

        /// <summary>
        /// 获取某个结点的表达式树
        /// </summary>
        /// <typeparam name="TFunc"></typeparam>
        /// <param name="body"></param>
        /// <returns></returns>
        public Expression<Func<T, bool>> GetLambdaExpression(Expression body)
        {
            return Expression.Lambda<Func<T, bool>>(body, this.Parameters);
        }

        public LambdaExpression ParseLambda(ParameterExpression[] parameters, Type resultType, string expression, params object[] values)
        {
            return DynamicExpressionParser.ParseLambda(parameters, resultType, expression, values);
        }
        public LambdaExpression ParseLambda(Type resultType, string expression, params object[] values)
        {
            return DynamicExpressionParser.ParseLambda(resultType, expression, values);
        }

        /// <summary>
        /// 递归生成表达式
        /// </summary>
        /// <param name="c"></param>
        private void RecursionGenerateExpression(ColumnQueryCondition c)
        {
            //生成相应的表达式树
            if (!c.IsChildExpress) GenerateExpression(c);
            foreach (var t in c.Childs)
            {
                RecursionGenerateExpression(t);
                //拼接表达式树
                ConcatExpression(c, t, c.Childs.First() == t);
            }
        }

        /// <summary>
        /// 生成单个条件的表达式
        /// </summary>
        /// <param name="qc"></param>
        private void GenerateExpression(ColumnQueryCondition qc)
        {
            //(e)=>Test(e,qc)

            Expression expr = null;

            //根据操作符生成相应的表达式
            switch (qc.OpertionSymbol)
            {
                case OpertionSymbol.Equals:
                    expr = this.Equals(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.NotEquals:
                    expr = this.NotEquals(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.GreaterThan:
                    expr = this.GreaterThan(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.NotGreaterThan:
                    expr = this.NotGreaterThan(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.GreaterThanOrEqueals:
                    expr = this.GreaterThanOrEquals(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.LessThan:
                    expr = this.LessThan(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.NotLessThan:
                    expr = this.LessThan(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.LessThanOrEqual:
                    expr = this.LessThanOrEqual(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.Like:
                    expr = this.Like<T>(qc.PropertyName, qc.Value1 as string, qc.Concat);
                    break;
                case OpertionSymbol.NotLike:
                    expr = this.NotLike<T>(qc.PropertyName, qc.Value1 as string, qc.Concat);
                    break;
                case OpertionSymbol.StartsWith:
                    expr = this.StartsWith(qc.PropertyName, ConverterHelper.ConvertTo<string>(qc.Value1), qc.Concat);
                    break;
                case OpertionSymbol.NotStartsWith:
                    expr = this.StartsWith(qc.PropertyName, ConverterHelper.ConvertTo<string>(qc.Value1), qc.Concat);
                    break;
                case OpertionSymbol.EndsWith:
                    expr = this.EndsWith(qc.PropertyName, qc.Value1 as string, qc.Concat);
                    break;
                case OpertionSymbol.NotEndsWith:
                    expr = this.NotEndsWith(qc.PropertyName, qc.Value1 as string, qc.Concat);
                    break;
                case OpertionSymbol.In:
                    expr = this.In(qc.PropertyName, qc.Concat, ConverterHelper.ChangeType(qc.Value1, typeof(object[])));
                    break;
                case OpertionSymbol.NotIn:
                    expr = this.NotIn(qc.PropertyName, qc.Concat, ConverterHelper.ChangeType(qc.Value1, typeof(object[])));
                    break;
                case OpertionSymbol.Between:
                    expr = this.Between(qc.PropertyName, qc.Value1, qc.Value2, qc.Concat);
                    break;
                case OpertionSymbol.NotBetween:
                    expr = this.NotBetween(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), ConverterHelper.ChangeType(qc.Value2, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.Fuzzy:
                    expr = this.Fuzzy(qc.PropertyName, ConverterHelper.ConvertTo<string>(qc.Value1), qc.Concat);
                    break;
                case OpertionSymbol.NotFuzzy:
                    expr = this.NotFuzzy(qc.PropertyName, ConverterHelper.ConvertTo<string>(qc.Value1), qc.Concat);
                    break;
                default:
                    break;
            }

            qc.Expression = expr;
        }
        /// <summary>
        /// 接接表达式树
        /// </summary>
        /// <param name="p"></param>
        /// <param name="c"></param>
        /// <param name="first"></param>
        private void ConcatExpression(ColumnQueryCondition p, ColumnQueryCondition c, bool first)
        {
            if (first)
            {
                p.Expression = c.Expression;
            }
            else if (c.Concat == ConcatType.AndAlso || c.Concat == ConcatType.None)
            {
                p.Expression = Expression.AndAlso(p.Expression, c.Expression);
            }
            else if (c.Concat == ConcatType.OrElse)
            {
                p.Expression = Expression.OrElse(p.Expression, c.Expression);
            }
            else if (c.Concat == ConcatType.Not)
            {
                p.Expression = Expression.Not(p.Expression);
            }
        }
    }
}
