using BusinessPlugins.OrganizationModule.Domain.Extensions;
using BusinessPlugins.OrganizationModule.Domain.Services;
using InfrastructurePlugins.BaseModule.Application.Extensions;
using InfrastructurePlugins.BaseModule.Application.Services.Interfaces;
using InfrastructurePlugins.BaseModule.Components.QueryBuilder;
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
    /// 用户预定的条件提供者
    /// </summary>
    [ExportMany, SingletonReuse]
    public class UserPresetTemplateFilterProvider : IUserPresetTemplateFilterProvider
    {
        public Expression<Func<TEntity, bool>> GetUserPresetFilter<TEntity>()
        {
            // 根据userId + tempId + filterId , tempId + filterId查找对应的条件
            var injector = ZKWeb.Application.Ioc;
            var sessionManager = injector.Resolve<SessionManager>();
            var session = sessionManager.FastGetSession();

            var user = session.GetUser();
            var empId = user?.EmployeeId ?? Guid.Empty;

            if (empId == Guid.Empty)
            {
                return e => true;
            }
            else
            {
                var tempId = HttpManager.CurrentContext.GetApiMethodInfo().ServiceId;
                //
                var empMan = injector.Resolve<IEmployeeManager>();
                //获取模板
                var userTempFilters = empMan.GetTemplateFilter(empId, tempId);
                var root = new ColumnQueryCondition() { IsChildExpress = true };
                var queryConds = userTempFilters.Select(f =>
                {
                    var qc = JsonConvert.DeserializeObject<ColumnQueryCondition>(f.ConditionJson);
                    qc.IsChildExpress = true;
                    qc.Concat = f.ConcatType == ConcatType.None ? ConcatType.OrElse : f.ConcatType;
                    return qc;
                });
                root.Childs.AddRange(queryConds);
                var expBuilder = new LambdaExpressionBuilder<TEntity>();
                return expBuilder.GenerateLambdaExpression(root);
            }
        }
    }
}
