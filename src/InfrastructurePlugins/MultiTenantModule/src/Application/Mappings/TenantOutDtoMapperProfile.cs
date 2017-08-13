using InfrastructurePlugins.BaseModule.Components.DtoToModelMap;
using InfrastructurePlugins.MultiTenantModule.Application.Dtos;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.MultiTenantModule.Application.Mappings
{
    [ExportMany]
    public class TenantOutDtoMapperProfile : CreateDtoToModelMap<Tenant, TenantOutputDto, Guid>
    {
        public TenantOutDtoMapperProfile()
        {
            ForMember(u => u.CreateTime, opt => opt.Map(m => m.CreateTime.ToString())).
            ForMember(u => u.Remark, opt => opt.Map(m => m.Remark)).
            ForMember(d => d.UpdateTime, opt => opt.Map(m => m.UpdateTime.ToString()).Map(m => { m.Editable = true; })).
            ForMember(d => d.IsMaster, opt => opt.Map(u => u.Id)).
            ForMember(d => d.Name, (opt) => opt.Map(t => t.Name));
        }
    }
}
