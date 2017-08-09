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
        public ICreateDtoToModelMap<TSrc, TDest, TPrimaryKey> GetDtoToModelMap<TSrc, TDest, TPrimaryKey>()
            where TDest : IOutputDto where TSrc : class, IEntity, IEntity<TPrimaryKey>
        {
            var key = typeof(TDest).FullName;
            IDtoToModelMap value;
            xDtoToModelMaps.TryGetValue(key, out value);
            return value == null ? null : value as ICreateDtoToModelMap<TSrc, TDest, TPrimaryKey>;
        }
        public bool AddOrUpdateMap<TSrc, TDest, TPrimaryKey>(IDtoToModelMap dtoToModelMap)
         where TDest : IOutputDto where TSrc : class, IEntity, IEntity<TPrimaryKey>
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

        public CreateDtoToModelMap<TModel, TDto, TPrimaryKey> ForMember<TMember>(Expression<Func<TDto, TMember>> destMember,
            Expression<Func<TModel, TMember>> srcMember)
        {
            var prop = destMember.Body.ToString().Split('.')[1];
            var value = new DtoToModelMapValue<TModel, TPrimaryKey>() { Column = prop, Expression = srcMember };
            xDtoToModelMap.AddOrUpdate(prop, value, (src, val) => val = value);
            return this;
        }

        public CreateDtoToModelMap<TModel, TDto, TPrimaryKey> ForMember<TMember>(Expression<Func<TDto, TMember>> destMember,
           QueryColumnFilterDelegate<TModel, TPrimaryKey> customeFilter)
        {
            var prop = destMember.Body.ToString().Split('.')[1];
            var value = new DtoToModelMapValue<TModel, TPrimaryKey>() { Column = prop, ColumnFilter = customeFilter };
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

        public DtoToModelMapValue<TModel, TPrimaryKey> GetMember<TMember>(string destMember)
        {
            var propKey = destMember;
            DtoToModelMapValue<TModel, TPrimaryKey> expValue;
            xDtoToModelMap.TryGetValue(propKey, out expValue);
            return expValue;
        }
    }

    public interface ICreateDtoToModelMap<TSrc, TDest, TPrimaryKey> : IDtoToModelMap
        where TDest : IOutputDto where TSrc : class, IEntity, IEntity<TPrimaryKey>
    {
        CreateDtoToModelMap<TSrc, TDest, TPrimaryKey> ForMember<TMember>(
            Expression<Func<TDest, TMember>> destMember,
            Expression<Func<TSrc, TMember>> srcMember);
        CreateDtoToModelMap<TSrc, TDest, TPrimaryKey> ForMember<TMember>(
            Expression<Func<TDest, TMember>> destMember,
            QueryColumnFilterDelegate<TSrc, TPrimaryKey> customeFilter);
    }

    public interface IDtoToModelMap
    {
        string Name { get; set; }
    }

    public class DtoToModelMapValue<TModel, TPrimaryKey>
         where TModel : class, IEntity, IEntity<TPrimaryKey>
    {
        public string Column { get; set; }
        public Expression Expression { get; set; }
        public QueryColumnFilterDelegate<TModel, TPrimaryKey> ColumnFilter { get; set; }
    }

    public interface IDtoToModelMapper
    {
        ICreateDtoToModelMap<TSrc, TDest, TPrimaryKey> GetDtoToModelMap<TSrc, TDest, TPrimaryKey>()
           where TDest : IOutputDto where TSrc : class, IEntity, IEntity<TPrimaryKey>;
        bool AddOrUpdateMap<TSrc, TDest, TPrimaryKey>(IDtoToModelMap dtoToModelMap)
          where TDest : IOutputDto where TSrc : class, IEntity, IEntity<TPrimaryKey>;
    }
    public interface IValueResolver<in TSource, in TDestination, TDestMember>
    {
        TDestMember Resolve(TSource source, TDestination destination, TDestMember destMember);
    }
}
