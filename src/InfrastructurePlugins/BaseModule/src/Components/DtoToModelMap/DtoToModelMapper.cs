using AutoMapper;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Components.GridSearchResponseBuilder;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Components.DtoToModelMap
{
    [ExportMany, SingletonReuse]
    public class DtoToModelMapper : IDtoToModelMapper
    {
        private ConcurrentDictionary<string, IDtoToModelMapperBase> xDtoToModelMaps = new ConcurrentDictionary<string, IDtoToModelMapperBase>();
        private ConcurrentDictionary<string, IMapper> xDtoMapperMaps = new ConcurrentDictionary<string, IMapper>();
        public IMapper GetDtoMapper<TDto>()
            where TDto : IOutputDto
        {
            var key = typeof(TDto).FullName;
            IMapper value;
            xDtoMapperMaps.TryGetValue(key, out value);
            return value == null ? null : value;
        }

        public bool AddOrUpdateMapper<TDto>(IMapper dtoMapper)
             where TDto : IOutputDto
        {
            if (dtoMapper == null) return false;
            var key = typeof(TDto).FullName;
            IMapper value = xDtoMapperMaps.AddOrUpdate(key, dtoMapper, (k, v) => v = dtoMapper);
            return value != null;
        }
        public IDtoToModelMapProfile<TModel, TDto, TPrimaryKey> GetDtoToModelMap<TModel, TDto, TPrimaryKey>()
            where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>
        {
            var key = typeof(TDto).FullName;
            IDtoToModelMapperBase value;
            xDtoToModelMaps.TryGetValue(key, out value);
            return value == null ? null : value as IDtoToModelMapProfile<TModel, TDto, TPrimaryKey>;
        }

        public bool AddOrUpdateMap<TModel, TDto, TPrimaryKey>(IDtoToModelMapperBase dtoToModelMap)
         where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>
        {
            if (dtoToModelMap == null) return false;
            var key = dtoToModelMap.Name;
            IDtoToModelMapperBase value = xDtoToModelMaps.AddOrUpdate(key, dtoToModelMap, (k, v) => v = dtoToModelMap);
            return value != null;
        }
    }

    public class DtoToModelMapProfile<TModel, TDto, TPrimaryKey> : IDtoToModelMapProfile<TModel, TDto, TPrimaryKey>
        where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>
    {
        private ConcurrentDictionary<string, DtoToModelMapValue<TModel, TPrimaryKey>> xDtoToModelMap = new ConcurrentDictionary<string, DtoToModelMapValue<TModel, TPrimaryKey>>();

        public string Name { get; set; }

        public Expression<Func<TModel, object>> KeywordFilterExpression { get; set; }

        public DtoToModelMapProfile()
        {
            this.Name = typeof(TDto).FullName;
        }
        public void Register()
        {
            var mapper = ZKWeb.Application.Ioc.Resolve<IDtoToModelMapper>();
            mapper.AddOrUpdateMap<TModel, TDto, TPrimaryKey>(this);
        }

        DtoMapOption<TModel, TPrimaryKey> mapOptions = new DtoMapOption<TModel, TPrimaryKey>();
        public DtoToModelMapProfile<TModel, TDto, TPrimaryKey> ForMember<TMember>(Expression<Func<TDto, TMember>> destMember,
         Action<DtoMapOption<TModel, TPrimaryKey>> optionAction)
        {
            optionAction(mapOptions);
            var prop = destMember.Body.ToString().Split('.')[1];
            var value = new DtoToModelMapValue<TModel, TPrimaryKey>()
            {
                Column = prop,
                ColumnType = typeof(TMember),
                Expression = mapOptions.Expression,
                ColumnFilter = mapOptions.ColumnFilter,
                TemplateObjectInfo = mapOptions.TemplateObjectInfo,
                ColumnFilterFunc = mapOptions.ColumnFilterFunc
            };
            xDtoToModelMap.AddOrUpdate(prop, value, (k, v) => v = value);
            return this;
        }

        public DtoToModelMapProfile<TModel, TDto, TPrimaryKey> FilterKeywordWith(Expression<Func<TModel, object>> selector)
        {
            KeywordFilterExpression = selector;
            return this;
        }

        public DtoToModelMapValue<TModel, TPrimaryKey> GetMember<TMember>(Expression<Func<TDto, TMember>> destMember)
        {
            var propKey = destMember.Body.ToString().Split('.')[1];
            DtoToModelMapValue<TModel, TPrimaryKey> expValue;
            xDtoToModelMap.TryGetValue(propKey, out expValue);
            return expValue;
        }

        public DtoToModelMapValue<TModel, TPrimaryKey> GetMember(string destMember)
        {
            var propKey = destMember;
            DtoToModelMapValue<TModel, TPrimaryKey> expValue;
            xDtoToModelMap.TryGetValue(propKey, out expValue);
            return expValue;
        }
    }

    public interface IDtoToModelMapProfile<TModel, TDto, TPrimaryKey> : IDtoToModelMapperBase
        where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 获取outputDto的关键字过滤字段选择表达式树
        /// </summary>
        Expression<Func<TModel, object>> KeywordFilterExpression { get; set; }
        /// <summary>
        /// 配置outputDTO和Model字段的对应关系
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="destMember"></param>
        /// <param name="optionAction"></param>
        /// <returns></returns>
        DtoToModelMapProfile<TModel, TDto, TPrimaryKey> ForMember<TMember>(Expression<Func<TDto, TMember>> destMember,
        Action<DtoMapOption<TModel, TPrimaryKey>> optionAction);
        /// <summary>
        /// 设置TDTO的默认关键字过滤字段
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="selector">关键字过滤字段选择器</param>
        /// <returns></returns>
        DtoToModelMapProfile<TModel, TDto, TPrimaryKey> FilterKeywordWith(Expression<Func<TModel, object>> selector);
        /// <summary>
        /// 获取Dto和Model的映射器
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="destMember"></param>
        /// <returns></returns>
        DtoToModelMapValue<TModel, TPrimaryKey> GetMember<TMember>(Expression<Func<TDto, TMember>> destMember);
        /// <summary>
        /// 根据名称获取相应的映射对象
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="destMember"></param>
        /// <returns></returns>
        DtoToModelMapValue<TModel, TPrimaryKey> GetMember(string destMember);
    }

    public interface IDtoToModelMapperBase
    {
        string Name { get; set; }
        void Register();
    }

    public class DtoToModelMapValue<TModel, TPrimaryKey>
         where TModel : class, IEntity, IEntity<TPrimaryKey>
    {
        public string Column { get; set; }

        public Type ColumnType { get; set; }

        public LambdaExpression Expression { get; set; }

        public QueryColumnFilterDelegate<TModel, TPrimaryKey> ColumnFilter { get; set; }

        public QueryColumnFilterFunc<TModel, TPrimaryKey> ColumnFilterFunc { get; set; }

        public ComponentPropertyAttribute TemplateObjectInfo { get; set; }
    }

    public class DtoMapOption<TModel, TPrimaryKey> where TModel : class, IEntity, IEntity<TPrimaryKey>
    {
        public LambdaExpression Expression { get; set; }

        public QueryColumnFilterDelegate<TModel, TPrimaryKey> ColumnFilter { get; set; }

        public QueryColumnFilterFunc<TModel, TPrimaryKey> ColumnFilterFunc { get; set; }

        public ComponentPropertyAttribute TemplateObjectInfo { get; set; } = new ComponentPropertyAttribute();


        public DtoMapOption<TModel, TPrimaryKey> Map<TMember>(Expression<Func<TModel, TMember>> expression)
        {
            this.Expression = expression;
            return this;
        }
        public DtoMapOption<TModel, TPrimaryKey> Map(Expression<Func<TModel, GridSearchColumnFilter, bool>> expression)
        {
            this.Expression = expression;
            return this;
        }
        public DtoMapOption<TModel, TPrimaryKey> Map(QueryColumnFilterDelegate<TModel, TPrimaryKey> columnFilter)
        {
            ColumnFilter = columnFilter;
            return this;
        }
        public DtoMapOption<TModel, TPrimaryKey> Map(Action<ComponentPropertyAttribute> objectInfoAction)
        {
            objectInfoAction(TemplateObjectInfo);
            return this;
        }

        public DtoMapOption<TModel, TPrimaryKey> MapFunc(QueryColumnFilterFunc<TModel, TPrimaryKey> columnFilterFunc)
        {
            ColumnFilterFunc = columnFilterFunc;
            return this;
        }
    }
    public interface IDtoToModelMapper
    {
        IDtoToModelMapProfile<TModel, TDto, TPrimaryKey> GetDtoToModelMap<TModel, TDto, TPrimaryKey>()
           where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>;
        bool AddOrUpdateMap<TModel, TDto, TPrimaryKey>(IDtoToModelMapperBase dtoToModelMap)
          where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>;

        IMapper GetDtoMapper<TDto>() where TDto : IOutputDto;
        bool AddOrUpdateMapper<TDto>(IMapper dtoMapper) where TDto : IOutputDto;
    }
    public interface IValueResolver<in TSource, in TDestination, TDestMember>
    {
        TDestMember Resolve(TSource source, TDestination destination, TDestMember destMember);
    }
}
