﻿using AutoMapper;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Application.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Components.QueryBuilder
{
    /// <summary>
    /// 查询表达式创建工厂
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public class ExpressionCreateFactory<TEntity, TDto, TPrimaryKey>
         where TEntity : class, IEntity, IEntity<TPrimaryKey> where TDto : IOutputDto
    {
        protected IContainer Injector => ZKWeb.Application.Ioc;

        protected IMapper xMapper;
        public IMapper Mapper { get { return xMapper; } }
        public ExpressionCreateFactory()
        {
            xMapper = new ColumnFilterMapperFactory<TEntity, TDto, TPrimaryKey>().CreateMapper();
        }

        /// <summary>
        /// 创建表达式
        /// </summary>
        /// <param name="columnFilters"></param>
        /// <returns></returns>
        public Expression<Func<TEntity, bool>> CreateQueryExpression(params GridSearchColumnFilter[] columnFilters)
        {
            var root = new ColumnQueryCondition() { IsChildExpress = true };
            var colFilters = columnFilters.ToList();
            var cqconds = xMapper.Map<List<ColumnQueryCondition>>(colFilters);

            root.Childs.AddRange(cqconds);
            var expBuilder = new LambdaExpressionBuilder<TEntity>();
            var rootExpr = expBuilder.GenerateLambdaExpression(root);
            return rootExpr;
        }

        /// <summary>
        /// 创建Lambda表达式
        /// </summary>
        /// <param name="columnFilters"></param>
        /// <returns></returns>
        public Func<TEntity, bool> CreateQueryLambda(params GridSearchColumnFilter[] columnFilters)
        {
            return CreateQueryExpression(columnFilters).Compile();
        }

        /// <summary>
        /// 创建子查询表达式
        /// </summary>
        /// <param name="columnFilters"></param>
        /// <returns></returns>
        public Expression<Func<TEntity, bool>> CreateChildQueryExpression(params GridSearchColumnFilter[] columnFilters)
        {
            var root = new ColumnQueryCondition() { IsChildExpress = true };
            var colFilters = columnFilters.Where(f => f.IsChildQuery).ToList();
            var cqconds = xMapper.Map<List<ColumnQueryCondition>>(colFilters);

            root.Childs.AddRange(cqconds);
            var expBuilder = new LambdaExpressionBuilder<TEntity>();
            var rootExpr = expBuilder.GenerateLambdaExpression(root);
            return rootExpr;
        }

        /// <summary>
        /// 创建子查询Lambda表达式
        /// </summary>
        /// <param name="columnFilters"></param>
        /// <returns></returns>
        public Func<TEntity, bool> CreateChildQueryLambda(params GridSearchColumnFilter[] columnFilters)
        {
            return CreateChildQueryExpression(columnFilters).Compile();
        }

        public LambdaExpressionBuilder<TEntity> CreateBuilder()
        {
            return new LambdaExpressionBuilder<TEntity>();
        }

    }
}
