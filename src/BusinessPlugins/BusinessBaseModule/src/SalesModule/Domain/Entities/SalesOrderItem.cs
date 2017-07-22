using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 销售订单项
    /// </summary>
    [ExportMany]
    public class SalesOrderItem : IFullAudit<SalesOrderItem, Guid>
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

        public ProductFeatureValueGroup ProdFeatValGrp { get; set; }

        /// <summary>
        /// 销售订单BOM
        /// </summary>

        public Nullable<Guid> MfdOrdBomId { get; set; }

        public ManufactureBom MfdOrdBom { get; set; }
        /// <summary>
        /// 销售订单抬头
        /// </summary>
        public Guid SalesOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; }
        /// <summary>
        /// 主需求计划行
        /// </summary>
        public MdsItem MdsItem { get; set; }
        /// <summary>
        /// 预留
        /// </summary>
        public Reservation Reservation { get; set; }
        #endregion
        #region 实体对象关系配置
        public void Configure(IEntityMappingBuilder<SalesOrderItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //SaleOrder
            builder.HasMany(i => i.SalesOrder, o => o.Items, i => i.SalesOrderId);
            //产品版次
            builder.HasMany(i => i.ProductVersion, i => i.ProductVersionId);
            //产品特性值
            builder.HasOne(i => i.ProdFeatValGrp, i => i.ProdFeatValGrpId);
            //生产订单BOM
            builder.HasMany(i => i.MfdOrdBom, i => i.MfdOrdBomId);

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
        #endregion
    }
}
