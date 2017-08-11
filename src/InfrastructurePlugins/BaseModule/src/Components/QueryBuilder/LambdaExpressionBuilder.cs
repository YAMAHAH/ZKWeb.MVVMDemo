using InfrastructurePlugins.BaseModule.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace InfrastructurePlugins.BaseModule.Components.QueryBuilder
{
    public class LambdaExpressionBuilder<T> : ILambdaExpressionBuilder<T>
    {
        public LambdaExpressionBuilder()
        {
            ParameterName = "e";
            CreateParaExpr(ParameterName);
        }

        private void CreateParaExpr(string paraName = "e")
        {
            ParameterExpression paraExpr = Expression.Parameter(typeof(T), paraName);
            Parameters.Append(paraExpr);
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
                ParameterName = "e";
            }
            else
            {
                ParameterName = paraName;
            }
            CreateParaExpr(ParameterName);
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
            Expression expr = null;

            //根据操作符生成相应的表达式
            switch (qc.OpertionSymbol)
            {
                case OpertionSymbol.Equals:
                    expr = qc.SrcExpression == null ? this.Equals(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat) :
                    this.Equals(qc.SrcExpression, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.NotEquals:
                    expr = qc.SrcExpression == null ? this.NotEquals(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat) :
                       this.NotEquals(qc.SrcExpression, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.GreaterThan:
                    expr = qc.SrcExpression == null ? this.GreaterThan(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat) :
                        this.GreaterThan(qc.SrcExpression, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.NotGreaterThan:
                    expr = qc.SrcExpression == null ? this.NotGreaterThan(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat) :
                        this.NotGreaterThan(qc.SrcExpression, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.GreaterThanOrEqueals:
                    expr = qc.SrcExpression == null ? this.GreaterThanOrEquals(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat) :
                        this.GreaterThanOrEquals(qc.SrcExpression, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.LessThan:
                    expr = qc.SrcExpression == null ? this.LessThan(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat) :
                        this.LessThan(qc.SrcExpression, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.NotLessThan:
                    expr = qc.SrcExpression == null ? this.LessThan(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat) :
                         this.LessThan(qc.SrcExpression, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.LessThanOrEqual:
                    expr = qc.SrcExpression == null ? this.LessThanOrEqual(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat) :
                        this.LessThanOrEqual(qc.SrcExpression, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.Like:
                    expr = qc.SrcExpression == null ? this.Like<T>(qc.PropertyName, qc.Value1 as string, qc.Concat) :
                        this.Like<T>(qc.SrcExpression, qc.Value1 as string, qc.Concat);
                    break;
                case OpertionSymbol.NotLike:
                    expr = qc.SrcExpression == null ? this.NotLike<T>(qc.PropertyName, qc.Value1 as string, qc.Concat) :
                        this.NotLike<T>(qc.SrcExpression, qc.Value1 as string, qc.Concat);
                    break;
                case OpertionSymbol.StartsWith:
                    expr = qc.SrcExpression == null ? this.StartsWith(qc.PropertyName, ConverterHelper.ConvertTo<string>(qc.Value1), qc.Concat) :
                        this.StartsWith(qc.SrcExpression, ConverterHelper.ConvertTo<string>(qc.Value1), qc.Concat);
                    break;
                case OpertionSymbol.NotStartsWith:
                    expr = qc.SrcExpression == null ? this.StartsWith(qc.PropertyName, ConverterHelper.ConvertTo<string>(qc.Value1), qc.Concat) :
                        this.StartsWith(qc.SrcExpression, ConverterHelper.ConvertTo<string>(qc.Value1), qc.Concat);
                    break;
                case OpertionSymbol.EndsWith:
                    expr = qc.SrcExpression == null ? this.EndsWith(qc.PropertyName, qc.Value1 as string, qc.Concat) :
                        this.EndsWith(qc.SrcExpression, qc.Value1 as string, qc.Concat);
                    break;
                case OpertionSymbol.NotEndsWith:
                    expr = qc.SrcExpression == null ? this.NotEndsWith(qc.PropertyName, qc.Value1 as string, qc.Concat) :
                        this.NotEndsWith(qc.SrcExpression, qc.Value1 as string, qc.Concat);
                    break;
                case OpertionSymbol.In:
                    expr = qc.SrcExpression == null ? this.In(qc.PropertyName, qc.Concat, ConverterHelper.ChangeType(qc.Value1, typeof(object[]))) :
                        this.In(qc.SrcExpression, qc.Concat, ConverterHelper.ChangeType(qc.Value1, typeof(object[])));
                    break;
                case OpertionSymbol.NotIn:
                    expr = qc.SrcExpression == null ? this.NotIn(qc.PropertyName, qc.Concat, ConverterHelper.ChangeType(qc.Value1, typeof(object[]))) :
                        this.NotIn(qc.SrcExpression, qc.Concat, ConverterHelper.ChangeType(qc.Value1, typeof(object[])));
                    break;
                case OpertionSymbol.Between:
                    expr = qc.SrcExpression == null ? this.Between(qc.PropertyName, qc.Value1, qc.Value2, qc.Concat) :
                        this.Between(qc.SrcExpression, qc.Value1, qc.Value2, qc.Concat);
                    break;
                case OpertionSymbol.NotBetween:
                    expr = qc.SrcExpression == null ? this.NotBetween(qc.PropertyName, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), ConverterHelper.ChangeType(qc.Value2, qc.ProperyType), qc.Concat) :
                        this.NotBetween(qc.SrcExpression, ConverterHelper.ChangeType(qc.Value1, qc.ProperyType), ConverterHelper.ChangeType(qc.Value2, qc.ProperyType), qc.Concat);
                    break;
                case OpertionSymbol.Fuzzy:
                    expr = qc.SrcExpression == null ? this.Fuzzy(qc.PropertyName, ConverterHelper.ConvertTo<string>(qc.Value1), qc.Concat) :
                        this.Fuzzy(qc.SrcExpression, ConverterHelper.ConvertTo<string>(qc.Value1), qc.Concat);
                    break;
                case OpertionSymbol.NotFuzzy:
                    expr = qc.SrcExpression == null ? this.NotFuzzy(qc.PropertyName, ConverterHelper.ConvertTo<string>(qc.Value1), qc.Concat) :
                        this.NotFuzzy(qc.SrcExpression, ConverterHelper.ConvertTo<string>(qc.Value1), qc.Concat);
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
