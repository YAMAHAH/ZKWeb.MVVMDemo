using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.ProductionModule.Domain.Entities;
using BusinessPlugins.ProductionPlanModule.Domain.Entities;
using BusinessPlugins.PurchaseModule.Domain.Entities;
using BusinessPlugins.SubcontractModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    [ExportMany]
    public class SaleOrderItem : IFullAudit<SaleOrderItem, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion

        #region 订单行主数据属性

        /// <summary>
        /// 子订单号码
        /// 表内唯一
        /// 根据这个号码可以找出对应的项
        /// </summary>
        public string ChildNo { get; set; }
        /// <summary>
        /// 明细序号
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 单位换算率 
        /// =辅助单位/基本单位 1h=100pc
        /// </summary>
        public double UnitRate { get; set; }
        /// <summary>
        /// 单重
        /// </summary>
        public double SingleWeight { get; set; }

        /// <summary>
        /// 重量 
        /// weight = SingleWeight *  ProdctionQty*UnitRate
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// 销售订单数量
        /// </summary>
        public double SalesOrderQty { get; set; }
        /// <summary>
        /// 完成数量
        /// </summary>
        public double FinishQty { get; set; }
        /// <summary>
        /// 剩余数量 计算字段
        /// </summary>
        public double RemainQty { get; set; }
        /// <summary>
        /// 排程完成数量
        /// </summary>
        public decimal ScheduleFinishQty { get; set; }
        /// <summary>
        /// 排程剩余数量
        /// </summary>
        public decimal ScheduleRemainQty { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// 成本价格
        /// </summary>
        public double CostPrice { get; set; }
        /// <summary>
        /// 含税成本价
        /// </summary>
        public double TaxCostPrice { get; set; }
        /// <summary>
        /// 成本金额
        /// </summary>
        public double CostAmount { get; set; }
        /// <summary>
        /// 含税成本金额
        /// </summary>
        public double TaxCostAmount { get; set; }
        /// <summary>
        /// 金额 计算字段
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// 含税单价
        /// </summary>
        public double TaxPrice { get; set; }
        /// <summary>
        /// 含税金额 计算字段
        /// </summary>
        public double TaxAmount { get; set; }

        /// <summary>
        /// 完成率 计算字段
        /// </summary>
        public double FinishRate { get; set; }

        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime NeedDate { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsDone { get; set; }
        /// <summary>
        /// 是否取消
        /// </summary>
        public bool IsCancel { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion

        #region 依赖对象引用
        /// <summary>
        /// 产品版次
        /// </summary>
        public Guid ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        /// <summary>
        /// 产品特性值
        /// </summary>
        public Nullable<Guid> ProdFeatValGrpId { get; set; }

        public ProductFeature ProductFeatureValueGroup { get; set; }

        /// <summary>
        /// 销售订单BOM ID
        /// </summary>

        public Nullable<Guid> ProdOrdBomId { get; set; }

        public ProductOrderBom ProductOrderBom { get; set; }
        /// <summary>
        /// 销售订单抬头
        /// </summary>
        public Guid SaleOrderId { get; set; }
        public SaleOrder SaleOrder { get; set; }
        /// <summary>
        /// 主需求计划行
        /// </summary>
        public MdsItem MdsItem { get; set; }
        /// <summary>
        /// 预留
        /// </summary>
        public Reservation Reservation { get; set; }
        ///// <summary>
        ///// MPS行
        ///// </summary>
        //public List<MpsItem> MpsItems { get; set; } = new List<MpsItem>();
        ///// <summary>
        ///// MRP行
        ///// </summary>
        //public List<MrpItem> MrpItems { get; set; } = new List<MrpItem>();
        ///// <summary>
        ///// 计划生产订单行
        ///// </summary>
        //public List<PlannedOrderItem> PlannedOrderItems { get; set; }
        ///// <summary>
        ///// 计划生产订单物料行
        ///// </summary>
        //public List<PlannedOrderMaterialItem> PlannedOrderMaterialItems { get; set; }

        ///// <summary>
        ///// 生产订单行
        ///// </summary>
        //public List<ProductionOrderItem> ProductionOrderItems { get; set; }
        ///// <summary>
        ///// 生产订单物料行
        ///// </summary>
        //public List<ProductionMaterialItem> ProductionOrderMaterialItems { get; set; }
        ///// 采购申请行
        ///// </summary>
        //public List<PurchaseRequestItem> PurchaseRequestItems { get; set; }
        ///// <summary>
        ///// 采购申请物料行
        ///// </summary>
        //public List<PurchaseRequestMaterialItem> PurchaseRequestMaterialItems { get; set; }

        ///// <summary>
        ///// 采购订单行
        ///// </summary>
        //public List<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        ///// <summary>
        ///// 采购物料行
        ///// </summary>
        //public List<PurchaseMaterialItem> PurchaseMaterialItems { get; set; }

        //public List<SubcontractOrderItem> SubcontractOrderItems { get; set; }
        #endregion

        public void Configure(IEntityMappingBuilder<SaleOrderItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //SaleOrder
            builder.HasMany(i => i.SaleOrder, o => o.Items, i => i.SaleOrderId);

            //产品版次
            builder.HasMany(i => i.ProductVersion, i => i.ProductVersionId);
            //产品特性值
            builder.HasOne(i => i.ProductFeatureValueGroup, i => i.ProdFeatValGrpId);
            //生产订单BOM
            builder.HasMany(i => i.ProductOrderBom, i => i.ProdOrdBomId);

            //剩余数量
            nativeBuilder.Property(i => i.RemainQty)
              .HasComputedColumnSql("[SalesOrderQty] - [FinishQty]");
            //重量
            nativeBuilder.Property(i => i.Weight)
                .HasComputedColumnSql("[SingleWeight] * [SalesOrderQty] * [UnitRate]");
            //完成率
            nativeBuilder.Property(i => i.FinishRate)
                .HasComputedColumnSql("[FinishQty] / [SalesOrderQty]");

            nativeBuilder.Property(i => i.TaxAmount)
               .HasComputedColumnSql("[TaxPrice]*[SalesOrderQty]");
            nativeBuilder.Property(i => i.Amount)
              .HasComputedColumnSql("[Price]*[SalesOrderQty]");
            nativeBuilder.Property(i => i.TaxCostAmount)
              .HasComputedColumnSql("[TaxCostPrice]*[SalesOrderQty]");
            nativeBuilder.Property(i => i.CostAmount)
              .HasComputedColumnSql("[CostPrice]*[SalesOrderQty]");
        }
    }
}
