using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace InfrastructurePlugins.BaseModule.Utils
{
    internal class ParameterExpressionVisitor : ExpressionVisitor
    {
        public ParameterExpressionVisitor(ParameterExpression paramExpr)
        {
            this.ParameterExpression = paramExpr;
        }

        public ParameterExpression ParameterExpression { get; private set; }

        public Expression ReplaceParameter(Expression expr)
        {
            return this.Visit(expr);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            return this.ParameterExpression;
        }
    }
    public static class ExpressionHelper
    {
        public static Expression Parameter(Type type, string name)
        {
            return Expression.Parameter(type, name);
        }

        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> one)
        {
            var candidateExpr = one.Parameters[0];
            var body = Expression.Not(one.Body);
            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterExpressionVisitor(candidateExpr);
            var left = parameterReplacer.ReplaceParameter(one.Body);
            var right = parameterReplacer.ReplaceParameter(another.Body);
            var body = Expression.And(left, right);
            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterExpressionVisitor(candidateExpr);

            var leftExpr = parameterReplacer.ReplaceParameter(left.Body);
            var rightExpr = parameterReplacer.ReplaceParameter(right.Body);
            var body = leftExpr.Or(rightExpr);
            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var invokedExpression = Expression.Invoke(right, left.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(left.Body.OrElse(invokedExpression), left.Parameters);
        }
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var invokedExpression = Expression.Invoke(right, left.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(left.Body.AndAlso(invokedExpression), left.Parameters);
        }

        public static Expression True()
        {
            return Expression.Constant(true);
        }

        public static Expression False()
        {
            return Expression.Constant(false);
        }

        public static Expression AndAlso(this Expression left, Expression right)
        {
            return Expression.AndAlso(left, right);
        }
        public static Expression OrElse(this Expression left, Expression right)
        {
            return Expression.OrElse(left, right);
        }

        public static Expression Or(this Expression left, Expression right)
        {
            return Expression.Or(left, right);
        }
        public static Expression And(this Expression left, Expression right)
        {
            return Expression.And(left, right);
        }
        public static Expression Call(this Expression instance, string methodName, params Expression[] arguments)
        {
            return Expression.Call(instance, instance.Type.GetMethod(methodName), arguments);
        }
        public static Expression Property(this Expression expression, string propertyName)
        {
            return Expression.Property(expression, propertyName);
        }

        public static Expression<Func<T, TResult>> Property<T, TResult>(string paramName, string propertyName)
        {
            ParameterExpression paraExpr = Expression.Parameter(typeof(T), paramName);
            Expression propExpr = paraExpr.Property(propertyName);
            return propExpr.ToLambda<Func<T, TResult>>(paraExpr);
        }
        /// <summary>
        /// 创建大于表达式
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <returns></returns>
        public static Expression GreaterThan(this Expression left, Expression right)
        {
            return Expression.GreaterThan(left, right);
        }
        /// <summary>
        /// 大于或等于表达式
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression GreaterThanOrEqual(this Expression left, Expression right)
        {
            return Expression.GreaterThanOrEqual(left, right);
        }
        /// <summary>
        /// 小于或等于表达式
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression LessThanOrEqual(this Expression left, Expression right)
        {
            return Expression.LessThanOrEqual(left, right);
        }

        /// <summary>
        /// 小于表达式
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression LessThan(this Expression left, Expression right)
        {
            return Expression.LessThan(left, right);
        }
        /// <summary>
        /// 等于表达式
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression Equal(this Expression left, Expression right)
        {
            return Expression.Equal(left, right);
        }
        /// <summary>
        /// 不等于表达式
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression NotEqual(this Expression left, Expression right)
        {
            return Expression.NotEqual(left, right);
        }
        public static Expression Contains(this Expression left, string memberName, Expression right)
        {
            return Expression.Call(Expression.Property(left, memberName), typeof(string).GetMethod("Contains"), right);
        }

        public static Expression NotContains(this Expression left, string memberName, Expression right)
        {
            return Expression.Not(Expression.Call(Expression.Property(left, memberName), typeof(string).GetMethod("Contains"), right));
        }

        public static Expression<TDelegate> ToLambda<TDelegate>(this Expression body, params ParameterExpression[] parameters)
        {
            return Expression.Lambda<TDelegate>(body, parameters);
        }
    }
}
