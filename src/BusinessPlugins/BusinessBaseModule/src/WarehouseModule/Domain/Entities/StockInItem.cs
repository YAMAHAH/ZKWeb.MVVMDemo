using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.ProductionModule.Domain.Entities;
using BusinessPlugins.PurchaseModule.Domain.Entities;
using BusinessPlugins.SalesModule.Domain.Entities;
using BusinessPlugins.SubcontractingModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 入库Item
    /// </summary>
    [ExportMany]
    public class StockInItem : IFullAudit<StockInItem, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion

        #region 入库行基本信息
        /// <summary>
        /// 明细序号
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 收货数量delivery
        /// </summary>
        public double QuantityReceived { get; set; }
        /// <summary>
        /// 栋配数量
        /// </summary>
        public double PickingQuantity { get; set; }
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
        public string BatchNo { get; set; }
        /// <summary>
        /// 货位 goods allocation
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion
        #region 依赖对象引用
        /// <summary>
        /// 入库单头ID
        /// </summary>
        public Guid StockInId { get; set; }
        public StockIn StockIn { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public Guid ProductId { get; set; }

        public Product Product { get; set; }
        #endregion
        #region 依赖对象集合引用

        #endregion

        #region 销售订单行关联
        public Nullable<Guid> SaleOrderItemId { get; set; }
        public SaleOrderItem SaleOrderItem { get; set; }
        #endregion
        #region 生产订单行关联
        public Nullable<Guid> ProductionOrderItemId { get; set; }
        public ProductionOrderItem ProductionOrderItem { get; set; }
        #endregion
        #region 采购订单行关联
        public Nullable<Guid> PurchaseOrderItemId { get; set; }
        public PurchaseOrderItem PurchaseOrderItem { get; set; }
        #endregion
        #region 外包订单行关联

        public Nullable<Guid> SubcontractingOrderItemId { get; set; }
        public SubcontractingOrderItem SubcontractingOrderItem { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<StockInItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            //主从表
            nativeBuilder.HasOne(i => i.StockIn)
                .WithMany(i => i.Items)
                .HasForeignKey(ii => ii.StockInId);
            //当前物料
            nativeBuilder.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            //销售订单
            nativeBuilder.HasOne(i => i.SaleOrderItem)
                .WithMany()
                .HasForeignKey(i => i.SaleOrderItemId)
                .OnDelete(DeleteBehavior.Restrict);

            //生产订单
            nativeBuilder.HasOne(i => i.ProductionOrderItem)
                .WithMany()
                .HasForeignKey(i => i.ProductionOrderItemId)
                .OnDelete(DeleteBehavior.Restrict);
            //采购订单
            nativeBuilder.HasOne(i => i.PurchaseOrderItem)
                .WithMany()
                .HasForeignKey(i => i.PurchaseOrderItemId)
                .OnDelete(DeleteBehavior.Restrict);
            //外包订单
            nativeBuilder.HasOne(i => i.SubcontractingOrderItem)
                .WithMany()
                .HasForeignKey(i => i.SubcontractingOrderItemId)
                .OnDelete(DeleteBehavior.Restrict);
           
        }
        #endregion

    }
}
