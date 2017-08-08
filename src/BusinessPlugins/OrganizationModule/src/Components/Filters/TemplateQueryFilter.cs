using BusinessPlugins.BasicModule.Application.Module;
using BusinessPlugins.BasicModule.Application.Services;
using BusinessPlugins.BasicModule.ModuleCatalogs;
using BusinessPlugins.OrganizationModule.Domain.Extensions;
using BusinessPlugins.OrganizationModule.Domain.Services;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Application.Extensions;
using InfrastructurePlugins.BaseModule.Components.QueryBuilder;
using InfrastructurePlugins.BaseModule.Domain.Filters.Interfaces;
using InfrastructurePlugins.SessionStateModule.Domain.Services;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Linq.Expressions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace BusinessPlugins.OrganizationModule.Application.Components.Filters
{
    /// <summary>
    /// 模板自定义条件查询
    /// </summary>
    [ExportMany]
    [ComponentFilter(typeof(BasicModuleCatalog), typeof(GlobalManagerModule), typeof(GlobalManageService), "模板自定义条件查询")]
    public class TemplateQueryFilter : IEntityQueryFilter
    {
        /// <summary>
        /// 过滤查询
        /// </summary>
        IQueryable<TEntity> IEntityQueryFilter.FilterQuery<TEntity, TPrimaryKey>(
            IQueryable<TEntity> query)
        {
            //从缓存中读取
            var filterExp = GetUserFilter<TEntity>();
            if (filterExp != null)
            {
                query = query.Where(filterExp);
            }
            return query;
        }

        private Expression<Func<TEntity, bool>> GetUserFilter<TEntity>()
        {
            return null;
            //根据userId + tempId + filterId , tempId + filterId查找对应的条件
            //var injector = ZKWeb.Application.Ioc;
            //var sessionManager = injector.Resolve<SessionManager>();
            //var session = sessionManager.FastGetSession();

            //var user = session.GetUser();
            //var empId = user?.EmployeeId ?? Guid.Empty;

            //if (empId == Guid.Empty)
            //{
            //    return null;
            //}
            //else
            //{
            //    var tempId = HttpManager.CurrentContext.GetApiMethodInfo().ServiceId;
            //    //
            //    var empMan = injector.Resolve<IEmployeeManager>();
            //    //获取模板
            //    var userTempFilters = empMan.GetTemplateFilter(empId, tempId);
            //    var root = new QueryCondition() { IsChildExpress = true };
            //    var queryConds = userTempFilters.Select(f =>
            //    {
            //        var qc = JsonConvert.DeserializeObject<QueryCondition>(f.ConditionJson);
            //        qc.IsChildExpress = true;
            //        qc.Concat = ConcatType.OrElse;
            //        return qc;
            //    });
            //    root.Childs.AddRange(queryConds);
            //    var expBuilder = new LambdaExpressionBuilder<TEntity>();
            //    return expBuilder.GenerateLambdaExpression(root);
            //}
        }

        /// <summary>
        /// 过滤查询条件
        /// </summary>
        Expression<Func<TEntity, bool>> IEntityQueryFilter.FilterPredicate<TEntity, TPrimaryKey>(
                Expression<Func<TEntity, bool>> predicate)
        {
            var userFilterExpr = GetUserFilter<TEntity>();
            if (userFilterExpr != null)
            {
                var paramExpr = predicate.Parameters[0];
                var body = Expression.AndAlso(predicate.Body, userFilterExpr.Body);
                predicate = Expression.Lambda<Func<TEntity, bool>>(body, paramExpr);
            }
            return predicate;
        }
    }
}
