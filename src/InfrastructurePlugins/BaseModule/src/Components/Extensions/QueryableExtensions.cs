using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Components.GridSearchResponseBuilder;
using System.Linq;
using ZKWeb.Database;

namespace InfrastructurePlugins.BaseModule.Components.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// 创建某个实体的映射项目
        /// </summary>
        /// <typeparam name="TSource">源实体</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ProjectionExpression<TSource> Project<TSource>(this IQueryable<TSource> source)
        {
            return new ProjectionExpression<TSource>(source);
        }
        /// <summary>
        /// 构建响应生成器
        /// </summary>
        /// <typeparam name="TEntity">领域实体</typeparam>
        /// <typeparam name="TDto">视图实体</typeparam>
        /// <typeparam name="TPrimaryKey">主键</typeparam>
        /// <param name="query">查询</param>
        /// <param name="request">请求</param>
        /// <returns></returns>
        public static GridSearchResponseBuilder<TEntity, TDto, TPrimaryKey> BuildResponse<TEntity, TDto, TPrimaryKey>(this IQueryable<TEntity> query, GridSearchRequestDto request)
           where TEntity : class, IEntity, IEntity<TPrimaryKey> where TDto : IOutputDto
        {
            return new GridSearchResponseBuilder<TEntity, TDto, TPrimaryKey>(request, query);
        }
    }
}
