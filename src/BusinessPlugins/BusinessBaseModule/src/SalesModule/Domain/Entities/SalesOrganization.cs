using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 销售组织
    /// </summary>
    [ExportMany]
    public class SalesOrganization : IFullAudit<SalesOrganization, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion
        #region 主数据属性

        #endregion


        #region 依赖对象引用
        public Guid CompanyCodeId { get; set; }
        public CompanyCode CompanyCode { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public List<Plant> Plants { get; set; }
        public List<SalesOrgToDistr> DistributionChannels { get; set; }
        public List<SalesOrgToDivision> SalesDivisions { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<SalesOrganization> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
            builder.HasMany(o => o.SalesDivisions);
            builder.HasMany(o => o.DistributionChannels);
            //公司代码
            nativeBuilder.HasOne(o => o.CompanyCode)
                .WithMany(c => c.SalesOrganizations)
                .HasForeignKey(o => o.CompanyCodeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        #endregion
    }
}
