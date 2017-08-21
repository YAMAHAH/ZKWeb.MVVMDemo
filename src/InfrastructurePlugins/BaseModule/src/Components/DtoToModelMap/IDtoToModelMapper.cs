using AutoMapper;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using ZKWeb.Database;

namespace InfrastructurePlugins.BaseModule.Components.DtoToModelMap
{
    public interface IDtoToModelMapper
    {
        IDtoToModelMapProfile<TModel, TDto, TPrimaryKey> GetDtoToModelMap<TModel, TDto, TPrimaryKey>()
           where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>;
        bool AddOrUpdateMap<TModel, TDto, TPrimaryKey>(IDtoToModelMapProfileBase dtoToModelMap)
          where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>;

        IMapper GetDtoMapper<TDto>() where TDto : IOutputDto;
        bool AddOrUpdateMapper<TDto>(IMapper dtoMapper) where TDto : IOutputDto;
    }
}
