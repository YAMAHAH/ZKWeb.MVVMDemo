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
        private ConcurrentDictionary<string, IDtoToModelMap> xDtoToModelMaps = new ConcurrentDictionary<string, IDtoToModelMap>();
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
        public ICreateDtoToModelMap<TModel, TDto, TPrimaryKey> GetDtoToModelMap<TModel, TDto, TPrimaryKey>()
            where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>
        {
            var key = typeof(TDto).FullName;
            IDtoToModelMap value;
            xDtoToModelMaps.TryGetValue(key, out value);
            return value == null ? null : value as ICreateDtoToModelMap<TModel, TDto, TPrimaryKey>;
        }

        public bool AddOrUpdateMap<TModel, TDto, TPrimaryKey>(IDtoToModelMap dtoToModelMap)
         where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>
        {
            if (dtoToModelMap == null) return false;
            var key = dtoToModelMap.Name;
            IDtoToModelMap value = xDtoToModelMaps.AddOrUpdate(key, dtoToModelMap, (k, v) => v = dtoToModelMap);
            return value != null;
        }
    }

    public class CreateDtoToModelMap<TModel, TDto, TPrimaryKey> : ICreateDtoToModelMap<TModel, TDto, TPrimaryKey>
        where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>
    {
        private ConcurrentDictionary<string, DtoToModelMapValue<TModel, TPrimaryKey>> xDtoToModelMap = new ConcurrentDictionary<string, DtoToModelMapValue<TModel, TPrimaryKey>>();

        public string Name { get; set; }

        public CreateDtoToModelMap()
        {
            this.Name = typeof(TDto).FullName;
        }
        public void Register()
        {
            var mapper = ZKWeb.Application.Ioc.Resolve<IDtoToModelMapper>();
            mapper.AddOrUpdateMap<TModel, TDto, TPrimaryKey>(this);
        }

        DtoMapOption<TModel, TPrimaryKey> mapOptions = new DtoMapOption<TModel, TPrimaryKey>();
        public CreateDtoToModelMap<TModel, TDto, TPrimaryKey> ForMember<TMember>(Expression<Func<TDto, TMember>> destMember,
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

    public interface ICreateDtoToModelMap<TModel, TDto, TPrimaryKey> : IDtoToModelMap
        where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>
    {
        CreateDtoToModelMap<TModel, TDto, TPrimaryKey> ForMember<TMember>(Expression<Func<TDto, TMember>> destMember,
        Action<DtoMapOption<TModel, TPrimaryKey>> optionAction);

        DtoToModelMapValue<TModel, TPrimaryKey> GetMember<TMember>(Expression<Func<TDto, TMember>> destMember);
        /// <summary>
        /// 根据名称获取相应的映射对象
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="destMember"></param>
        /// <returns></returns>
        DtoToModelMapValue<TModel, TPrimaryKey> GetMember(string destMember);
    }

    public interface IDtoToModelMap
    {
        string Name { get; set; }
    }

    public class DtoToModelMapValue<TModel, TPrimaryKey>
         where TModel : class, IEntity, IEntity<TPrimaryKey>
    {
        public string Column { get; set; }

        public Type ColumnType { get; set; }

        public Expression Expression { get; set; }

        public QueryColumnFilterDelegate<TModel, TPrimaryKey> ColumnFilter { get; set; }

        public QueryColumnFilterFunc<TModel, TPrimaryKey> ColumnFilterFunc { get; set; }

        public ComponentPropertyAttribute TemplateObjectInfo { get; set; }
    }

    public class DtoMapOption<TModel, TPrimaryKey> where TModel : class, IEntity, IEntity<TPrimaryKey>
    {
        public Expression Expression { get; set; }

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
        ICreateDtoToModelMap<TModel, TDto, TPrimaryKey> GetDtoToModelMap<TModel, TDto, TPrimaryKey>()
           where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>;
        bool AddOrUpdateMap<TModel, TDto, TPrimaryKey>(IDtoToModelMap dtoToModelMap)
          where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>;

        IMapper GetDtoMapper<TDto>() where TDto : IOutputDto;
        bool AddOrUpdateMapper<TDto>(IMapper dtoMapper) where TDto : IOutputDto;
    }
    public interface IValueResolver<in TSource, in TDestination, TDestMember>
    {
        TDestMember Resolve(TSource source, TDestination destination, TDestMember destMember);
    }
}
