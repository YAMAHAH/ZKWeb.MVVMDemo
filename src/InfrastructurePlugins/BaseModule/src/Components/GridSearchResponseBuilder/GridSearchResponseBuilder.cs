using AutoMapper;
using AutoMapper.Configuration;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Application.Services.Interfaces;
using InfrastructurePlugins.BaseModule.Components.DtoToModelMap;
using InfrastructurePlugins.BaseModule.Components.QueryBuilder;
using InfrastructurePlugins.BaseModule.Domain.Filters.Interfaces;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using InfrastructurePlugins.BaseModule.Domain.Uow.Extensions;
using InfrastructurePlugins.BaseModule.Domain.Uow.Interfaces;
using InfrastructurePlugins.BaseModule.Utils;
using System;
using System.Collections.Generic;
using System.FastReflection;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using ZKWeb.Database;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Components.GridSearchResponseBuilder
{
    public class DynamicOrdering
    {
        public LambdaExpression Selector;
        public bool Ascending;
        public string MethodName;
    }

    /// <summary>
    /// 表格搜索回应的构建器
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public class GridSearchResponseBuilder<TEntity, TDto, TPrimaryKey>
        where TEntity : class, IEntity, IEntity<TPrimaryKey> where TDto : IOutputDto
    {
        protected GridSearchRequestDto _request;
        protected IList<IEntityQueryFilter> _enableFilters;
        protected IList<Type> _disableFilters;
        protected IList<Expression<Func<TEntity, bool>>> _keywordConditions;
        protected IList<QueryFilterDelegate<TEntity>> _customQueryFilters;
        protected QueryFilterDelegate<TEntity> _customQuerySorter;
        protected IDictionary<string, QueryColumnFilterDelegate<TEntity, TPrimaryKey>> _customColumnFilters;

        protected IMapper mapper;

        protected IQueryable<TEntity> customQuery;

        private ParameterExpression xParaExpr;
        protected ParameterExpression ParaExpression
        {
            get
            {
                if (xParaExpr == null)
                {
                    var entityType = typeof(TEntity);
                    xParaExpr = Expression.Parameter(entityType, "e");
                }
                return xParaExpr;
            }
        }
        public GridSearchResponseBuilder(GridSearchRequestDto request)
        {
            _request = request;
            _enableFilters = new List<IEntityQueryFilter>();
            _disableFilters = new List<Type>();
            _keywordConditions = new List<Expression<Func<TEntity, bool>>>();
            _customQueryFilters = new List<QueryFilterDelegate<TEntity>>();
            _customQuerySorter = null;
            _customColumnFilters = new Dictionary<string, QueryColumnFilterDelegate<TEntity, TPrimaryKey>>();

            mapper = ColumnQueryFilterProfile();
        }

        public GridSearchResponseBuilder(GridSearchRequestDto request, IQueryable<TEntity> query) : this(request)
        {
            customQuery = query;
        }

        protected IContainer Injector => ZKWeb.Application.Ioc;

        private IDtoToModelMapProfile<TEntity, TDto, TPrimaryKey> DtoToModelProfile
        {
            get
            {
                var dtoToModelMapper = Injector.Resolve<IDtoToModelMapper>();
                var dtoMapProfile = dtoToModelMapper.GetDtoToModelMap<TEntity, TDto, TPrimaryKey>();
                return dtoMapProfile;
            }
        }

        private IMapper ColumnQueryFilterProfile()
        {
            var dtmMapper = Injector.Resolve<IDtoToModelMapper>();
            var dtmMapProfile = dtmMapper.GetDtoToModelMap<TEntity, TDto, TPrimaryKey>();
            dtmMapProfile = Injector.Resolve<IDtoToModelMapProfile<TEntity, TDto, TPrimaryKey>>();
            var dtmAutoMapper = dtmMapper.GetDtoMapper<TDto>();
            if (dtmAutoMapper == null)
            {
                var mapperConf = new MapperConfiguration(cfg => cfg.CreateMap<GridSearchColumnFilter, ColumnQueryCondition>()
                    .ForMember(m => m.SrcExpression, opt => opt.ResolveUsing(m =>
                    {
                        var dtoMapVal = dtmMapProfile?.GetMember(m.Column);
                        var cqExpr = dtoMapVal?.Expression;
                        if (dtoMapVal?.ColumnFilterWrapper != null)
                        {
                            cqExpr = dtoMapVal.ColumnFilterWrapper(m);
                        }
                        //创建实体
                        if (dtoMapVal == null)
                        {
                            cqExpr = GetPropertyExpression(ParaExpression, m.Column);
                            var val = dtmMapProfile.CreateMapValue(m.Column, cqExpr.ReturnType, cqExpr);
                            dtmMapProfile.AddOrUpdate(val.Column, val);
                        }
                        else if (cqExpr == null)
                        {
                            cqExpr = GetPropertyExpression(ParaExpression, m.Column);
                            dtoMapVal.Expression = cqExpr;
                            dtoMapVal.ColumnType = cqExpr.ReturnType;
                        }
                        return cqExpr;
                    }))
                    .ForMember(m => m.IsCustomColumnFilter, opt => opt.ResolveUsing(m =>
                    {
                        var dtoMapVal = dtmMapProfile?.GetMember(m.Column);
                        return dtoMapVal.IsCustomColumnFilter;
                    }))
                    .ForMember(m => m.PropertyName, opt => opt.MapFrom(m => m.Column))
                    .ForMember(m => m.Value1, opt => opt.ResolveUsing(m =>
                    {
                        if (m.Value != null)
                        {
                            return m.Value;
                        }
                        return m.Value1;
                    }))
                    .ForMember(m => m.Value2, opt => opt.ResolveUsing(m => typeof(TDto).FullName))
                    .ForMember(m => m.OpertionSymbol, opt => opt.ResolveUsing(m =>
                    {
                        if (m.OpertionSymbol == OpertionSymbol.None)
                        {
                            switch (m.MatchMode)
                            {
                                case GridSearchColumnFilterMatchMode.Default:
                                    return OpertionSymbol.Like;
                                case GridSearchColumnFilterMatchMode.EndsWith:
                                    return OpertionSymbol.EndsWith;
                                case GridSearchColumnFilterMatchMode.Equals:
                                    return OpertionSymbol.Equals;
                                case GridSearchColumnFilterMatchMode.In:
                                    return OpertionSymbol.In;
                                case GridSearchColumnFilterMatchMode.StartsWith:
                                    return OpertionSymbol.StartsWith;
                                default:
                                    break;
                            }
                        }
                        return m.OpertionSymbol;
                    }))
                    .ForMember(m => m.ProperyType, opt => opt.ResolveUsing(m =>
                    {
                        var dtoMapVal = dtmMapProfile?.GetMember(m.Column);

                        if (dtoMapVal == null)
                        {
                            var cqExpr = GetPropertyExpression(ParaExpression, m.Column);
                            dtoMapVal = dtmMapProfile.CreateMapValue(m.Column, cqExpr.ReturnType, cqExpr);
                            dtmMapProfile.AddOrUpdate(dtoMapVal.Column, dtoMapVal);
                        }
                        var propType = m.ProperyType ?? dtoMapVal.ColumnType;
                        return propType;
                    })));
                return mapperConf.CreateMapper();
            }
            return dtmAutoMapper;
        }

        /// <summary>
        /// 启用查询过滤器
        /// </summary>
        public virtual GridSearchResponseBuilder<TEntity, TDto, TPrimaryKey>
            EnableQueryFilter(IEntityQueryFilter filter)
        {
            _enableFilters.Add(filter);
            return this;
        }

        /// <summary>
        /// 禁用查询过滤器
        /// </summary>
        public virtual GridSearchResponseBuilder<TEntity, TDto, TPrimaryKey>
            DisableQueryFilter(Type type)
        {
            _disableFilters.Add(type);
            return this;
        }

        /// <summary>
        /// 指定过滤关键词时使用的成员
        /// </summary>
        public virtual GridSearchResponseBuilder<TEntity, TDto, TPrimaryKey>
            FilterKeywordWith<TMember>(Expression<Func<TEntity, TMember>> memberSelector)
        {
            var paramExpr = memberSelector.Parameters[0];
            List<dynamic> memberExprs;
            if (memberSelector.Body.NodeType == ExpressionType.New)
            {
                NewExpression newExpr = (NewExpression)memberSelector.Body;
                memberExprs = newExpr.Arguments.Select(propExpr =>
                 {
                     dynamic exprInfo = new
                     {
                         ReturnType = propExpr.Type,
                         Expr = propExpr
                     };
                     return exprInfo;
                 }).ToList();
            }
            else
            {
                dynamic exprInfo = new
                {
                    ReturnType = typeof(TMember),
                    Expr = memberSelector.Body
                };
                memberExprs = new List<dynamic>() { exprInfo };
            }
            foreach (dynamic bodyExpr in memberExprs)
            {
                Expression newBodyExpr;
                if (bodyExpr.ReturnType == typeof(string))
                {
                    newBodyExpr = Expression.Call(
                        bodyExpr.Expr,
                        typeof(string).FastGetMethod(nameof(string.Contains)),
                        Expression.Constant(_request.Keyword));
                }
                else
                {
                    newBodyExpr = Expression.Equal(
                        bodyExpr.Expr,
                        Expression.Constant(_request.Keyword));
                }
                var conditionExpr = Expression.Lambda<Func<TEntity, bool>>(newBodyExpr, paramExpr);
                FilterKeywordWithCondition(conditionExpr);
            }
            return this;
        }

        /// <summary>
        /// 指定过滤关键词时使用的条件
        /// </summary>
        public virtual GridSearchResponseBuilder<TEntity, TDto, TPrimaryKey>
            FilterKeywordWithCondition(Expression<Func<TEntity, bool>> conditionExpr)
        {
            _keywordConditions.Add(conditionExpr);
            return this;
        }

        /// <summary>
        /// 指定自定义的查询过滤函数
        /// </summary>
        public virtual GridSearchResponseBuilder<TEntity, TDto, TPrimaryKey>
            FilterQuery(QueryFilterDelegate<TEntity> filterFunc)
        {
            _customQueryFilters.Add(filterFunc);
            return this;
        }

        /// <summary>
        /// 指定自定义的查询排序函数
        /// </summary>
        public virtual GridSearchResponseBuilder<TEntity, TDto, TPrimaryKey>
            SortQuery(QueryFilterDelegate<TEntity> sortFunc)
        {
            _customQuerySorter = sortFunc;
            return this;
        }

        /// <summary>
        /// 指定自定义的列过滤函数
        /// </summary>
        public virtual GridSearchResponseBuilder<TEntity, TDto, TPrimaryKey>
            FilterColumnWith(string column, QueryColumnFilterDelegate<TEntity, TPrimaryKey> columnFilter)
        {
            _customColumnFilters[column] = columnFilter;
            return this;
        }

        /// <summary>
        /// 获取可在执行过程中应用过滤器设置的函数执行器
        /// </summary>
        /// <returns></returns>
        protected Func<Func<GridSearchResponseDto>, GridSearchResponseDto> GetScopeInvoker()
        {
            var uow = ZKWeb.Application.Ioc.Resolve<IUnitOfWork>();
            var scopeInvoker = new Func<Func<GridSearchResponseDto>, GridSearchResponseDto>(func => func());
            // 应用启用的过滤器
            foreach (var enableFilter in _enableFilters)
            {
                var oldScopeInvoker = scopeInvoker;
                scopeInvoker = func =>
                {
                    using (uow.EnableQueryFilter(enableFilter))
                    {
                        return oldScopeInvoker(func);
                    }
                };
            }
            // 应用禁用的过滤器
            foreach (var disableFilter in _disableFilters)
            {
                var oldScopeInvoker = scopeInvoker;
                scopeInvoker = func =>
                {
                    using (uow.DisableQueryFilter(disableFilter))
                    {
                        return oldScopeInvoker(func);
                    }
                };
            }
            // 开启uow，以防万一不是在应用服务中调用
            return func =>
            {
                using (uow.Scope())
                {
                    return scopeInvoker(func);
                }
            };
        }

        /// <summary>
        /// http://stackoverflow.com/questions/457316/combining-two-expressions-expressionfunct-bool
        /// </summary>
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

        /// <summary>
        /// 按关键词进行过滤
        /// 当关键值为空或者没有自定义条件,也没有全局配置条件就返回原有查询
        /// 自定义条件优先于全局配置条件
        /// </summary>
        protected virtual IQueryable<TEntity> ApplyKeywordFilter(IQueryable<TEntity> query)
        {
            //获取预定义的关键字模糊查询字段
            var globalKeywordFilter = DtoToModelProfile?.KeywordFilterExpression;
            // 无关键字或无过滤条件且全局配置为空时跳过
            if (string.IsNullOrEmpty(_request.Keyword) || (globalKeywordFilter == null && _keywordConditions.Count == 0))
            {
                return query;
            }
            //根据全局配置生成相应的关键字模糊查询条件
            if (_keywordConditions.Count == 0 && globalKeywordFilter != null) this.FilterKeywordWith(globalKeywordFilter);
            // 使用or合并所有表达式
            var firstCondition = _keywordConditions.First();
            var leftParamExpr = firstCondition.Parameters[0];
            var leftBodyExpr = firstCondition.Body;
            foreach (var condition in _keywordConditions.Skip(1))
            {
                var rightParamExpr = condition.Parameters[0];
                var visitor = new ReplaceExpressionVisitor(rightParamExpr, leftParamExpr);
                var rightBodyExpr = visitor.Visit(condition.Body);
                leftBodyExpr = Expression.OrElse(leftBodyExpr, rightBodyExpr);
            }
            var predicate = Expression.Lambda<Func<TEntity, bool>>(leftBodyExpr, leftParamExpr);
            return query.Where(predicate);
        }

        /// <summary>
        /// 按列查询条件进行过滤
        /// </summary>
        protected virtual IQueryable<TEntity> ApplyColumnFilterOld(IQueryable<TEntity> query)
        {
            // 无列查询条件时跳过
            if (_request.ColumnFilters == null || _request.ColumnFilters.Count == 0)
            {
                return query;
            }
            // 按列查询条件进行过滤
            var entityType = typeof(TEntity);
            foreach (var columnFilter in _request.ColumnFilters)
            {
                // 有自定义过滤器时使用自定义过滤器
                QueryColumnFilterDelegate<TEntity, TPrimaryKey> customColumnFilter;
                if (_customColumnFilters.TryGetValue(columnFilter.Column, out customColumnFilter))
                {
                    query = customColumnFilter(columnFilter, query);
                    continue;
                }
                // 获取属性，时间或枚举列可能以Text结尾需要去掉再试
                var filterProperty = entityType.FastGetProperty(columnFilter.Column);
                if (filterProperty == null && columnFilter.Column.EndsWith("Text"))
                {
                    filterProperty = entityType.FastGetProperty(
                        columnFilter.Column.Substring(0, columnFilter.Column.Length - 4));
                }
                if (filterProperty == null)
                {
                    continue;
                }
                var propertyType = Nullable.GetUnderlyingType(filterProperty.PropertyType) ??
                    filterProperty.PropertyType;
                var isNullable = propertyType != filterProperty.PropertyType;
                // 构建过滤表达式
                var paramExpr = Expression.Parameter(entityType, "e");
                var memberExpr = Expression.Property(paramExpr, filterProperty);
                var memberValueExpr = isNullable ?
                    Expression.Property(memberExpr, nameof(Nullable<int>.Value)) : memberExpr;
                Expression bodyExpr;
                if (propertyType == typeof(string))
                {
                    // 成员类型是字符串
                    var valueExpr = Expression.Constant(columnFilter.Value?.ToString());
                    if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.StartsWith)
                    {
                        // 以搜索值开始
                        bodyExpr = Expression.Call(
                            memberValueExpr, nameof(string.StartsWith), Type.EmptyTypes, valueExpr);
                    }
                    else if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.EndsWith)
                    {
                        // 以搜索值结尾
                        bodyExpr = Expression.Call(
                            memberValueExpr, nameof(string.EndsWith), Type.EmptyTypes, valueExpr);
                    }
                    else if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.Equals)
                    {
                        // 等于搜索值
                        bodyExpr = Expression.Equal(memberValueExpr, valueExpr);
                    }
                    else
                    {
                        // 包含搜索值
                        bodyExpr = Expression.Call(
                            memberValueExpr, nameof(string.Contains), Type.EmptyTypes, valueExpr);
                    }
                }
                else if (propertyType == typeof(DateTime))
                {
                    // 成员类型是时间
                    // 首先从传入值解析时间，解析失败时跳过
                    // 传入值可以是时间，也可以是{ start: 时间, end: 时间 }
                    DateTime startTime, endTime;
                    if (columnFilter.Value == null)
                    {
                        continue;
                    }
                    else if (columnFilter.Value is string)
                    {
                        if (!DateTime.TryParse((string)columnFilter.Value, out startTime))
                        {
                            continue;
                        }
                        endTime = startTime;
                    }
                    else if (columnFilter.Value is DateTime)
                    {
                        endTime = startTime = (DateTime)columnFilter.Value;
                    }
                    else
                    {
                        var dict = columnFilter.Value.ConvertOrDefault<IDictionary<string, object>>();
                        if (dict == null)
                        {
                            continue;
                        }
                        startTime = dict.GetOrDefault<DateTime>("start");
                        endTime = dict.GetOrDefault<DateTime>("end");
                    }
                    // 转换时区
                    startTime = startTime.FromClientTime();
                    endTime = endTime.FromClientTime().AddDays(1);
                    // 生成表达式
                    var startTimeExpr = Expression.Constant(startTime);
                    var endTimeExpr = Expression.Constant(endTime);
                    if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.StartsWith)
                    {
                        // 以搜索时间开始
                        bodyExpr = Expression.GreaterThanOrEqual(memberValueExpr, startTimeExpr);
                    }
                    else if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.EndsWith)
                    {
                        // 以搜索时间结尾
                        bodyExpr = Expression.LessThan(memberValueExpr, endTimeExpr);
                    }
                    else if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.Equals)
                    {
                        // 等于搜索时间(1天内)
                        bodyExpr = Expression.AndAlso(
                            Expression.GreaterThanOrEqual(memberValueExpr, startTimeExpr),
                            Expression.LessThan(memberValueExpr, Expression.Constant(startTime.AddDays(1))));
                    }
                    else
                    {
                        // 包含在搜索时间范围内
                        bodyExpr = Expression.AndAlso(
                            Expression.GreaterThanOrEqual(memberValueExpr, startTimeExpr),
                            Expression.LessThan(memberValueExpr, endTimeExpr));
                    }
                }
                else if (propertyType == typeof(sbyte) || propertyType == typeof(byte) ||
                  propertyType == typeof(short) || propertyType == typeof(ushort) ||
                  propertyType == typeof(int) || propertyType == typeof(uint) ||
                  propertyType == typeof(long) || propertyType == typeof(ulong) ||
                  propertyType == typeof(decimal))
                {
                    // 成员类型是数值
                    // 首先从传入值解析数值，解析失败时跳过
                    // 传入值可以是数值，也可以是"数值~数值"，也可以是{ start: 数值, end: 数值 }
                    object lower, upper;
                    if (columnFilter.Value == null)
                    {
                        continue;
                    }
                    lower = columnFilter.Value.ConvertOrDefault(propertyType, null);
                    if (lower != null)
                    {
                        upper = lower;
                    }
                    else if (columnFilter.Value?.ToString().Contains("~") ?? false)
                    {
                        var range = columnFilter.Value.ToString().Split('~');
                        lower = range[0].ConvertOrDefault(propertyType, null);
                        upper = range[1].ConvertOrDefault(propertyType, null);
                    }
                    else
                    {
                        var dict = columnFilter.Value.ConvertOrDefault<IDictionary<string, object>>();
                        if (dict == null)
                        {
                            continue;
                        }
                        lower = dict.GetOrDefault("start").ConvertOrDefault(propertyType, null);
                        upper = dict.GetOrDefault("end").ConvertOrDefault(propertyType, null);
                    }
                    if (lower == null || upper == null)
                    {
                        continue;
                    }
                    // 生成表达式
                    var lowerExpr = Expression.Constant(lower);
                    var upperExpr = Expression.Constant(upper);
                    if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.StartsWith)
                    {
                        // 以搜索数值开始
                        bodyExpr = Expression.GreaterThanOrEqual(memberValueExpr, lowerExpr);
                    }
                    else if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.EndsWith)
                    {
                        // 以搜索数值结尾
                        bodyExpr = Expression.LessThanOrEqual(memberValueExpr, upperExpr);
                    }
                    else if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.Equals)
                    {
                        // 等于搜索数值
                        bodyExpr = Expression.Equal(memberValueExpr, lowerExpr);
                    }
                    else
                    {
                        // 包含在搜索时间范围内
                        bodyExpr = Expression.AndAlso(
                            Expression.GreaterThanOrEqual(memberValueExpr, lowerExpr),
                            Expression.LessThanOrEqual(memberValueExpr, upperExpr));
                    }
                }
                else
                {
                    // 成员类型是其他
                    var value = columnFilter.Value.ConvertOrDefault(propertyType, null);
                    if (value == null)
                    {
                        continue;
                    }
                    var valueExpr = Expression.Constant(value);
                    if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.StartsWith)
                    {
                        // 以搜索值开始
                        bodyExpr = Expression.GreaterThanOrEqual(memberValueExpr, valueExpr);
                    }
                    else if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.EndsWith)
                    {
                        // 以搜索值结尾
                        bodyExpr = Expression.LessThanOrEqual(memberValueExpr, valueExpr);
                    }
                    else
                    {
                        // 等于搜索值
                        bodyExpr = Expression.Equal(memberValueExpr, valueExpr);
                    }
                }
                // 如果类型是nullable还要再加一层过滤避免出错
                if (isNullable)
                {
                    bodyExpr = Expression.AndAlso(
                        Expression.NotEqual(memberExpr, Expression.Constant(null)), bodyExpr);
                }
                // 根据构建出来的条件筛选
                var predicate = Expression.Lambda<Func<TEntity, bool>>(bodyExpr, paramExpr);
                query = query.Where(predicate);
            }
            return query;
        }

        protected virtual IQueryable<TEntity> ApplyColumnFilter(IQueryable<TEntity> query)
        {
            // 无列查询条件时跳过
            if (_request.ColumnFilters == null || _request.ColumnFilters.Count == 0)
            {
                return query;
            }
            //获取用户模板预设的过滤条件
            var userPresetFilterProv = ZKWeb.Application.Ioc.Resolve<IUserPresetTemplateFilterProvider>();
            var userPresetFilter = userPresetFilterProv.GetUserPresetFilter<TEntity>();

            // 按列查询条件进行过滤
            var entityType = typeof(TEntity);

            var root = new ColumnQueryCondition() { IsChildExpress = true };
            var cqconds = mapper.Map<List<ColumnQueryCondition>>(_request.ColumnFilters.ToList());

            root.Childs.AddRange(cqconds);
            var expBuilder = new LambdaExpressionBuilder<TEntity>();
            var rootExpr = expBuilder.GenerateLambdaExpression(root);
            Expression<Func<TEntity, bool>> columnFilterExpr = rootExpr;

            ////合并表达式
            var paramExpr = userPresetFilter.Parameters[0];
            var leftBodyExpr = userPresetFilter.Body;
            var rightParamExpr = columnFilterExpr.Parameters[0];
            var visitor = new ReplaceExpressionVisitor(rightParamExpr, paramExpr);
            var rightBodyExpr = visitor.Visit(columnFilterExpr.Body);
            var bodyExpr = Expression.AndAlso(leftBodyExpr, rightBodyExpr);
            var predicate = Expression.Lambda<Func<TEntity, bool>>(bodyExpr, paramExpr);

            return predicate == null ? query : query.Where(predicate);
        }

        /// <summary>
        /// 调用自定义的过滤函数
        /// </summary>
        protected virtual IQueryable<TEntity> ApplyCustomFilter(IQueryable<TEntity> query)
        {
            // 无自定义过滤函数时跳过
            if (_customQueryFilters == null || _customQueryFilters.Count == 0)
            {
                return query;
            }
            // 调用自定义的过滤函数
            foreach (var filter in _customQueryFilters)
            {
                query = filter(query);
            }
            return query;
        }

        /// <summary>
        /// 排序的泛型函数
        /// https://github.com/aspnet/EntityFramework/issues/6735
        /// </summary>
        protected IQueryable<TEntity> SortQueryGeneric<TProperty>(
            IQueryable<TEntity> query, PropertyInfo orderProperty, bool ascending)
        {
            var orderExprParam = Expression.Parameter(typeof(TEntity), "e");
            var orderExprMember = Expression.Property(orderExprParam, orderProperty);
            var orderExpr = Expression.Lambda<Func<TEntity, TProperty>>(orderExprMember, orderExprParam);
            if (ascending)
            {
                query = query.OrderBy(orderExpr);
            }
            else
            {
                query = query.OrderByDescending(orderExpr);
            }
            return query;
        }
        protected static readonly MethodInfo _sortQueryGenericMethod =
            typeof(GridSearchResponseBuilder<TEntity, TDto, TPrimaryKey>).FastGetMethod(
                nameof(SortQueryGeneric), BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// 按自定义的排序函数或者请求的排序字段进行排序
        /// </summary>
        protected virtual IQueryable<TEntity> ApplySorterOld(IQueryable<TEntity> query)
        {
            // 有自定义的排序函数时使用自定义的排序函数
            if (_customQuerySorter != null)
            {
                return _customQuerySorter(query);
            }
            // 按查询给出的列进行排序，如果列不存在则按Id进行排序
            var entityType = typeof(TEntity);
            PropertyInfo orderProperty = null;
            if (!string.IsNullOrEmpty(_request.OrderBy))
            {
                orderProperty = entityType.FastGetProperty(_request.OrderBy);
                if (orderProperty == null)
                {
                    orderProperty = entityType.FastGetProperty(_request.OrderBy + "Text");
                }
            }
            if (orderProperty == null)
            {
                orderProperty = entityType.FastGetProperty(nameof(IEntity<object>.Id));
            }
            query = (IQueryable<TEntity>)_sortQueryGenericMethod
                .MakeGenericMethod(orderProperty.PropertyType)
                .FastInvoke(this, query, orderProperty, _request.Ascending);
            return query;
        }

        private Expression BuildAny<TSource>(Expression memberExpr, string propName, Expression<Func<TSource, bool>> predicate)
        {
            var overload = typeof(Queryable).GetMethods().Single(
                      method => method.Name == "Any"
                              && method.IsGenericMethodDefinition
                              && method.GetGenericArguments().Length == 2
                              && method.GetParameters().Length == 2);

            var call = Expression.Call(
                overload,
                Expression.PropertyOrField(memberExpr, propName),
                predicate);

            return call;
        }
        private LambdaExpression GetPropertyExpression(ParameterExpression paraExpr, string propertyName)
        {
            var props = propertyName.Split('.');
            Expression propExpr = paraExpr;
            foreach (var prop in props)
            {
                propExpr = propExpr.Property(prop);
            }
            return Expression.Lambda(propExpr, paraExpr);
        }
        protected virtual IQueryable<TEntity> ApplySorter(IQueryable<TEntity> query)
        {
            // 有自定义的排序函数时使用自定义的排序函数
            if (_customQuerySorter != null)
            {
                return _customQuerySorter(query);
            }
            //获取前端传来的排序数据
            var entityType = typeof(TEntity);
            ParameterExpression paraExpr = Expression.Parameter(entityType, "e");
            var sortMetas = _request.MultiSortMeta;
            //从缓存中找到对应的字段的表达式,生成新的排序实体
            IList<DynamicOrdering> dynamicOrderings = sortMetas.Select(sortMeta =>
            {
                var colMapVal = DtoToModelProfile?.GetMember(sortMeta.Field);
                return new DynamicOrdering()
                {
                    Ascending = sortMeta.Order,
                    Selector = colMapVal?.Expression ?? GetPropertyExpression(paraExpr, sortMeta.Field),
                    MethodName = sortMetas.First() == sortMeta ?
                                 sortMeta.Order ? "OrderBy" : "OrderByDescending" :
                                 sortMeta.Order ? "ThenBy" : "ThenByDescending"
                };
            }).ToList();

            // 按查询给出的列进行排序，如果列不存在则按Id进行排序
            if (dynamicOrderings.Count == 0) return query.OrderBy(t => t.Id);

            object orderResult = query;
            foreach (var dynOrd in dynamicOrderings)
            {
                var propType = dynOrd.Selector.ReturnType;
                // Type delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), propType);
                // LambdaExpression lambda = Expression.Lambda(delegateType, dynOrd.Selector.Body, paraExpr);
                orderResult = typeof(Queryable).GetMethods().Single(
                       method => method.Name == dynOrd.MethodName
                               && method.IsGenericMethodDefinition
                               && method.GetGenericArguments().Length == 2
                               && method.GetParameters().Length == 2)
                       .MakeGenericMethod(typeof(TEntity), propType)
                       .Invoke(null, new object[] { orderResult, dynOrd.Selector });
            }
            return (IQueryable<TEntity>)orderResult;
        }
        /// <summary>
        /// 对查询进行分页
        /// </summary>
        protected virtual IQueryable<TEntity> ApplyPagination(IQueryable<TEntity> query)
        {
            var skipCount = _request.Page * _request.PageSize;
            if (skipCount > 0)
            {
                // ef core的bug，skip 0会导致排序失效
                query = query.Skip(skipCount); query.Any(t => Guid.Empty.Equals(t.Id));
            }
            return query.Take(_request.PageSize);
        }

        /// <summary>
        /// 创建搜索回应
        /// </summary>
        /// <returns></returns>
        public virtual GridSearchResponseDto ToResponse()
        {
            if (customQuery == null)
            {
                return GetScopeInvoker()(() =>
                {
                    // 获取领域服务
                    var domainService = ZKWeb.Application.Ioc.Resolve<IDomainService<TEntity, TPrimaryKey>>();
                    // 获取查询对象
                    return domainService.GetMany<GridSearchResponseDto>(query =>
                {
                    // 按关键词进行过滤
                    query = ApplyKeywordFilter(query);
                    // 按列查询条件进行过滤
                    query = ApplyColumnFilter(query);
                    // 调用自定义的过滤函数
                    query = ApplyCustomFilter(query);
                    // 按自定义的排序函数或者请求的排序字段进行排序
                    query = ApplySorter(query);
                    // 获取总数量
                    var count = query.LongCount();
                    // 对查询进行分页
                    query = ApplyPagination(query);
                    // 构建搜索回应
                    var records = query.ToList();
                    var dtos = records.Select(r => (object)Mapper.Map<TDto>(r)).ToList();
                    return new GridSearchResponseDto(count, dtos);
                });
                });
            }
            return ToResponse(customQuery);
        }

        private GridSearchResponseDto ToResponse(IQueryable<TEntity> query)
        {
            return GetScopeInvoker()(() =>
            {
                // 按关键词进行过滤
                query = ApplyKeywordFilter(query);
                // 按列查询条件进行过滤
                query = ApplyColumnFilter(query);
                // 调用自定义的过滤函数
                query = ApplyCustomFilter(query);
                // 按自定义的排序函数或者请求的排序字段进行排序
                query = ApplySorter(query);
                // 获取总数量
                var count = query.LongCount();
                // 对查询进行分页
                query = ApplyPagination(query);
                // 构建搜索回应
                var records = query.ToList();
                var dtos = records.Select(r => (object)Mapper.Map<TDto>(r)).ToList();
                return new GridSearchResponseDto(count, dtos);
            });
        }

    }
}
