using AutoMapper;
using BusinessPlugins.BasicModule.ModuleCatalogs;
using BusinessPlugins.OrganizationModule.Application.Module;
using BusinessPlugins.OrganizationModule.Components.ActionFilters;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain.Entities.Interfaces;
using BusinessPlugins.OrganizationModule.Domain.Entities.UserTypes;
using BusinessPlugins.OrganizationModule.Domain.Extensions;
using BusinessPlugins.OrganizationModule.Domain.Services;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Application.Services.Attributes;
using InfrastructurePlugins.BaseModule.Application.Services.Bases;
using InfrastructurePlugins.BaseModule.Components.Exceptions;
using InfrastructurePlugins.BaseModule.Domain.Filters;
using InfrastructurePlugins.BaseModule.Domain.Uow.Extensions;
using InfrastructurePlugins.MultiTenantModule.Application.Dtos;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.MultiTenantModule.Domain.Filters;
using InfrastructurePlugins.MultiTenantModule.Domain.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Application.Services
{
    /// <summary>
    /// 租户管理服务
    /// </summary>
    [ExportMany, SingletonReuse, Description("租户管理服务")]
    [ComponentClass(typeof(MultiTenantModule), typeof(OrganizationModuleCatalog), "租户管理", typeof(TenantOutputDto))]
    public class TenantManageService : ApplicationServiceBase
    {
        private TenantManager _tenantManager;
        private UserManager _userManager;

        public TenantManageService(TenantManager tenantManager, UserManager userManager)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
        }

        /// <summary>
        /// 更新Dto中的超级管理员名称
        /// </summary>
        private void UpdateSuperAdminName(IEnumerable<TenantOutputDto> tenants)
        {
            var tenantIds = tenants.Select(t => t.Id).ToList();
            var tenantToSuperAdmins = _userManager.GetSuperAdminsFromTenants(tenantIds);
            foreach (var tenant in tenants)
            {
                tenant.SuperAdminName = tenantToSuperAdmins.GetOrDefault(tenant.Id)?.Username;
            }
        }

        [Description("搜索租户"), ComponentMethod("View", "搜索", true, true)]
        [CheckPrivilege(true, typeof(IAmAdmin), "Tenant:View")]
        public GridSearchResponseDto Search(GridSearchRequestDto request)
        {

            var response = request.BuildResponse<Tenant, TenantOutputDto, Guid>()
                .FilterKeywordWith(t => t.Name)
                .FilterKeywordWith(t => t.Remark)
                .ToResponse();
            UpdateSuperAdminName(response.Result.OfType<TenantOutputDto>());
            return response;
        }

        [Description("编辑租户")]
        [CheckPrivilege(true, typeof(IAmAdmin), "Tenant:Edit")]
        [ComponentMethod("Edit", "编辑", true, false)]
        [UnitOfWork(IsTransactional = true)]
        public ActionResponseDto Edit(TenantInputDto dto)
        {
            var tenant = _tenantManager.Get(dto.Id) ?? new Tenant();
            Mapper.Map(dto, tenant);
            bool isNewTenant = tenant.Id == Guid.Empty;
            // 保存租户
            _tenantManager.Save(ref tenant);
            // 保存超级管理员
            using (UnitOfWork.DisableFilter(typeof(OwnerTenantFilter)))
            {
                var superAdmin = _userManager.Get(u =>
                    u.Username == dto.SuperAdminName &&
                    u.OwnerTenantId == tenant.Id) ?? new User();
                superAdmin.Type = SuperAdminUserType.ConstType;
                superAdmin.Username = dto.SuperAdminName;
                superAdmin.OwnerTenant = tenant;
                if (dto.SuperAdminPassword != dto.SuperAdminConfirmPassword)
                {
                    throw new BadRequestException("Confirm password not matched with password");
                }
                else if (!string.IsNullOrEmpty(dto.SuperAdminPassword))
                {
                    superAdmin.SetPassword(dto.SuperAdminPassword);
                }
                else if (superAdmin.Id == Guid.Empty)
                {
                    throw new BadRequestException("Please provider a password for new user");
                }
                _userManager.Save(ref superAdmin);
            }
            return ActionResponseDto.CreateSuccess("Saved Successfully");
        }

        [Description("删除租户")]
        [CheckPrivilege(true, typeof(IAmAdmin), "Tenant:Remove")]
        [ComponentMethod("Remove", "删除", true, false)]
        public ActionResponseDto Remove(Guid id)
        {
            if (_tenantManager.Count(x => x.Id == id && x.IsMaster) > 0)
            {
                throw new BadRequestException("You can't delete master tenant");
            }
            _tenantManager.Delete(id);
            return ActionResponseDto.CreateSuccess("Deleted Successfully");
        }
    }
}
