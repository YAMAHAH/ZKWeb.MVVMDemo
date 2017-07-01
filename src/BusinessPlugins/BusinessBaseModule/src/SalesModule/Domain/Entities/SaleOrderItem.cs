using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities.Interfaces;
using ZKWeb.ORM.EFCore;
using ZKWebStandard.Ioc;
using BusinessPlugins.OrganizationModule.Domain;
using Microsoft.EntityFrameworkCore;
using BusinessPlugins.ProductionPlanModule.Domain.Entities;
using System.Collections.Generic;

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
        public double RemainingQty { get; set; }
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
        /// 销售订单抬头
        /// </summary>
        public Guid SaleOrderId { get; set; }
        public SaleOrder SaleOrder { get; set; }
        /// <summary>
        /// MpsItems
        /// </summary>
        public List<MpsItem> MpsItems { get; set; } = new List<MpsItem>();
        /// <summary>
        /// MrpItems
        /// </summary>
        public List<MrpItem> MrpItems { get; set; } = new List<MrpItem>();

        #endregion

        public void Configure(IEntityMappingBuilder<SaleOrderItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            builder.HasMany(i => i.SaleOrder, o => o.Items, i => i.SaleOrderId);

            //产品版次
            builder.HasMany(i => i.ProductVersion, i => i.ProductVersionId);
            //剩余数量
            nativeBuilder.Property(i => i.RemainingQty)
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
