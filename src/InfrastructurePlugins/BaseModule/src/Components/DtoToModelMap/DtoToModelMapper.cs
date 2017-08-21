using AutoMapper;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using System;
using System.Collections.Concurrent;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Components.DtoToModelMap
{
    [ExportMany, SingletonReuse]
    public class DtoToModelMapper : IDtoToModelMapper
    {
        private ConcurrentDictionary<string, IDtoToModelMapProfileBase> xDtoToModelMaps = new ConcurrentDictionary<string, IDtoToModelMapProfileBase>();
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
            IDtoToModelMapProfileBase value;
            xDtoToModelMaps.TryGetValue(key, out value);
            //没有新增
            if (value == null)
            {
                value = new DtoToModelMapProfile<TModel, TDto, TPrimaryKey>();
                AddOrUpdateMap<TModel, TDto, TPrimaryKey>(value);
            }
            return value as IDtoToModelMapProfile<TModel, TDto, TPrimaryKey>;
        }
        public IDtoToModelMapProfileBase GetDtoToModelMap(Type dtoToModelMapType)
        {
            IDtoToModelMapProfileBase value;
            var key = dtoToModelMapType.FullName;
            xDtoToModelMaps.TryGetValue(key, out value);
            return (IDtoToModelMapProfileBase)value;
        }

        public bool AddOrUpdateMap<TModel, TDto, TPrimaryKey>(IDtoToModelMapProfileBase dtoToModelMap)
         where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>
        {
            if (dtoToModelMap == null) return false;
            var key = dtoToModelMap.Name;
            IDtoToModelMapProfileBase value = xDtoToModelMaps.AddOrUpdate(key, dtoToModelMap, (k, v) => v = dtoToModelMap);
            return value != null;
        }
    }
}
