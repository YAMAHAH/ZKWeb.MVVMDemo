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
            //  Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), typeof(bool));
            var bodyExpr = body == null ? Expression.Constant(true) : body;
            // LambdaExpression lambda = Expression.Lambda(delegateType, bodyExpr, this.Parameters);
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
                if (c.PropClassify == PropClassify.List)
                    RecursionGenerateListExpression(c);
                else
                    GenerateExpression(c);
            }
            foreach (var t in c.Childs)
            {
                if (c.PropClassify == PropClassify.List && c.IsProcessDone) break;
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
        public static object CreateBuilder(Type type)
        {
            var builderType = typeof(LambdaExpressionBuilder<>).MakeGenericType(type);
            return Activator.CreateInstance(builderType);
        }

        //U为一个对象,
        //Ritems是U的一个R对象的集合属性,
        //rname是R对象的一个属性,
        //Citems是R对象的一个C对象的集合属性,
        //P是U的一个对象属性,
        //PV是P的一个对象属性,
        //prop1是PV对象的属性,
        //prop2是P对象的属性,
        //Prop3是U对象的一个属性

        //对于集合的操作暂时可取any, all, count

        //构建如下表达式:

        //u.Ritems.any(r=>r.rname == "rname" && r.Citems.any(c=>c.id == "13")) ||

        //(u.p.pv.Prop1 == "myprop1" && u.p.prop2 == "myprop2") &&

        //u.prop3 == "myprop3"

        //column prefix
        //Ritems      null     =>Ritems.any()
        //Citems      Ritems    =>Ritems.any(r=>r.Citems.any())
        //rname Ritems => Ritems.any(r => r.rname == 455)
        //id Ritems.Citems => r.Citems.any(c => c.id == "13")
        //p           null     =>p
        //pv          p        =>p.pv
        //prop1       p.pv     =>p.pv.prop1
        //prop2       p        =>p.prop2

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
                    break;
                case SetOpertionSymbol.Min:
                    break;
                case SetOpertionSymbol.Max:
                    break;
                case SetOpertionSymbol.average:
                    break;
                default:
                    break;
            }
            return expr;
        }
        private Expression RecursionGenerateListExpression(ColumnQueryCondition c)
        {
            Expression predicateExpr = null;
            c.IsProcessDone = true;
            foreach (var child in c.Childs)
            {
                if (child.PropClassify == PropClassify.List)
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
            //属性   操作   值  连接 u.Ritems.Any(r=>r.Rname=="Rname" && r.Citems.Any(c=>c.id == "13"))
            // Ritems Any none none list  1
            //   Rname Equals "rname" none basic 2
            //   Citems Any none and list 3
            //     id Equals "13" none basic 4
            //

            //0.从c中获取前缀
            //1.构建u.Ritems
            //2.创建any()
            // u.Ritems.any(r => r.rname == "rname" && r.Citems.any(c => c.id == "13"))
            ////****************请求的是复杂对象列表(any,all)************************
            ////创建基于UserToRole表达式工厂
            //var childExprFactory = new ExpressionCreateFactory<UserToRole, UserToRoleOutputDto, Guid>();
            ////创建表达式树生成器
            //var lbdExprBuilder = childExprFactory.CreateBuilder();

            ////创建子查询表达式
            //Expression<Func<UserToRole, bool>> childQueryExpr = childExprFactory.CreateChildQueryExpression(c.Childs.ToArray());
            ////创建基于User表达式工厂
            //var anyExprFactory = new ExpressionCreateFactory<User, UserOutputDto, Guid>();
            ////创建表达式生成器
            //var anyExprBuilder = anyExprFactory.CreateBuilder();
            ////创建any方法表达式
            //var anyExpr = anyExprBuilder.Any(c.Column, childQueryExpr);
            ////创建e.Roles.Any(childQueryExpr)
            //var resultExpr = anyExprBuilder.GetLambdaExpression(anyExpr);
        }

        /// <summary>
        /// 生成单个条件的表达式
        /// </summary>
        /// <param name="qc"></param>
        public void GenerateExpression(ColumnQueryCondition qc)
        {
            Expression expr = null;
            if (qc.IsCustomColumnFilter)
            {
                var leftParamExpr = this.Parameters[0];
                var rightParamExpr = qc.SrcExpression.Parameters[0];
                var visitor = new ReplaceExpressionVisitor(rightParamExpr, leftParamExpr);
                var rightBodyExpr = visitor.Visit(qc.SrcExpression.Body);
                qc.Expression = rightBodyExpr;
                return;
            }
            if (qc.PropClassify == PropClassify.List)
            {
                return;
            }
            //if (!string.IsNullOrEmpty(qc.RegExp))
            //{
            //    var regExpr = this.RegExp(qc.SrcExpression, qc.RegExp);
            //    qc.Expression = regExpr;
            //    return;
            //}
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
                    expr = qc.SrcExpression == null ? this.Like<T>(qc.PropertyName, Convert.ToString(qc.Value1)) :
                        this.Like<T>(qc.SrcExpression, qc.Value1 as string);
                    break;
                case OpertionSymbol.NotLike:
                    expr = qc.SrcExpression == null ? this.NotLike<T>(qc.PropertyName, Convert.ToString(qc.Value1)) :
                        this.NotLike<T>(qc.SrcExpression, qc.Value1 as string);
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
                    expr = qc.SrcExpression == null ? this.EndsWith(qc.PropertyName, Convert.ToString(qc.Value1), qc.Concat) :
                        this.EndsWith(qc.SrcExpression, qc.Value1 as string, qc.Concat);
                    break;
                case OpertionSymbol.NotEndsWith:
                    expr = qc.SrcExpression == null ? this.NotEndsWith(qc.PropertyName, Convert.ToString(qc.Value1), qc.Concat) :
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
                case OpertionSymbol.RegExp:
                    expr = qc.SrcExpression == null ? this.RegExp(qc.PropertyName, qc.Value1.ToString()) :
                        this.RegExp(qc.SrcExpression, qc.Value1.ToString());
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
