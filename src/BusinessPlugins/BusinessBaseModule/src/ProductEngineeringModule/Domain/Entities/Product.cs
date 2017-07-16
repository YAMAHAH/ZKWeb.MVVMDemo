using System;
using System.Collections.Generic;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;
using BusinessPlugins.OrganizationModule.Domain;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Entities
{
    /// <summary>
    /// 物料
    /// </summary>
    [ExportMany]
    public class Product : IFullAudit<Product, Guid>
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
        /// 当前库存
        /// </summary>
        public decimal CurrentStock { get; set; }
        /// <summary>
        /// 再订货点
        /// 再订货点 = 安全库存 + 提前期的预测需求
        /// 当库存水平低于再订货点时,发出订货建议请求
        /// </summary>
        public decimal ReorderPoint { get; set; }

        /// <summary>
        /// 是否虚拟件
        /// </summary>
        public bool IsPhantom { get; set; }
        #endregion

        #region 依赖对象引用
        public ProductMrpData MrpData { get; set; }
        public List<ProductVersion> ProductVersions { get; set; } = new List<ProductVersion>();
        #endregion


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
