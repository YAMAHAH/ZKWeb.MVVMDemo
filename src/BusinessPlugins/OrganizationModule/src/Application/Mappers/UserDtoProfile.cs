using BusinessPlugins.OrganizationModule.Application.Dtos;
using BusinessPlugins.OrganizationModule.Components.PrivilegeTranslators.Interfaces;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain.Extensions;
using InfrastructurePlugins.BaseModule.Components.DtoToModelMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Application.Mappers
{
    /// <summary>
    /// 用户DTO和用户实体对应关系配置
    /// </summary>
    [ExportMany]
    public class UserDtoProfile : DtoToModelMapProfile<User, UserOutputDto, Guid>
    {
        public UserDtoProfile()
        {
            FilterKeywordWith(u => new { u.Username, u.Remark })
             .ForMember(r => r.CreateTime, opt => opt.Map(m => m.CreateTime.ToString()))
             .ForMember(r => r.UpdateTime, opt => opt.Map(m => m.UpdateTime.ToString()))
             .ForMember(r => r.Id, opt => opt.Map(m => m.Id))
             .ForMember(r => r.Username, opt => opt.Map(u => u.Username))
             .ForMember(r => r.OwnerTenantName, (opt) => opt.Map(t => t.OwnerTenant.Name))
             .ForMember(r => r.OwnerTenantId, opt => opt.Map(r => r.OwnerTenantId))
             .ForMember(r => r.Remark, opt => opt.Map(r => r.Remark))
             .ForMember(r => r.Deleted, opt => opt.Map(r => r.Deleted))
             .ForMember(r => r.Type, opt => opt.Map(r => r.Type))
             .ForMember(r => r.Privileges, opt => opt.Map(r => r.GetPrivileges()))
             .ForMember(u => u.RoleIds, opt => opt.Map(u => u.Roles.Select(r => r.To.Id).ToList()))
             .ForMember(u => u.Roles, opt => opt.MapColumnFilterWrapper(c =>
             {
                 var roleIds = c.Value.ConvertOrDefault<IList<Guid>>();
                 
                 if (roleIds != null)
                 {
                     Expression<Func<User, bool>> expr = e => Regex.IsMatch(e.Id.ToString(), ".*") && e.Roles.Any(r => roleIds.Contains(r.To.Id));
                     return expr;
                 }
                 return u => false;
             }))
             ;
        }

        private string GetPrivilegeNames(Role r)
        {
            var translator = ZKWeb.Application.Ioc.Resolve<IPrivilegeTranslator>();
            return string.Join(",", r.GetPrivileges().Select(p => translator.Translate(p)));
        }

    }
}
