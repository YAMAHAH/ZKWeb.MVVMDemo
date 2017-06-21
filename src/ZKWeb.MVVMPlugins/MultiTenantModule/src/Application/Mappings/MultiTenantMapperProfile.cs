using AutoMapper;
using InfrastructurePlugins.MultiTenantModule.Application.Dtos;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.MultiTenantModule.Application.Mappings
{
    /// <summary>
    /// AutoMapper的配置
    /// </summary>
    [ExportMany]
    public class MultiTenantMapperProfile : Profile
    {
        public MultiTenantMapperProfile()
        {
            // 租户
            CreateMap<TenantInputDto, Tenant>();
            CreateMap<Tenant, TenantOutputDto>();
        }
    }
}
