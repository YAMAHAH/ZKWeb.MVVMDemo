using AutoMapper;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Components.DtoToModelMap;
using InfrastructurePlugins.BaseModule.Components.Exceptions;
using InfrastructurePlugins.BaseModule.Components.QueryBuilder;
using InfrastructurePlugins.BaseModule.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWebStandard.Ioc;
using System.FastReflection;

namespace InfrastructurePlugins.BaseModule.Application.Mappers
{
    public class ColumnFilterMapperFactory<TEntity, TDto, TPrimaryKey>
         where TEntity : class, IEntity, IEntity<TPrimaryKey> where TDto : IOutputDto
    {
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
        protected IContainer Injector => ZKWeb.Application.Ioc;
        private LambdaExpression GetPropertyExpression(ParameterExpression paraExpr, string propertyName)
        {
            var props = propertyName.Split('.');
            Expression propExpr = paraExpr;
            foreach (var prop in props)
            {
                if (typeof(TEntity).FastGetProperty(prop) == null)
                {
                    throw new BadRequestException(typeof(TEntity).Name + "不支持指定属性:" + propertyName);
                }
                propExpr = propExpr.Property(prop);
            }
            return propExpr == null ? null : Expression.Lambda(propExpr, paraExpr);
        }

        public ColumnFilterMapperFactory(string name) : this()
        {

        }
        public ColumnFilterMapperFactory()
        {
            Mapper = CreateMapper();
        }
        public IMapper Mapper { get; set; }

        public IMapper CreateMapper()
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

        public Expression<Func<TEntity, bool>> CreateQueryExpression(params GridSearchColumnFilter[] columnFilters)
        {
            var root = new ColumnQueryCondition() { IsChildExpress = true };
            var colFilters = columnFilters.ToList();
            var cqconds = Mapper.Map<List<ColumnQueryCondition>>(colFilters);

            root.Childs.AddRange(cqconds);
            var expBuilder = new LambdaExpressionBuilder<TEntity>();
            var rootExpr = expBuilder.GenerateLambdaExpression(root);
            return rootExpr;
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
            var cqconds = Mapper.Map<List<ColumnQueryCondition>>(colFilters);

            root.Childs.AddRange(cqconds);
            var expBuilder = new LambdaExpressionBuilder<TEntity>();
            var rootExpr = expBuilder.GenerateLambdaExpression(root);
            return rootExpr;
        }
    }
}
