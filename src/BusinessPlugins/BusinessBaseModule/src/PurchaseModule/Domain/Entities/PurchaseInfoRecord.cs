using BusinessPlugins.BasicModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.PurchaseModule.Domain.Entities
{
    /// <summary>
    /// 采购价目表
    /// </summary>
    [ExportMany]
    public class PurchaseInfoRecord : IFullAudit<PurchaseInfoRecord, Guid>
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
        /// <summary>
        /// 起始批量
        /// </summary>
        public double StartRange { get; set; }
        /// <summary>
        /// 结束批量
        /// </summary>
        public double EndRange { get; set; }
        /// <summary>
        /// 最新生效日期
        /// 业务日期大于等于生效日期
        /// </summary>
        public Nullable<DateTime> FromDate { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 单位转换率
        /// </summary>
        public double UnitRate { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public double TaxRate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion

        #region 依赖对象引用

        /// <summary>
        /// 工厂
        /// </summary>
        public Nullable<Guid> PlantId { get; set; }
        public Plant Plant { get; set; }
        /// <summary>
        /// 采购组织
        /// </summary>
        public Nullable<Guid> PurOrgId { get; set; }
        public PurchaseOrganization PurchaseOrganization { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public Nullable<Guid> VendorId { get; set; }
        public Partner Vendor { get; set; }
        /// <summary>
        /// 产品版次
        /// </summary>
        public Nullable<Guid> ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }

        /// <summary>
        /// 产品特性值
        /// </summary>
        public Nullable<Guid> ProdFeatValGrpId { get; set; }
        public ProductFeatureValueGroup ProdFeatValGrp { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public Nullable<Guid> ProcessStepId { get; set; }
        public ProcessStep ProcessStep { get; set; }
        /// <summary>
        /// 货币
        /// </summary>
        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; }

        #endregion

        #region 实体配置
        public void Configure(IEntityMappingBuilder<PurchaseInfoRecord> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            //主键
            nativeBuilder.HasKey(p => p.Id)
               .HasName("PurInfoRecordId");

            builder.Map(p => p.Unit, new EntityMappingOptions() { Length = 10 });
            //租户
            builder.HasMany(p => p.OwnerTenant, p => p.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //采购组织
            builder.HasMany(r => r.PurchaseOrganization, r => r.PurOrgId);
            //厂商
            builder.HasMany(p => p.Vendor, p => p.VendorId);
            //产品
            builder.HasMany(p => p.ProductVersion, p => p.ProductVersionId);
            //产品特性值
            builder.HasOne(i => i.ProdFeatValGrp, i => i.ProdFeatValGrpId);
            //工序
            builder.HasMany(p => p.ProcessStep, p => p.ProcessStepId);
            //货币
            builder.HasMany(p => p.Currency, p => p.CurrencyId);

        }
        #endregion
    }
}
