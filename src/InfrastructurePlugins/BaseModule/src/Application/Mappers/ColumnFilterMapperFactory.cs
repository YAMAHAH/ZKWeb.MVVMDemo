using AutoMapper;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Components.DtoToModelMap;
using InfrastructurePlugins.BaseModule.Components.Exceptions;
using InfrastructurePlugins.BaseModule.Components.QueryBuilder;
using InfrastructurePlugins.BaseModule.Utils;
using System.FastReflection;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Application.Mappers
{
    /// <summary>
    /// 查询条件映射配置工厂
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public class ColumnFilterMapperFactory<TEntity, TDto, TPrimaryKey>
         where TEntity : class, IEntity, IEntity<TPrimaryKey> where TDto : IOutputDto
    {
        protected IContainer Injector => ZKWeb.Application.Ioc;

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

        private LambdaExpression CreatePropertyExpression(ParameterExpression paraExpr, string propertyName)
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
                            cqExpr = CreatePropertyExpression(ParaExpression, m.Column);
                            var val = dtmMapProfile.CreateMapValue(m.Column, cqExpr.ReturnType, cqExpr);
                            dtmMapProfile.AddOrUpdate(val.Column, val);
                        }
                        else if (cqExpr == null)
                        {
                            var memberName = (dtoMapVal.Prefix + "." + m.Column).Trim();
                            cqExpr = CreatePropertyExpression(ParaExpression, memberName);
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
                    .ForMember(m => m.PropClassify, opt => opt.ResolveUsing(m =>
                    {
                        var dtoMapVal = dtmMapProfile?.GetMember(m.Column);
                        return dtoMapVal.PropertyClassify;
                    }))
                     .ForMember(m => m.PropClassify, opt => opt.ResolveUsing(m =>
                     {
                         var dtoMapVal = dtmMapProfile?.GetMember(m.Column);
                         return dtoMapVal.Prefix;
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
                            var cqExpr = CreatePropertyExpression(ParaExpression, m.Column);
                            dtoMapVal = dtmMapProfile.CreateMapValue(m.Column, cqExpr.ReturnType, cqExpr);
                            dtmMapProfile.AddOrUpdate(dtoMapVal.Column, dtoMapVal);
                        }
                        var propType = m.PropertyType ?? dtoMapVal.ColumnType;
                        return propType;
                    })));
                return mapperConf.CreateMapper();
            }
            return dtmAutoMapper;
        }
    }
}
