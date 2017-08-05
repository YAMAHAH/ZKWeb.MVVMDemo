using BusinessPlugins.BasicModule.Application.Module;
using BusinessPlugins.BasicModule.Application.Services;
using BusinessPlugins.BasicModule.ModuleCatalogs;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.BaseModule.Domain.Entities.TypeTraits;
using InfrastructurePlugins.BaseModule.Domain.Filters.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Application.Filters
{
    /// <summary>
    /// 模板自定义条件查询
    /// </summary>
    [ExportMany]
    [ComponentFilter(typeof(BasicModuleCatalog), typeof(GlobalManagerModule), typeof(GlobalManageService), "模板自定义条件查询")]
    public class TemplateQueryFilter : IEntityQueryFilter
    {
        /// <summary>
        /// true: 查询已删除的对象
        /// false: 查询未删除的对象
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// 初始化
        /// 默认查询未删除的对象
        /// </summary>
        public TemplateQueryFilter()
        {

        }

        /// <summary>
        /// 过滤查询
        /// </summary>
        IQueryable<TEntity> IEntityQueryFilter.FilterQuery<TEntity, TPrimaryKey>(
            IQueryable<TEntity> query)
        {
            //根据userId + tempId + filterId ,查找对应的条件

            if (DeletedTypeTrait<TEntity>.HaveDeleted)
            {
                query = query.Where(e => ((IHaveDeleted)e).Deleted == Deleted);
            }
            return query;
        }

        /// <summary>
        /// 过滤查询条件
        /// </summary>
        Expression<Func<TEntity, bool>> IEntityQueryFilter.FilterPredicate<TEntity, TPrimaryKey>(
                Expression<Func<TEntity, bool>> predicate)
        {
            if (DeletedTypeTrait<TEntity>.HaveDeleted)
            {
                var paramExpr = predicate.Parameters[0];
                var memberExpr = Expression.Property(paramExpr, nameof(IHaveDeleted.Deleted));
                var body = Expression.AndAlso(
                    predicate.Body,
                    Expression.Equal(memberExpr, Expression.Constant(Deleted)));
                predicate = Expression.Lambda<Func<TEntity, bool>>(body, paramExpr);
            }
            return predicate;
        }
    }
}
