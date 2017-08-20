using BusinessPlugins.OrganizationModule.Application.Dtos;
using BusinessPlugins.OrganizationModule.Application.Services;
using BusinessPlugins.OrganizationModule.Components.PrivilegeTranslators.Interfaces;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.DtoToModelMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Application.Mappers
{
    /// <summary>
    /// 角色DTO和角色实体对应关系配置
    /// </summary>
    [ExportMany]
    public class RoleDtoProfile : DtoToModelMapProfile<Role, RoleOutputDto, Guid>
    {
        public RoleDtoProfile()
        {
            BelongTo(typeof(RoleManageService));
            FilterKeywordWith(r => new { r.Name, r.Remark })
             .ForMember(r => r.CreateTime, opt => opt.Map(m => m.CreateTime.ToString()))
             .ForMember(r => r.UpdateTime, opt => opt.Map(m => m.UpdateTime.ToString()))
             .ForMember(r => r.Id, opt => opt.Map(m => m.Id))
             .ForMember(r => r.Name, opt => opt.Map(u => u.Name))
             .ForMember(r => r.OwnerTenantName, (opt) => opt.Map(t => t.OwnerTenant.Name))
             .ForMember(r => r.OwnerTenantId, opt => opt.Map(r => r.OwnerTenantId))
             .ForMember(r => r.Remark, opt => opt.Map(r => r.Remark))
             .ForMember(r => r.Deleted, opt => opt.Map(r => r.Deleted))
             .ForMember(r => r.PrivilegeNames, opt => opt.Map(r => GetPrivilegeNames(r)))
             .ForMember(r => r.Privileges, opt => opt.Map(r => r.GetPrivileges()))
             ;
        }

        private string GetPrivilegeNames(Role r)
        {
            var translator = ZKWeb.Application.Ioc.Resolve<IPrivilegeTranslator>();
            return string.Join(",", r.GetPrivileges().Select(p => translator.Translate(p)));
        }

    }
}
