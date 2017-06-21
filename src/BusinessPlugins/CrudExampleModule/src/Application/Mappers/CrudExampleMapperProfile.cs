using AutoMapper;
using BusinessPlugins.CrudExampleModule.Application.Dtos;
using BusinessPlugins.CrudExampleModule.Domain.Entities;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.CrudExampleModule.Application.Mappers
{
    /// <summary>
    /// AutoMapper的配置
    /// </summary>
    [ExportMany]
    public class CrudExampleMapperProfile : Profile
    {
        public CrudExampleMapperProfile()
        {
            CreateMap<ExampleData, ExampleDataOutputDto>();
            CreateMap<ExampleDataInputDto, ExampleData>();
        }
    }
}
