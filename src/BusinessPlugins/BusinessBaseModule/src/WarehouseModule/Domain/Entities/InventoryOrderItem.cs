using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 库存订单项
    /// </summary>
    [ExportMany]
    public class InventoryOrderItem : IFullAudit<InventoryOrderItem, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion

        #region 库存订单基本属性
        /// <summary>
        /// 子号码
        /// </summary>
        public string ChildNumber { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime NeedDate { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public Decimal Quantity { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 换算率
        /// </summary>
        public decimal UnitRate { get; set; }
        /// <summary>
        /// 单重
        /// </summary>
        public decimal SingleWeight { get; set; }

        /// <summary>
        /// 明细行备注
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
        /// 产品版本
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
        public Nullable<Guid> StorLocId { get; set; }
        public StorageLocation StorageLocation { get; set; }
        /// <summary>
        /// 库存申请单
        /// </summary>
        public Guid StockOrdId { get; set; }
        public InventoryOrder InventoryOrder { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNumber { get; set; }

        public Nullable<Guid> StorageBinId { get; set; }
        /// <summary>
        /// 储位
        /// </summary>
        public StoragePosition StorageBin { get; set; }
        /// <summary>
        /// 主需求计划行
        /// </summary>
        public MdsItem MdsItem { get; set; }

        #endregion

        #region 实体关系配置
        public virtual void Configure(IEntityMappingBuilder<InventoryOrderItem> builder)
        {
            var nbuidler = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(g => g.OwnerTenant, g => g.OwnerTenantId);
            //工厂
            builder.HasMany(g => g.Plant, g => g.PlantId);
            //库存地点
            builder.HasMany(g => g.StorageLocation, g => g.StorLocId);
            //储位
            builder.HasMany(g => g.StorageBin, g => g.StorageBinId);
            //库存订单
            builder.HasMany(i => i.InventoryOrder, r => r.Items, i => i.StockOrdId);
            //产品版次
            builder.HasMany(i => i.ProductVersion, i => i.ProdVerId);
            //产品特性值
            builder.HasOne(i => i.ProdFeatValGrp, i => i.ProdFeatValGrpId);
        }
        #endregion

    }
}
