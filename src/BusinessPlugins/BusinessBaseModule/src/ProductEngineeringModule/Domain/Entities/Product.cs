using System;
using System.Collections.Generic;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Entities
{
    /// <summary>
    /// 物料
    /// </summary>
    [ExportMany]
    public class Product :
        IEntity<Guid>,
        IHaveCreateTime,
        IHaveUpdateTime,
        IHaveDeleted,
        IHaveOwnerTenant,
        IEntityMappingProvider<Product>
    {
        public bool Deleted { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid Id { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }
        #region 物料基本信息
        /// <summary>
        /// 料号
        /// </summary>
        public string ProductNo { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string ProductDesc { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 物料类型
        /// </summary>
        public Guid MaterialTypeId { get; set; }
        public MaterialType MatrialType { get; set; }
        /// <summary>
        /// 物料组
        /// </summary>
        public Guid MaterialGroupId { get; set; }
        /// <summary>
        /// 经济订货量
        /// </summary>
        public double EconomicOrderQty { get; set; }
        /// <summary>
        /// 当前库存
        /// </summary>
        public double CurrentStock { get; set; }
        /// <summary>
        /// 安全库存量
        /// </summary>
        public double SafetyStock { get; set; }
        /// <summary>
        /// 最低库存量
        /// </summary>
        public double MinimumStock { get; set; }
        /// <summary>
        /// 是否客供料
        /// </summary>
        public bool IsConsignMaterial { get; set; }
        /// <summary>
        /// 是否虚拟件
        /// </summary>
        public bool IsPhantom { get; set; }
        #endregion

        public List<ProductVersion> ProductVersions { get; set; } = new List<ProductVersion>();

        public void Configure(IEntityMappingBuilder<Product> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            nativeBuilder.Property(p => p.ProductNo).IsRequired();
            nativeBuilder.Property(p => p.ProductName).IsRequired();
            nativeBuilder.Property(p => p.ProductDesc).IsRequired();
            nativeBuilder.Property(p => p.Unit).IsRequired();
        }
    }
}
