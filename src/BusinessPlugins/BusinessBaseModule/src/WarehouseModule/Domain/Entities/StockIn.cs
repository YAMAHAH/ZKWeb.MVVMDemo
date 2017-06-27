using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductionModule.Domain.Entities;
using BusinessPlugins.PurchaseModule.Domain.Entities;
using BusinessPlugins.SalesModule.Domain.Entities;
using BusinessPlugins.SubcontractingModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    [ExportMany]
    /// <summary>
    /// 入库单
    /// </summary>
    public class StockIn : IFullAudit<StockIn, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion

        #region 入库单信息
        /// <summary>
        /// 入库类型
        /// </summary>
        public string StockInType { get; set; }
        /// <summary>
        /// 登记日期
        /// </summary>
        public DateTime RecordDate { get; set; }
        /// <summary>
        /// 入库单号
        /// </summary>
        public string StockInCode { get; set; }
        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime StockInDate { get; set; }
        /// <summary>
        /// 源单据号
        /// </summary>
        public string OriginalTicketNo { get; set; }
        /// <summary>
        /// 单据备注
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
        /// 仓库ID
        /// </summary>
        public Guid WarehouseId { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse { get; set; }

        /// <summary>
        /// 伙伴ID
        /// </summary>
        public Nullable<Guid> PartnerId { get; set; }
        /// <summary>
        /// 合作伙伴
        /// </summary>
        public Partner Partner { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid DptmId { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public Department Department { get; set; }

        #region 销售订单
        public Nullable<Guid> SaleOrderId { get; set; }

        public SaleOrderHeader SaleOrder { get; set; }

        #endregion
        #region 生产订单
        public Nullable<Guid> ProductionOrderId { get; set; }

        public ProductionOrder ProductionOrder { get; set; }
        #endregion
        #region 采购订单

        public Nullable<Guid> PurchaseOrderId { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }
        #endregion
        #region 外包订单

        public Nullable<Guid> SubcontractingOrderId { get; set; }

        public SubcontractingOrder SubcontractingOrder { get; set; }
        #endregion

        #endregion

        #region 依赖对象集合引用
        /// <summary>
        /// 入库Item
        /// </summary>
        public List<StockInItem> Items { get; set; } = new List<StockInItem>();
        #endregion

        #region 实体关系配置
        public virtual void Configure(IEntityMappingBuilder<StockIn> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });
            ////仓库
            nativeBuilder.HasOne(i => i.Warehouse)
                .WithMany()
                .HasForeignKey(i => i.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);
            ////合作合伴
            nativeBuilder.HasOne(i => i.Partner)
                .WithMany()
                .HasForeignKey(i => i.PartnerId)
                .OnDelete(DeleteBehavior.Restrict);

            ////部门,部门删除时有引用时应该禁止删除
            nativeBuilder.HasOne(i => i.Department)
                .WithMany()
                .HasForeignKey(i => i.DptmId)
                .OnDelete(DeleteBehavior.Restrict);
            //销售订单
            nativeBuilder.HasOne(i => i.SaleOrder)
              .WithMany()
              .HasForeignKey(i => i.SaleOrderId)
              .OnDelete(DeleteBehavior.Restrict);

            //生产订单
            nativeBuilder.HasOne(i => i.ProductionOrder)
                .WithMany()
                .HasForeignKey(i => i.ProductionOrderId)
                .OnDelete(DeleteBehavior.Restrict);
            //采购订单
            nativeBuilder.HasOne(i => i.PurchaseOrder)
                .WithMany()
                .HasForeignKey(i => i.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);
            //外包订单
            nativeBuilder.HasOne(i => i.SubcontractingOrder)
                .WithMany()
                .HasForeignKey(i => i.SubcontractingOrderId)
                .OnDelete(DeleteBehavior.Restrict);
            //工厂
            nativeBuilder.HasOne(i => i.Plant)
              .WithMany()
              .HasForeignKey(i => i.PlantId)
              .OnDelete(DeleteBehavior.Restrict);
        }
        #endregion

    }
}
