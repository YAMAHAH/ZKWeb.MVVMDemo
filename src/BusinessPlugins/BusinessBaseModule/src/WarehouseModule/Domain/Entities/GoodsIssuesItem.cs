using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.ProductionModule.Domain.Entities;
using BusinessPlugins.PurchaseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 发货项
    /// </summary>
    [ExportMany]
    public class GoodsIssuesItem : IFullAudit<GoodsIssuesItem, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion
        #region 发货项目基本信息
        /// <summary>
        /// 发货数量
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
        /// 发货单抬头
        /// </summary>
        public Guid GoodsIssuesId { get; set; }
        public GoodsIssues GoodsIssues { get; set; }
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
        /// 生产订单项
        /// </summary>
        public Nullable<Guid> MfdOrdItemId { get; set; }
        public ManufactureOrderItem MfdOrdItem { get; set; }
        /// <summary>
        /// 采购订单项
        /// </summary>
        public Nullable<Guid> PurOrdItemId { get; set; }
        public PurchaseOrderItem PurOrdItem { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<GoodsIssuesItem> builder)
        {
            //主键
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(i => i.OwnerTenant, i => i.OwnerTenantId);
            //工厂
            builder.HasMany(d => d.Plant, d => d.PlantId);
            //收货单
            builder.HasMany(i => i.GoodsIssues, d => d.Items, i => i.GoodsIssuesId);
            //产品版本
            builder.HasMany(i => i.ProductVersion, i => i.ProdVerId);
            //产品特性值
            builder.HasOne(i => i.ProdFeatValGrp, i => i.ProdFeatValGrpId);
            //库存地点
            builder.HasMany(g => g.StorageLocation, g => g.StorLocId);
            //货位
            builder.HasMany(g => g.StorageBin, g => g.StorageBinId);
            //采购订单项
            builder.HasMany(i => i.PurOrdItem, i => i.PurOrdItemId);
            //生产订单项
            builder.HasMany(i => i.MfdOrdItem, i => i.MfdOrdItemId);
        }
        #endregion

    }
}
