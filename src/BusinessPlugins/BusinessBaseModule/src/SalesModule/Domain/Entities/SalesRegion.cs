using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 销售区域
    /// </summary>
    [ExportMany]
    public class SalesRegion : IFullAudit<SalesRegion, Guid>
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
        public string SalesRegionCode { get; set; }
        public string SalesRegionName { get; set; }
        #endregion
        #region 依赖对象引用
        public Guid SalesOrgId { get; set; }
        public SalesOrganization SalesOrganiation { get; set; }
        public Guid DistrId { get; set; }
        public DistributionChannel DistributionChannel { get; set; }
        public Guid SalesDivId { get; set; }
        public SalesDivision SalesDivision { get; set; }

        public List<SalesRegionToOffice> SalesOffices { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<SalesRegion> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
            builder.HasMany(r => r.SalesOffices);

            nativeBuilder.HasAlternateKey(r => new { r.SalesOrgId, r.DistrId, r.SalesDivId });
            nativeBuilder.HasOne(r => r.SalesOrganiation)
                .WithOne()
                .HasForeignKey<SalesRegion>(r => r.SalesOrgId)
                .OnDelete(DeleteBehavior.Restrict);
            nativeBuilder.HasOne(r => r.DistributionChannel)
                .WithOne()
                .HasForeignKey<SalesRegion>(r => r.DistrId)
                .OnDelete(DeleteBehavior.Restrict);
            nativeBuilder.HasOne(r => r.SalesDivision)
                .WithOne()
                .HasForeignKey<SalesRegion>(r => r.SalesDivId)
                .OnDelete(DeleteBehavior.Restrict);

        }
        #endregion
    }
}
