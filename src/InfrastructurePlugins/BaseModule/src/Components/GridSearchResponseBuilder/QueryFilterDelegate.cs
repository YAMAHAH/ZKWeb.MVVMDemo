﻿using InfrastructurePlugins.BaseModule.Application.Dtos;
using System.Linq;
using ZKWeb.Database;

namespace InfrastructurePlugins.BaseModule.Components.GridSearchResponseBuilder
{
    /// <summary>
    /// 查询条件调用函数委托
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <param name="columnFilter"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public delegate bool QueryColumnFilterFunc<TEntity, TPrimaryKey>(
        TEntity entity,
        GridSearchColumnFilter columnFilter
    ) where TEntity : class, IEntity, IEntity<TPrimaryKey>;

    /// <summary>
    /// 列查询过滤委托
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <param name="columnFilter"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    public delegate IQueryable<TEntity> QueryColumnFilterDelegate<TEntity, TPrimaryKey>(
        GridSearchColumnFilter columnFilter,
        IQueryable<TEntity> query)
        where TEntity : class, IEntity, IEntity<TPrimaryKey>;
    /// <summary>
    /// 查询过滤委托
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>

    public delegate IQueryable<TEntity> QueryFilterDelegate<TEntity>(IQueryable<TEntity> query)
         where TEntity : class, IEntity;
}