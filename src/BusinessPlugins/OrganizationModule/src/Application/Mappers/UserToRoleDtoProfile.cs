using BusinessPlugins.OrganizationModule.Application.Dtos;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.DtoToModelMap;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Application.Mappers
{
    /// <summary>
    /// 用户DTO和用户实体对应关系配置
    /// </summary>
    [ExportMany, SingletonReuse]
    public class UserToRoleDtoProfile : DtoToModelMapProfile<UserToRole, UserToRoleOutputDto, Guid>
    {
        public UserToRoleDtoProfile()
        {
            ForMember(r => r.Id, opt => opt.Map(m => m.Id))
                .ForMember(u => u.User, opt => opt.Map(m => m.From))
                .ForMember(u => u.Role, opt => opt.Map(m => m.To));
        }
    }
}
