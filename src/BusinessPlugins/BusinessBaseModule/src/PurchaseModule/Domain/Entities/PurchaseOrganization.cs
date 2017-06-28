using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.PurchaseModule.Domain.Entities
{
    /// <summary>
    /// 采购组织
    /// 集团,不设置公司代码的采购组织就是集团级别的
    /// 公司代码可以有多个采购组织,一个采购组织只能设置一个公司代码
    /// 工厂可以有多个采购组织,采购组织可以分配到多个工厂,条件是没有为采购组织分配公司代码
    /// </summary>
    [ExportMany]
    public class PurchaseOrganization : IFullAudit<PurchaseOrganization, Guid>
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
        public string PurchaseOrgCode { get; set; }
        public string PurchaseOrgName { get; set; }
        public string Remark { get; set; }
        #endregion

        #region 依赖对象引用
        public Nullable<Guid> CompanyCodeId { get; set; }
        public CompanyCode CompanyCode { get; set; }

        public List<PurchaseOrganizationToPlant> Plants { get; set; }
        /// <summary>
        /// 采购项目
        /// </summary>
        public List<PurchaseOrderItem> Items { get; set; }
        #endregion


        public void Configure(IEntityMappingBuilder<PurchaseOrganization> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
            builder.HasMany(o => o.Plants);

            nativeBuilder.HasOne(po => po.CompanyCode)
                .WithMany(c => c.PurchaseOrganizations)
                .HasForeignKey(o => o.CompanyCodeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
