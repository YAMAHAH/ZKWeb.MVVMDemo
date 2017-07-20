using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.ProductionModule.Domain.Entities;
using BusinessPlugins.ProductionPlanModule.Domain.Entities;
using BusinessPlugins.PurchaseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 收货项
    /// </summary>
    [ExportMany]
    public class GoodsReceiptItem : IFullAudit<GoodsReceiptItem, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion
        #region 收货项目基本信息
        /// <summary>
        /// 收货数量
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 单位换算率 辅助单位/基本单位
        /// </summary>
        public double UnitRate { get; set; }
        /// <summary>
        /// 单重
        /// </summary>
        public double SingleWeight { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNumber { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        #endregion
        #region 依赖对象引用
        /// <summary>
        /// 工厂
        /// </summary>
        public Guid PlantId { get; set; }
        public Plant Plant { get; set; }
        /// <summary>
        /// 收货单抬头
        /// </summary>
        public Guid GoodsReceiptId { get; set; }
        public GoodsReceipt GoodsReceipt { get; set; }
        /// <summary>
        /// 产品版次
        /// </summary>
        public Guid ProdVerId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        /// <summary>
        /// 产品特性值
        /// </summary>
        public Nullable<Guid> ProdFeatValGrpId { get; set; }
        public ProductFeatureValueGroup ProdFeatValGrp { get; set; }
        /// <summary>
        /// 库存地点
        /// </summary>
        public Guid StorLocId { get; set; }
        /// <summaryStorageLocation
        /// 库存地点
        /// </summary>
        public StorageLocation StorageLocation { get; set; }
        /// <summary>
        /// 货位
        /// </summary>
        public Nullable<Guid> StorageBinId { get; set; }
        public StoragePosition StorageBin { get; set; }
        /// <summary>
        /// 生产订单
        /// </summary>
        public Nullable<Guid> MfdOrdItemId { get; set; }
        public ManufactureOrderItem MfdOrdItem { get; set; }
        /// <summary>
        /// 采购订单
        /// </summary>
        public Nullable<Guid> PurOrdItemId { get; set; }
        public PurchaseOrderItem PurOrdItem { get; set; }

        /// <summary>
        /// 主需求计划行
        /// </summary>
        public Nullable<Guid> MdsItemId { get; set; }
        public MdsItem MdsItem { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<GoodsReceiptItem> builder)
        {
            //主键
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(i => i.OwnerTenant, i => i.OwnerTenantId);
            //工厂
            builder.HasMany(d => d.Plant, d => d.PlantId);
            //收货单
            builder.HasMany(i => i.GoodsReceipt, d => d.Items, i => i.GoodsReceiptId);
            //产品版本
            builder.HasMany(i => i.ProductVersion, i => i.ProdVerId);
            //产品特性值
            builder.HasOne(i => i.ProdFeatValGrp, i => i.ProdFeatValGrpId);
            //库存地点
            builder.HasMany(g => g.StorageLocation, g => g.StorLocId);
            //货位
            builder.HasMany(g => g.StorageBin, g => g.StorageBinId);
            //采购订单
            builder.HasMany(i => i.PurOrdItem, i => i.PurOrdItemId);
            //生产订单
            builder.HasMany(i => i.MfdOrdItem, i => i.MfdOrdItemId);
            //主需求计划行
            builder.HasMany(i => i.MdsItem, mdsItem => mdsItem.GRItems, i => i.MdsItemId);
        }
        #endregion

    }
}
