using InfrastructurePlugins.BaseModule.Module;
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

            Parameters = new ParameterExpression[] { paraExpr };
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
            return MakeLambdaExpression(root.Expression);
        }

        /// <summary>
        /// 获取某个结点的表达式树
        /// </summary>
        /// <typeparam name="TFunc"></typeparam>
        /// <param name="body"></param>
        /// <returns></returns>
        public Expression<Func<T, bool>> MakeLambdaExpression(Expression body)
        {
            var bodyExpr = body ?? Expression.Constant(true);
            return Expression.Lambda<Func<T, bool>>(bodyExpr, this.Parameters);
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
            if (!c.IsChildExpress)
            {
                if (c.IsSetNode && !c.IsCustomColumnFilter)
                    RecursionGenerateListExpression(c);
                else
                    GenerateExpression(c);
            }
            foreach (var t in c.Childs)
            {
                if (t.IsSetNode && c.IsProcessDone) break;
                RecursionGenerateExpression(t);
                //拼接表达式树
                ConcatExpression(c, t, c.Childs.First() == t);
            }
        }

        protected class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }
        public static LambdaExpressionBuilder<T> CreateExpressionBuilder<T>()
        {
            return new LambdaExpressionBuilder<T>();
        }
        public LambdaExpression CreatePropertyExpression(string propertyName)
        {
            var props = propertyName.Split('.');
            ParameterExpression paraExpr = this.Parameters[0];
            Expression propExpr = paraExpr;
            foreach (var prop in props)
            {
                propExpr = propExpr.Property(prop);
            }
            return Expression.Lambda(propExpr, paraExpr);
        }
        public Expression GetMemberExpression(LambdaExpression property)
        {
            var paraExpr = Parameters[0];
            if (paraExpr == null)
            {
                return property.Body;
            }
            ParameterExpressionVisitor visitor = new ParameterExpressionVisitor(paraExpr);
            Expression memberExpr = visitor.ReplaceParameter(property.Body);
            return memberExpr;
        }
        public Expression Any(ColumnQueryCondition c, Expression predicate)
        {
            var memberExpr = c.ParentExpressionBuilder.CreatePropertyExpression(c.MemberName);
            return Expression.Call(typeof(Enumerable),
                "Any",
                new Type[] { c.ModelType },
                c.ParentExpressionBuilder.GetMemberExpression(memberExpr), predicate);
        }
        public Expression All(ColumnQueryCondition c, Expression predicate)
        {
            var memberExpr = c.ParentExpressionBuilder.CreatePropertyExpression(c.MemberName);
            return Expression.Call(typeof(Enumerable),
                "All",
                new Type[] { c.ModelType },
                c.ParentExpressionBuilder.GetMemberExpression(memberExpr), predicate);
        }
        public Expression Count(ColumnQueryCondition c, Expression predicate)
        {
            var memberExpr = c.ParentExpressionBuilder.CreatePropertyExpression(c.MemberName);
            return Expression.Call(typeof(Enumerable),
                "Count",
                new Type[] { c.ModelType },
                c.ParentExpressionBuilder.GetMemberExpression(memberExpr), predicate);
        }
        public Expression Max(ColumnQueryCondition c, Expression predicate)
        {
            var memberExpr = c.ParentExpressionBuilder.CreatePropertyExpression(c.MemberName);
            return Expression.Call(typeof(Enumerable),
                "Max",
                new Type[] { c.ModelType },
                c.ExpressionBuilder.GetMemberExpression(memberExpr), predicate);
        }
        public Expression Min(ColumnQueryCondition c, Expression predicate)
        {
            var memberExpr = c.ParentExpressionBuilder.CreatePropertyExpression(c.MemberName);
            return Expression.Call(typeof(Enumerable),
                "Min",
                new Type[] { c.ModelType },
                c.ExpressionBuilder.GetMemberExpression(memberExpr), predicate);
        }
        public Expression Average(ColumnQueryCondition c, Expression predicate)
        {
            var memberExpr = c.ParentExpressionBuilder.CreatePropertyExpression(c.MemberName);
            return Expression.Call(typeof(Enumerable),
                "Average",
                new Type[] { c.ModelType },
                c.ExpressionBuilder.GetMemberExpression(memberExpr), predicate);
        }
        public Expression Sum(ColumnQueryCondition c, Expression predicate)
        {
            var memberExpr = c.ParentExpressionBuilder.CreatePropertyExpression(c.MemberName);
            return Expression.Call(typeof(Enumerable),
                "Sum",
                new Type[] { c.ModelType },
                c.ExpressionBuilder.GetMemberExpression(memberExpr), predicate);
        }
        public Expression GenerateListExpression(ColumnQueryCondition c, Expression predicateExpr)
        {
            Expression expr = null;
            switch (c.SetOpertion)
            {
                case SetOpertionSymbol.None:
                    break;
                case SetOpertionSymbol.Any:
                    expr = c.ParentExpressionBuilder.Any(c, predicateExpr);
                    break;
                case SetOpertionSymbol.All:
                    expr = c.ParentExpressionBuilder.All(c, predicateExpr);
                    break;
                case SetOpertionSymbol.Count:
                    c.SrcExpression = Expression.Lambda(c.ParentExpressionBuilder.Count(c, predicateExpr), c.ParentExpressionBuilder.Parameters[0]);
                    c.ParentExpressionBuilder.GenerateExpression(c);
                    expr = c.Expression;
                    break;
                case SetOpertionSymbol.Min:
                    c.SrcExpression = Expression.Lambda(c.ParentExpressionBuilder.Min(c, predicateExpr), c.ParentExpressionBuilder.Parameters[0]);
                    c.ParentExpressionBuilder.GenerateExpression(c);
                    expr = c.Expression;
                    break;
                case SetOpertionSymbol.Max:
                    c.SrcExpression = Expression.Lambda(c.ParentExpressionBuilder.Max(c, predicateExpr), c.ParentExpressionBuilder.Parameters[0]);
                    c.ParentExpressionBuilder.GenerateExpression(c);
                    expr = c.Expression;
                    break;
                case SetOpertionSymbol.Average:
                    c.SrcExpression = Expression.Lambda(c.ParentExpressionBuilder.Average(c, predicateExpr), c.ParentExpressionBuilder.Parameters[0]);
                    c.ParentExpressionBuilder.GenerateExpression(c);
                    expr = c.Expression;
                    break;
                case SetOpertionSymbol.Sum:
                    c.SrcExpression = Expression.Lambda(c.ParentExpressionBuilder.Sum(c, predicateExpr), c.ParentExpressionBuilder.Parameters[0]);
                    c.ParentExpressionBuilder.GenerateExpression(c);
                    expr = c.Expression;
                    break;
                default:
                    break;
            }
            return expr;
        }
        /// <summary>
        /// 递归生成列表对象表达式树
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private Expression RecursionGenerateListExpression(ColumnQueryCondition c)
        {
            Expression predicateExpr = null;
            c.IsProcessDone = true;
            foreach (var child in c.Childs)
            {
                if (child.PropClassify == PropClassify.List && !child.IsCustomColumnFilter)
                {
                    child.Expression = RecursionGenerateListExpression(child);
                }
                else
                {
                    c.ExpressionBuilder.GenerateExpression(child);
                }
                ConcatExpression(predicateExpr, child, c.Childs.First() == child);
            }
            if (c.PropClassify == PropClassify.List)
            {
                c.Expression = c.ParentExpressionBuilder.GenerateListExpression(c, predicateExpr);
            }
            else
            {
                c.ExpressionBuilder.GenerateExpression(c);
            }
            return c.Expression;
        }

        /// <summary>
        /// 生成单个条件的表达式
        /// </summary>
        /// <param name="filterRequest"></param>
        public void GenerateExpression(ColumnQueryCondition filterRequest)
        {
            Expression expr = null;
            if (filterRequest.IsCustomColumnFilter || filterRequest.IsSetOperation)
            {
                var leftParamExpr = this.Parameters[0];
                var rightParamExpr = filterRequest.SrcExpression.Parameters[0];
                var visitor = new ReplaceExpressionVisitor(rightParamExpr, leftParamExpr);
                var rightBodyExpr = visitor.Visit(filterRequest.SrcExpression.Body);
                filterRequest.Expression = rightBodyExpr;
                return;
            }
            //if (filterRequest.PropClassify == PropClassify.List)
            //{
            //    return;
            //}

            //if (!string.IsNullOrEmpty(qc.RegExp))
            //{
            //    var regExpr = this.RegExp(qc.SrcExpression, qc.RegExp);
            //    qc.Expression = regExpr;
            //    return;
            //}
            //根据操作符生成相应的表达式
            switch (filterRequest.OpertionSymbol)
            {
                case OpertionSymbol.Equals:
                    expr = filterRequest.SrcExpression == null ? this.Equals(filterRequest.MemberName, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat) :
                    this.Equals(filterRequest.SrcExpression, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat);
                    break;
                case OpertionSymbol.NotEquals:
                    expr = filterRequest.SrcExpression == null ? this.NotEquals(filterRequest.MemberName, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat) :
                       this.NotEquals(filterRequest.SrcExpression, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat);
                    break;
                case OpertionSymbol.GreaterThan:
                    expr = filterRequest.SrcExpression == null ? this.GreaterThan(filterRequest.MemberName, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat) :
                        this.GreaterThan(filterRequest.SrcExpression, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat);
                    break;
                case OpertionSymbol.NotGreaterThan:
                    expr = filterRequest.SrcExpression == null ? this.NotGreaterThan(filterRequest.MemberName, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat) :
                        this.NotGreaterThan(filterRequest.SrcExpression, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat);
                    break;
                case OpertionSymbol.GreaterThanOrEqueals:
                    expr = filterRequest.SrcExpression == null ? this.GreaterThanOrEquals(filterRequest.MemberName, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat) :
                        this.GreaterThanOrEquals(filterRequest.SrcExpression, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat);
                    break;
                case OpertionSymbol.LessThan:
                    expr = filterRequest.SrcExpression == null ? this.LessThan(filterRequest.MemberName, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat) :
                        this.LessThan(filterRequest.SrcExpression, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat);
                    break;
                case OpertionSymbol.NotLessThan:
                    expr = filterRequest.SrcExpression == null ? this.LessThan(filterRequest.MemberName, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat) :
                         this.LessThan(filterRequest.SrcExpression, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat);
                    break;
                case OpertionSymbol.LessThanOrEqual:
                    expr = filterRequest.SrcExpression == null ? this.LessThanOrEqual(filterRequest.MemberName, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat) :
                        this.LessThanOrEqual(filterRequest.SrcExpression, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), filterRequest.Concat);
                    break;
                case OpertionSymbol.Like:
                    expr = filterRequest.SrcExpression == null ? this.Like<T>(filterRequest.MemberName, Convert.ToString(filterRequest.Value1)) :
                        this.Like<T>(filterRequest.SrcExpression, filterRequest.Value1 as string);
                    break;
                case OpertionSymbol.NotLike:
                    expr = filterRequest.SrcExpression == null ? this.NotLike<T>(filterRequest.MemberName, Convert.ToString(filterRequest.Value1)) :
                        this.NotLike<T>(filterRequest.SrcExpression, filterRequest.Value1 as string);
                    break;
                case OpertionSymbol.StartsWith:
                    expr = filterRequest.SrcExpression == null ? this.StartsWith(filterRequest.MemberName, ConverterHelper.ConvertTo<string>(filterRequest.Value1), filterRequest.Concat) :
                        this.StartsWith(filterRequest.SrcExpression, ConverterHelper.ConvertTo<string>(filterRequest.Value1), filterRequest.Concat);
                    break;
                case OpertionSymbol.NotStartsWith:
                    expr = filterRequest.SrcExpression == null ? this.StartsWith(filterRequest.MemberName, ConverterHelper.ConvertTo<string>(filterRequest.Value1), filterRequest.Concat) :
                        this.StartsWith(filterRequest.SrcExpression, ConverterHelper.ConvertTo<string>(filterRequest.Value1), filterRequest.Concat);
                    break;
                case OpertionSymbol.EndsWith:
                    expr = filterRequest.SrcExpression == null ? this.EndsWith(filterRequest.MemberName, Convert.ToString(filterRequest.Value1), filterRequest.Concat) :
                        this.EndsWith(filterRequest.SrcExpression, filterRequest.Value1 as string, filterRequest.Concat);
                    break;
                case OpertionSymbol.NotEndsWith:
                    expr = filterRequest.SrcExpression == null ? this.NotEndsWith(filterRequest.MemberName, Convert.ToString(filterRequest.Value1), filterRequest.Concat) :
                        this.NotEndsWith(filterRequest.SrcExpression, filterRequest.Value1 as string, filterRequest.Concat);
                    break;
                case OpertionSymbol.In:
                    expr = filterRequest.SrcExpression == null ? this.In(filterRequest.MemberName, filterRequest.Concat, ConverterHelper.ChangeType(filterRequest.Value1, typeof(object[]))) :
                        this.In(filterRequest.SrcExpression, filterRequest.Concat, ConverterHelper.ChangeType(filterRequest.Value1, typeof(object[])));
                    break;
                case OpertionSymbol.NotIn:
                    expr = filterRequest.SrcExpression == null ? this.NotIn(filterRequest.MemberName, filterRequest.Concat, ConverterHelper.ChangeType(filterRequest.Value1, typeof(object[]))) :
                        this.NotIn(filterRequest.SrcExpression, filterRequest.Concat, ConverterHelper.ChangeType(filterRequest.Value1, typeof(object[])));
                    break;
                case OpertionSymbol.Between:
                    expr = filterRequest.SrcExpression == null ? this.Between(filterRequest.MemberName, filterRequest.Value1, filterRequest.Value2, filterRequest.Concat) :
                        this.Between(filterRequest.SrcExpression, filterRequest.Value1, filterRequest.Value2, filterRequest.Concat);
                    break;
                case OpertionSymbol.NotBetween:
                    expr = filterRequest.SrcExpression == null ? this.NotBetween(filterRequest.MemberName, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), ConverterHelper.ChangeType(filterRequest.Value2, filterRequest.ProperyType), filterRequest.Concat) :
                        this.NotBetween(filterRequest.SrcExpression, ConverterHelper.ChangeType(filterRequest.Value1, filterRequest.ProperyType), ConverterHelper.ChangeType(filterRequest.Value2, filterRequest.ProperyType), filterRequest.Concat);
                    break;
                case OpertionSymbol.Fuzzy:
                    expr = filterRequest.SrcExpression == null ? this.Fuzzy(filterRequest.MemberName, ConverterHelper.ConvertTo<string>(filterRequest.Value1), filterRequest.Concat) :
                        this.Fuzzy(filterRequest.SrcExpression, ConverterHelper.ConvertTo<string>(filterRequest.Value1), filterRequest.Concat);
                    break;
                case OpertionSymbol.NotFuzzy:
                    expr = filterRequest.SrcExpression == null ? this.NotFuzzy(filterRequest.MemberName, ConverterHelper.ConvertTo<string>(filterRequest.Value1), filterRequest.Concat) :
                        this.NotFuzzy(filterRequest.SrcExpression, ConverterHelper.ConvertTo<string>(filterRequest.Value1), filterRequest.Concat);
                    break;
                case OpertionSymbol.RegExp:
                    expr = filterRequest.SrcExpression == null ? this.RegExp(filterRequest.MemberName, filterRequest.Value1.ToString()) :
                        this.RegExp(filterRequest.SrcExpression, filterRequest.Value1.ToString());
                    break;
                default:
                    break;
            }

            filterRequest.Expression = expr;
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
        /// <summary>
        /// 接接表达式树
        /// </summary>
        /// <param name="p"></param>
        /// <param name="c"></param>
        /// <param name="first"></param>
        private void ConcatExpression(Expression p, ColumnQueryCondition c, bool first)
        {
            if (first)
            {
                p = c.Expression;
            }
            else if (c.Concat == ConcatType.AndAlso || c.Concat == ConcatType.None)
            {
                p = Expression.AndAlso(p, c.Expression);
            }
            else if (c.Concat == ConcatType.OrElse)
            {
                p = Expression.OrElse(p, c.Expression);
            }
            else if (c.Concat == ConcatType.Not)
            {
                p = Expression.Not(p);
            }
        }
    }
}
