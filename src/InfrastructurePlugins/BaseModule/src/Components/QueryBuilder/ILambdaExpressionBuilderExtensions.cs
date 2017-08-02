using InfrastructurePlugins.BaseModule.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace InfrastructurePlugins.BaseModule.Components.QueryBuilder
{
    public static class ILambdaExpressionBuilderExtensions
    {
        public static void SetQueryCondtion<T>(this ILambdaExpressionBuilder<T> q, ICollection<QueryCondition> queryCondtions)
        {
            q.QueryCondtions = queryCondtions;
        }
        /// <summary>
        /// 获取成员表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="q"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private static Expression GetMemberExpression<T, P>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, P>> property)
        {
            if (q.Parameters == null || q.Parameters.Length == 0)
            {
                q.Parameters = property.GetParameters();
                return property.Body;
            }
            ParameterExpressionVisitor visitor = new ParameterExpressionVisitor(q.Parameters[0]);
            Expression memberExpr = visitor.ReplaceParameter(property.Body);
            for (int i = 1; i < q.Parameters.Length; i++)
            {
                visitor = new ParameterExpressionVisitor(q.Parameters[0]);
                memberExpr = visitor.ReplaceParameter(property.Body);
            }
            return memberExpr;
        }
        private static Expression GetMemberExpression<T>(this ILambdaExpressionBuilder<T> q, LambdaExpression property)
        {
            if (q.Parameters == null || q.Parameters.Length == 0)
            {
                q.Parameters = property.Parameters.ToArray();
                return property.Body;
            }

            ParameterExpressionVisitor visitor = new ParameterExpressionVisitor(q.Parameters[0]);
            Expression memberExpr = visitor.ReplaceParameter(property.Body);
            for (int i = 1; i < q.Parameters.Length; i++)
            {
                visitor = new ParameterExpressionVisitor(q.Parameters[0]);
                memberExpr = visitor.ReplaceParameter(property.Body);
            }
            return memberExpr;
        }
        public static LambdaExpression Property<T>(this ILambdaExpressionBuilder<T> q, string propertyName)
        {
            //记得要加上属性名称
            ParameterExpression paraExpr = Expression.Parameter(typeof(T), q.ParameterName);
            Expression propExpr = paraExpr.Property(propertyName);
            return Expression.Lambda(propExpr, paraExpr);
        }

        #region Between
        private static readonly DateTime minDate = new DateTime(1800, 1, 1);
        private static readonly DateTime maxDate = new DateTime(2999, 1, 1);

        /// <summary>
        /// 建立 Between 查询条件
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="q">动态查询条件创建者</param>
        /// <param name="property">属性</param>
        /// <param name="from">开始值</param>
        /// <param name="to">结束值</param>
        /// <returns></returns>
        public static void Between<T, P>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, P>> property, P from, P to, ConcatType concat)
        {
            Type type = typeof(P);
            var constantFrom = Expression.Constant(from);
            var constantTo = Expression.Constant(to);

            var propertyBody = GetMemberExpression(q, property);

            Expression nonNullProperty = propertyBody;

            //如果是Nullable<X>类型，则转化成X类型
            if (type.IsNullableType())
            {
                type = type.GetNonNullableType();
                nonNullProperty = Expression.Convert(propertyBody, type);
            }
            var c1 = Expression.GreaterThanOrEqual(nonNullProperty, constantFrom);
            var c2 = Expression.LessThanOrEqual(nonNullProperty, constantTo);
            var c = Expression.AndAlso(c1, c2);
            // q.AppendExpression(new QueryExpression(c, concat));
        }
        public static Expression Between<T, P>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, P from, P to, ConcatType concat = ConcatType.AndAlso)
        {
            Type type = typeof(P);
            var constantFrom = Expression.Constant(from);
            var constantTo = Expression.Constant(to);

            var propertyBody = GetMemberExpression<T>(q, property);

            Expression nonNullProperty = propertyBody;

            //如果是Nullable<X>类型，则转化成X类型
            if (type.IsNullableType())
            {
                type = type.GetNonNullableType();
                nonNullProperty = Expression.Convert(propertyBody, type);
            }
            var c1 = Expression.GreaterThanOrEqual(nonNullProperty, constantFrom);
            var c2 = Expression.LessThanOrEqual(nonNullProperty, constantTo);
            var c = Expression.AndAlso(c1, c2);
            // q.AppendExpression(new QueryExpression(c, concat));
            return c;
        }
        public static Expression Between<T, P>(this ILambdaExpressionBuilder<T> q, string propertyName, P from, P to, ConcatType concat = ConcatType.AndAlso)
        {
            return Between<T, P>(q, Property<T>(q, propertyName), from, to, concat);
        }

        /// <summary>
        /// 不介于
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="q"></param>
        /// <param name="property"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static Expression NotBetween<T, P>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, P>> property, P from, P to, ConcatType concat)
        {
            Type type = typeof(P);
            var constantFrom = Expression.Constant(from);
            var constantTo = Expression.Constant(to);

            var propertyBody = GetMemberExpression(q, property);

            Expression nonNullProperty = propertyBody;

            //如果是Nullable<X>类型，则转化成X类型
            if (type.IsNullableType())
            {
                type = type.GetNonNullableType();
                nonNullProperty = Expression.Convert(propertyBody, type);
            }
            var c1 = Expression.GreaterThanOrEqual(nonNullProperty, constantFrom);
            var c2 = Expression.LessThanOrEqual(nonNullProperty, constantTo);
            var c = Expression.AndAlso(c1, c2);
            // q.AppendExpression(new QueryExpression(Expression.Not(c), concat));

            return Expression.Not(c);
        }
        public static Expression NotBetween<T, P>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, P from, P to, ConcatType concat = ConcatType.AndAlso)
        {
            Type type = typeof(P);
            var constantFrom = Expression.Constant(from);
            var constantTo = Expression.Constant(to);

            var propertyBody = GetMemberExpression<T>(q, property);

            Expression nonNullProperty = propertyBody;

            //如果是Nullable<X>类型，则转化成X类型
            if (type.IsNullableType())
            {
                type = type.GetNonNullableType();
                nonNullProperty = Expression.Convert(propertyBody, type);
            }
            var c1 = Expression.GreaterThanOrEqual(nonNullProperty, constantFrom);
            var c2 = Expression.LessThanOrEqual(nonNullProperty, constantTo);
            var c = Expression.AndAlso(c1, c2);
            // q.AppendExpression(new QueryExpression(Expression.Not(c), concat));
            return Expression.Not(c);
        }
        public static Expression NotBetween<T, P>(this ILambdaExpressionBuilder<T> q, string propertyName, P from, P to, ConcatType concat = ConcatType.AndAlso)
        {
            return NotBetween<T, P>(q, Property<T>(q, propertyName), from, to, concat);
        }

        /// <summary>
        /// 建立 Between 查询条件
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="q">动态查询条件创建者</param>
        /// <param name="property">属性</param>
        /// <param name="from">开始值</param>
        /// <param name="to">结束值</param>
        /// <returns></returns>
        public static Expression Between<T>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, string>> property, string from, string to, ConcatType concat)
        {
            from = from.Trim();
            to = to.Trim();
            Expression c = null;
            if (from != string.Empty && to != string.Empty)
            {
                var propertyBody = GetMemberExpression(q, property);
                var constantFrom = Expression.Constant(from);
                var constantTo = Expression.Constant(to);
                var constantZero = Expression.Constant(0);
                var compareMethod = typeof(string).GetMethod("Compare", new Type[] { typeof(string), typeof(string) });

                MethodCallExpression methodExp1 = Expression.Call(null, compareMethod, propertyBody, constantFrom);
                var c1 = Expression.GreaterThanOrEqual(methodExp1, constantZero);
                MethodCallExpression methodExp2 = Expression.Call(null, compareMethod, propertyBody, constantTo);
                var c2 = Expression.LessThanOrEqual(methodExp2, constantZero);
                c = Expression.AndAlso(c1, c2);

                // q.AppendExpression(new QueryExpression(c, concat));
            }

            return c;
        }

        public static Expression Between<T>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, string from, string to, ConcatType concat = ConcatType.AndAlso)
        {
            from = from.Trim();
            to = to.Trim();
            Expression c = null;
            if (from != string.Empty && to != string.Empty)
            {
                var propertyBody = GetMemberExpression<T>(q, property);
                var constantFrom = Expression.Constant(from);
                var constantTo = Expression.Constant(to);
                var constantZero = Expression.Constant(0);
                var compareMethod = typeof(string).GetMethod("Compare", new Type[] { typeof(string), typeof(string) });

                MethodCallExpression methodExp1 = Expression.Call(null, compareMethod, propertyBody, constantFrom);
                var c1 = Expression.GreaterThanOrEqual(methodExp1, constantZero);
                MethodCallExpression methodExp2 = Expression.Call(null, compareMethod, propertyBody, constantTo);
                var c2 = Expression.LessThanOrEqual(methodExp2, constantZero);
                c = Expression.AndAlso(c1, c2);

                //  q.AppendExpression(new QueryExpression(c, concat));

            }
            return c;
        }
        public static Expression Between<T>(this ILambdaExpressionBuilder<T> q, string propertyName, string from, string to, ConcatType concat = ConcatType.AndAlso)
        {
            return Between<T>(q, Property<T>(q, propertyName), from, to, concat);
        }

        public static Expression NotBetween<T>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, string from, string to, ConcatType concat = ConcatType.AndAlso)
        {
            from = from.Trim();
            to = to.Trim();
            Expression c = null;
            if (from != string.Empty && to != string.Empty)
            {
                var propertyBody = GetMemberExpression<T>(q, property);
                var constantFrom = Expression.Constant(from);
                var constantTo = Expression.Constant(to);
                var constantZero = Expression.Constant(0);
                var compareMethod = typeof(string).GetMethod("Compare", new Type[] { typeof(string), typeof(string) });

                MethodCallExpression methodExp1 = Expression.Call(null, compareMethod, propertyBody, constantFrom);
                var c1 = Expression.GreaterThanOrEqual(methodExp1, constantZero);
                MethodCallExpression methodExp2 = Expression.Call(null, compareMethod, propertyBody, constantTo);
                var c2 = Expression.LessThanOrEqual(methodExp2, constantZero);
                c = Expression.AndAlso(c1, c2);

                // q.AppendExpression(new QueryExpression(Expression.Not(c), concat));
            }
            return c;
        }

        public static Expression NotBetween<T>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, string>> property, string from, string to, ConcatType concat)
        {
            from = from.Trim();
            to = to.Trim();
            Expression c = null;
            if (from != string.Empty && to != string.Empty)
            {
                var propertyBody = GetMemberExpression(q, property);
                var constantFrom = Expression.Constant(from);
                var constantTo = Expression.Constant(to);
                var constantZero = Expression.Constant(0);
                var compareMethod = typeof(string).GetMethod("Compare", new Type[] { typeof(string), typeof(string) });

                MethodCallExpression methodExp1 = Expression.Call(null, compareMethod, propertyBody, constantFrom);
                var c1 = Expression.GreaterThanOrEqual(methodExp1, constantZero);
                MethodCallExpression methodExp2 = Expression.Call(null, compareMethod, propertyBody, constantTo);
                var c2 = Expression.LessThanOrEqual(methodExp2, constantZero);
                c = Expression.AndAlso(c1, c2);

                // q.AppendExpression(new QueryExpression(Expression.Not(c), concat));
            }
            return c;
        }

        public static Expression NotBetween<T>(this ILambdaExpressionBuilder<T> q, string propertyName, string from, string to, ConcatType concat = ConcatType.AndAlso)
        {
            return NotBetween<T>(q, Property<T>(q, propertyName), from, to, concat);
        }
        #endregion

        #region Like

        /// <summary>
        /// 建立 Like ( 模糊 ) 查询条件
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="q">动态查询条件创建者</param>
        /// <param name="property">属性</param>
        /// <param name="value">查询值</param>
        /// <returns></returns>
        /// 
        public static Expression Like<T>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, string>> property, string value, ConcatType concat)
        {
            Expression methodExpr = null;
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();

                var propertyBody = GetMemberExpression<T>(q, property);

                methodExpr = Expression.Call(propertyBody,
                    typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                    Expression.Constant(value)
                );
                // q.AppendExpression(new QueryExpression(methodExpr, concat));
            }
            return methodExpr;
        }

        public static Expression Like<T>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, string value, ConcatType concat = ConcatType.AndAlso)
        {
            Expression expr = null;
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();

                var propertyBody = GetMemberExpression<T>(q, property);

                MethodCallExpression methodExpr = Expression.Call(propertyBody,
                    typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                    Expression.Constant(value)
                );
                expr = methodExpr;
                // q.AppendExpression(new QueryExpression(methodExpr, concat));
            }
            return expr;
        }

        public static Expression Like<T>(this ILambdaExpressionBuilder<T> q, string propertyName, string value, ConcatType concat = ConcatType.AndAlso)
        {
            var propExpr = Property<T>(q, propertyName);
            return Like<T>(q, propExpr, value, concat);
        }

        /// <summary>
        /// 不包含
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression NotLike<T>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, string>> property, string value, ConcatType concat)
        {
            Expression methodExpr = null;
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();

                var propertyBody = GetMemberExpression(q, property);

                methodExpr = Expression.Call(propertyBody,
                   typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                   Expression.Constant(value)
               );
                // q.AppendExpression(new QueryExpression(Expression.Not(methodExpr), concat));
            }
            return Expression.Not(methodExpr);
        }

        public static Expression NotLike<T>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, string value, ConcatType concat = ConcatType.AndAlso)
        {
            Expression methodExpr = null;
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();

                var propertyBody = GetMemberExpression<T>(q, property);

                methodExpr = Expression.Call(propertyBody,
                    typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                    Expression.Constant(value)
                );
                // q.AppendExpression(new QueryExpression(Expression.Not(methodExpr), concat));
            }
            return Expression.Not(methodExpr);
        }

        public static Expression NotLike<T>(this ILambdaExpressionBuilder<T> q, string propertyName, string value, ConcatType concat = ConcatType.AndAlso)
        {
            return NotLike<T>(q, Property<T>(q, propertyName), value, concat);
        }
        #endregion

        #region StartsWith
        /// <summary>
        /// 以...开头
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// 

        public static Expression StartsWith<T>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, string value, ConcatType concat = ConcatType.AndAlso)
        {
            Expression methodExpr = null;
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();

                var propertyBody = GetMemberExpression<T>(q, property);

                methodExpr = Expression.Call(propertyBody,
                   typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }),
                   Expression.Constant(value)
               );
                // q.AppendExpression(new QueryExpression(methodExpr, concat));
            }
            return methodExpr;
        }
        public static Expression StartsWith<T>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, string>> property, string value, ConcatType concat)
        {
            return StartsWith<T>(q, property, value, concat);
        }
        public static Expression StartsWith<T>(this ILambdaExpressionBuilder<T> q, string propertyName, string value, ConcatType concat = ConcatType.AndAlso)
        {
            return StartsWith<T>(q, Property<T>(q, propertyName), value, concat);
        }
        /// <summary>
        /// 不以...开头
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression NotStartsWith<T>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, string>> property, string value, ConcatType concat)
        {
            return NotStartsWith<T>(q, property, value, concat);
        }

        public static Expression NotStartsWith<T>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, string value, ConcatType concat = ConcatType.AndAlso)
        {
            Expression methodExpr = null;
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();

                var propertyBody = GetMemberExpression<T>(q, property);

                methodExpr = Expression.Call(propertyBody,
                    typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }),
                    Expression.Constant(value)
                );
                // q.AppendExpression(new QueryExpression(Expression.Not(methodExpr), concat));
            }
            return Expression.Not(methodExpr);
        }

        public static Expression NotStartsWith<T>(this ILambdaExpressionBuilder<T> q, string propertyName, string value, ConcatType concat = ConcatType.AndAlso)
        {
            return NotStartsWith<T>(q, Property<T>(q, propertyName), value, concat);
        }
        #endregion

        #region EndsWith
        /// <summary>
        /// 以...结尾
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression EndsWith<T>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, string>> property, string value, ConcatType concat)
        {
            Expression methodExpr = null;
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();

                var propertyBody = GetMemberExpression(q, property);

                methodExpr = Expression.Call(propertyBody,
                    typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }),
                    Expression.Constant(value)
                );
                //  q.AppendExpression(new QueryExpression(methodExpr, concat));
            }
            return methodExpr;
        }

        public static Expression EndsWith<T>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, string value, ConcatType concat = ConcatType.AndAlso)
        {
            Expression methodExpr = null;
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();

                var propertyBody = GetMemberExpression<T>(q, property);

                methodExpr = Expression.Call(propertyBody,
                    typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }),
                    Expression.Constant(value)
                );
                // q.AppendExpression(new QueryExpression(methodExpr, concat));
            }
            return methodExpr;
        }

        public static Expression EndsWith<T>(this ILambdaExpressionBuilder<T> q, string propertyName, string value, ConcatType concat = ConcatType.AndAlso)
        {
            return EndsWith<T>(q, Property<T>(q, propertyName), value, concat);
        }
        /// <summary>
        /// 不以...结尾
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression NotEndsWith<T>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, string>> property, string value, ConcatType concat)
        {
            Expression methodExpr = null;
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();
                var propertyBody = GetMemberExpression(q, property);

                methodExpr = Expression.Call(propertyBody,
                   typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }),
                   Expression.Constant(value)
               );
                // q.AppendExpression(new QueryExpression(Expression.Not(methodExpr), concat));
            }
            return Expression.Not(methodExpr);
        }

        public static Expression NotEndsWith<T>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, string value, ConcatType concat = ConcatType.AndAlso)
        {
            Expression methodExpr = null;
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();
                var propertyBody = GetMemberExpression<T>(q, property);

                methodExpr = Expression.Call(propertyBody,
                    typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }),
                    Expression.Constant(value)
                );
                // q.AppendExpression(new QueryExpression(Expression.Not(methodExpr), concat));
            }
            return Expression.Not(methodExpr);
        }

        public static Expression NotEndsWith<T>(this ILambdaExpressionBuilder<T> q, string propertyName, string value, ConcatType concat = ConcatType.AndAlso)
        {
            return NotEndsWith<T>(q, Property<T>(q, propertyName), value, concat);
        }
        #endregion

        #region Equals

        /// <summary>
        /// 建立 Equals ( 相等 ) 查询条件
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="q">动态查询条件创建者</param>
        /// <param name="property">属性</param>
        /// <param name="value">查询值</param>
        /// <param name="exclude">排除值（意味着如果value==exclude，则当前条件不被包含到查询中）</param>
        /// <returns></returns>
        public static Expression Equals<T, P>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, P>> property, P value, P exclude)
        {
            if (value.Equals(exclude))
            {
                return null;
            }
            return Equals(q, property, value);
        }

        /// <summary>
        /// 建立 Equals ( 相等 ) 查询条件
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="q">动态查询条件创建者</param>
        /// <param name="property">属性</param>
        /// <param name="value">查询值</param>
        /// <returns></returns>
        public static Expression Equals<T, P>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, P>> property, P value, ConcatType concat)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.Equal(left, right);

            // q.AppendExpression(new QueryExpression(methodExpr, concat));

            return methodExpr;
        }

        public static Expression Equals<T, P>(this ILambdaExpressionBuilder<T> q, string propertyName, P value, ConcatType concat = ConcatType.AndAlso)
        {
            return Equals<T, P>(q, Property<T>(q, propertyName), value, concat);
        }
        public static Expression Equals<T, P>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, P value, ConcatType concat = ConcatType.AndAlso)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression<T>(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.Equal(left, right);

            //q.AppendExpression(new QueryExpression(methodExpr, concat));

            return methodExpr;
        }
        public static Expression NotEquals<T, P>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, P value, ConcatType concat = ConcatType.AndAlso)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression<T>(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.NotEqual(left, right);

            // q.AppendExpression(new QueryExpression(methodExpr, concat));
            return methodExpr;
        }
        public static Expression NotEquals<T, P>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, P>> property, P value, ConcatType concat)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.NotEqual(left, right);

            // q.AppendExpression(new QueryExpression(methodExpr, concat));
            return methodExpr;
        }
        public static Expression NotEquals<T, P>(this ILambdaExpressionBuilder<T> q, string propertyName, P value, ConcatType concat = ConcatType.AndAlso)
        {
            return NotEquals<T, P>(q, Property<T>(q, propertyName), value, concat);
        }
        #endregion

        #region GreaterThan
        /// <summary>
        /// 大于
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="q"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression GreaterThan<T, P>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, P>> property, P value, ConcatType concat)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.GreaterThan(left, right);

            // q.AppendExpression(new QueryExpression(methodExpr, concat));
            return methodExpr;
        }
        public static Expression GreaterThan<T, P>(this ILambdaExpressionBuilder<T> q, string propertyName, P value, ConcatType concat = ConcatType.AndAlso)
        {
            return GreaterThan<T, P>(q, Property<T>(q, propertyName), value, concat);
        }
        public static Expression GreaterThan<T, P>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, P value, ConcatType concat = ConcatType.AndAlso)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression<T>(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.GreaterThan(left, right);

            // q.AppendExpression(new QueryExpression(methodExpr, concat));
            return methodExpr;
        }

        public static Expression NotGreaterThan<T, P>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, P value, ConcatType concat = ConcatType.AndAlso)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression<T>(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.GreaterThan(left, right);

            // q.AppendExpression(new QueryExpression(Expression.Not(methodExpr), concat));
            return methodExpr;
        }
        public static Expression NotGreaterThan<T, P>(this ILambdaExpressionBuilder<T> q, string propertyName, P value, ConcatType concat = ConcatType.AndAlso)
        {
            return NotGreaterThan<T, P>(q, Property<T>(q, propertyName), value, concat);
        }

        public static Expression NotGreaterThan<T, P>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, P>> property, P value, ConcatType concat)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.GreaterThan(left, right);

            //q.AppendExpression(new QueryExpression(Expression.Not(methodExpr), concat));
            return methodExpr;
        }
        #endregion

        #region GreaterThanOrEqueals
        /// <summary>
        /// 大于或等于
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="q"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression GreaterThanOrEquals<T, P>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, P>> property, P value, ConcatType concat)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.GreaterThanOrEqual(left, right);

            // q.AppendExpression(new QueryExpression(methodExpr, concat));

            return methodExpr;
        }
        public static Expression GreaterThanOrEquals<T, P>(this ILambdaExpressionBuilder<T> q, string propertyName, P value, ConcatType concat = ConcatType.AndAlso)
        {
            return GreaterThanOrEquals<T, P>(q, Property<T>(q, propertyName), value, concat);
        }
        public static Expression GreaterThanOrEquals<T, P>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, P value, ConcatType concat = ConcatType.AndAlso)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression<T>(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.GreaterThanOrEqual(left, right);

            // q.AppendExpression(new QueryExpression(methodExpr, concat));

            return methodExpr;
        }

        #endregion

        #region LessThan
        /// <summary>
        /// 小于
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="q"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression LessThan<T, P>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, P>> property, P value, ConcatType concat)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.LessThan(left, right);

            //q.AppendExpression(new QueryExpression(methodExpr, concat));

            return methodExpr;
        }
        public static Expression LessThan<T, P>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, P value, ConcatType concat = ConcatType.AndAlso)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression<T>(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.LessThan(left, right);

            //  q.AppendExpression(new QueryExpression(methodExpr, concat));

            return methodExpr;
        }
        public static Expression LessThan<T, P>(this ILambdaExpressionBuilder<T> q, string propertyName, P value, ConcatType concat = ConcatType.AndAlso)
        {
            return LessThan<T, P>(q, Property<T>(q, propertyName), value, concat);
        }

        public static Expression NotLessThan<T, P>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, P>> property, P value, ConcatType concat)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.LessThan(left, right);

            // q.AppendExpression(new QueryExpression(Expression.Not(methodExpr), concat));

            return methodExpr;
        }
        public static Expression NotLessThan<T, P>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, P value, ConcatType concat = ConcatType.AndAlso)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.LessThan(left, right);

            //q.AppendExpression(new QueryExpression(Expression.Not(methodExpr), concat));

            return methodExpr;
        }
        public static Expression NotLessThan<T, P>(this ILambdaExpressionBuilder<T> q, string propertyName, P value, ConcatType concat = ConcatType.AndAlso)
        {
            return NotLessThan<T, P>(q, Property<T>(q, propertyName), value, concat);
        }
        #endregion

        #region LessThanOrEqual
        /// <summary>
        /// 小于或等于
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="q"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression LessThanOrEqual<T, P>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, P>> property, P value, ConcatType concat)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.LessThanOrEqual(left, right);

            // q.AppendExpression(new QueryExpression(methodExpr, concat));

            return methodExpr;
        }
        public static Expression LessThanOrEqual<T, P>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, P value, ConcatType concat = ConcatType.AndAlso)
        {
            Expression right = Expression.Constant(value);
            Type type = typeof(P);

            var propertyBody = GetMemberExpression<T>(q, property);

            Expression left = propertyBody;

            //如果是Nullable类型，则把value转化成Nullable类型
            if (type.IsNullableType())
            {
                right = Expression.Convert(right, type);
            }

            var methodExpr = Expression.LessThanOrEqual(left, right);

            // q.AppendExpression(new QueryExpression(methodExpr, concat));

            return methodExpr;
        }
        public static Expression LessThanOrEqual<T, P>(this ILambdaExpressionBuilder<T> q, string propertyName, P value, ConcatType concat = ConcatType.AndAlso)
        {
            return LessThanOrEqual<T, P>(q, Property<T>(q, propertyName), value, concat);
        }
        #endregion

        #region In


        //TODO:无法直接获取重载的泛型方法？
        //var method = typeof(Enumerable).GetMethod("Contains");
        private static MethodInfo method_Contains =
                        (from m in typeof(Enumerable).GetMethods()
                         where m.Name.Equals("Contains")
                             && m.IsGenericMethod
                             && m.GetGenericArguments().Length == 1
                             && m.GetParameters().Length == 2
                         select m
                        ).First();

        /// <summary>
        /// 建立 In 查询条件
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="q">动态查询条件创建者</param>
        /// <param name="property">属性</param>
        /// <param name="valuse">查询值</param> 
        /// <returns></returns>
        /// 


        public static Expression In<T, P>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, P>> property, ConcatType concat, params P[] values)
        {
            Expression methodExpr = null;
            if (values != null && values.Length > 0)
            {
                Type type = typeof(P);

                object nonNullValues = values;
                //如果是Nullable<X>类型，则转化成X类型
                if (type.IsNullableType())
                {
                    //type = type.GetNonNullableType();

                    //nonNullValues = values.Select(v => Convert.ChangeType(v, type)).ToArray();
                }

                var method = method_Contains.MakeGenericMethod(new Type[] { type });

                var propertyBody = GetMemberExpression(q, property);

                methodExpr = Expression.Call(null,
                    method,
                    Expression.Constant(nonNullValues),
                    propertyBody
                );

                // q.AppendExpression(new QueryExpression(methodExpr, concat));
            }

            return methodExpr;
        }
        public static Expression In<T, P>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, ConcatType concat = ConcatType.AndAlso, params P[] values)
        {
            Expression methodExpr = null;
            if (values != null && values.Length > 0)
            {
                Type type = typeof(P);

                object nonNullValues = values;
                //如果是Nullable<X>类型，则转化成X类型
                if (type.IsNullableType())
                {
                    //type = type.GetNonNullableType();

                    //nonNullValues = values.Select(v => Convert.ChangeType(v, type)).ToArray();
                }

                var method = method_Contains.MakeGenericMethod(new Type[] { type });

                var propertyBody = GetMemberExpression<T>(q, property);

                methodExpr = Expression.Call(null,
                    method,
                    Expression.Constant(nonNullValues),
                    propertyBody
                );

                // q.AppendExpression(new QueryExpression(methodExpr, concat));
            }

            return methodExpr;
        }
        public static Expression In<T, P>(this ILambdaExpressionBuilder<T> q, string propertyName, ConcatType concat = ConcatType.AndAlso, params P[] values)
        {
            return In<T, P>(q, Property<T>(q, propertyName), concat, values);
        }

        public static Expression NotIn<T, P>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, ConcatType concat = ConcatType.AndAlso, params P[] values)
        {
            Expression methodExpr = null;
            if (values != null && values.Length > 0)
            {
                Type type = typeof(P);

                object nonNullValues = values;
                //如果是Nullable<X>类型，则转化成X类型
                if (type.IsNullableType())
                {
                    //type = type.GetNonNullableType();

                    //nonNullValues = values.Select(v => Convert.ChangeType(v, type)).ToArray();
                }

                var method = method_Contains.MakeGenericMethod(new Type[] { type });

                var propertyBody = GetMemberExpression<T>(q, property);

                methodExpr = Expression.Call(null,
                    method,
                    Expression.Constant(nonNullValues),
                    propertyBody
                );

                // q.AppendExpression(new QueryExpression(Expression.Not(methodExpr), concat));
            }

            return methodExpr;
        }
        public static Expression NotIn<T, P>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, P>> property, ConcatType concat, params P[] values)
        {
            Expression methodExpr = null;
            if (values != null && values.Length > 0)
            {
                Type type = typeof(P);

                object nonNullValues = values;
                //如果是Nullable<X>类型，则转化成X类型
                if (type.IsNullableType())
                {
                    //type = type.GetNonNullableType();

                    //nonNullValues = values.Select(v => Convert.ChangeType(v, type)).ToArray();
                }

                var method = method_Contains.MakeGenericMethod(new Type[] { type });

                var propertyBody = GetMemberExpression(q, property);

                methodExpr = Expression.Call(null,
                   method,
                   Expression.Constant(nonNullValues),
                   propertyBody
               );

                // q.AppendExpression(new QueryExpression(Expression.Not(methodExpr), concat));
            }

            return Expression.Not(methodExpr);
        }
        public static Expression NotIn<T, P>(this ILambdaExpressionBuilder<T> q, string property, ConcatType concat = ConcatType.AndAlso, params P[] values)
        {
            return NotIn<T, P>(q, Property<T>(q, property), concat, values);
        }
        #endregion

        #region Fuzzy
        /// <summary>
        /// 建立 Fuzzy 查询条件
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="q">动态查询条件创建者</param>
        /// <param name="property">属性</param>
        /// <param name="expression">查询表达式（支持：1,2,3 或 1-3；如果不符合前面规则，即认为模糊查询；忽略空格；）</param>
        /// <returns></returns>
        public static Expression Fuzzy<T>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, string>> property, string expression, ConcatType concat)
        {
            if (string.IsNullOrEmpty(expression)) return null;
            expression = expression.Trim();
            List<string> valueList = new List<string>();
            string[] splits = expression.Split(',');
            if (splits.Length > 1)
            {
                splits.ForEach(c =>
                {
                    string el = c.Trim();
                    if (el.Length > 0)
                    {
                        valueList.Add(el);
                    }
                });

                return q.In<T, string>(property, concat, valueList.ToArray());
            }

            int index_minus = expression.IndexOf('-');
            if (index_minus > 0 && index_minus < expression.Length - 1)
            {
                string left = expression.Substring(0, index_minus).Trim();
                string right = expression.Substring(index_minus + 1).Trim();
                return q.Between<T>(property, left, right, concat);
            }

            return q.Like(property, expression, concat);
        }
        public static Expression Fuzzy<T>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, string expression, ConcatType concat = ConcatType.AndAlso)
        {
            if (string.IsNullOrEmpty(expression)) return null;
            expression = expression.Trim();
            List<string> valueList = new List<string>();
            string[] splits = expression.Split(',');
            if (splits.Length > 1)
            {
                splits.ForEach(c =>
                {
                    string el = c.Trim();
                    if (el.Length > 0)
                    {
                        valueList.Add(el);
                    }
                });

                return q.In<T, string>(property, concat, valueList.ToArray());
            }

            int index_minus = expression.IndexOf('-');
            if (index_minus > 0 && index_minus < expression.Length - 1)
            {
                string left = expression.Substring(0, index_minus).Trim();
                string right = expression.Substring(index_minus + 1).Trim();
                return q.Between<T>(property, left, right, concat);
            }

            return q.Like<T>(property, expression, concat);
        }
        public static Expression Fuzzy<T>(this ILambdaExpressionBuilder<T> q, string propertyName, string expression, ConcatType concat = ConcatType.AndAlso)
        {
            return Fuzzy<T>(q, Property<T>(q, propertyName), expression, concat);
        }

        public static Expression NotFuzzy<T>(this ILambdaExpressionBuilder<T> q, LambdaExpression property, string expression, ConcatType concat = ConcatType.AndAlso)
        {
            if (string.IsNullOrEmpty(expression)) return null;
            expression = expression.Trim();
            List<string> valueList = new List<string>();
            string[] splits = expression.Split(',');
            if (splits.Length > 1)
            {
                splits.ForEach(c =>
                {
                    string el = c.Trim();
                    if (el.Length > 0)
                    {
                        valueList.Add(el);
                    }
                });

                return q.NotIn<T, string>(property, concat, valueList.ToArray());
            }

            int index_minus = expression.IndexOf('-');
            if (index_minus > 0 && index_minus < expression.Length - 1)
            {
                string left = expression.Substring(0, index_minus).Trim();
                string right = expression.Substring(index_minus + 1).Trim();
                return q.NotBetween<T>(property, left, right, concat);
            }
            return q.NotLike<T>(property, expression, concat);
        }
        public static Expression NotFuzzy<T>(this ILambdaExpressionBuilder<T> q, string propertyName, string expression, ConcatType concat = ConcatType.AndAlso)
        {
            return NotFuzzy<T>(q, Property<T>(q, propertyName), expression, concat);
        }
        public static Expression NotFuzzy<T>(this ILambdaExpressionBuilder<T> q, Expression<Func<T, string>> property, string expression, ConcatType concat)
        {
            if (string.IsNullOrEmpty(expression)) return null;
            expression = expression.Trim();
            List<string> valueList = new List<string>();
            string[] splits = expression.Split(',');
            if (splits.Length > 1)
            {
                splits.ForEach(c =>
                {
                    string el = c.Trim();
                    if (el.Length > 0)
                    {
                        valueList.Add(el);
                    }
                });

                return q.NotIn<T, string>(property, concat, valueList.ToArray());
            }

            int index_minus = expression.IndexOf('-');
            if (index_minus > 0 && index_minus < expression.Length - 1)
            {
                string left = expression.Substring(0, index_minus).Trim();
                string right = expression.Substring(index_minus + 1).Trim();
                return q.NotBetween<T>(property, left, right, concat);
            }

            return q.NotLike(property, expression, concat);
        }

        #endregion

    }
}
