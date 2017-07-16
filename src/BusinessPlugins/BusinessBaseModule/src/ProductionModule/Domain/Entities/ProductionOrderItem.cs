using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.ProductionPlanModule.Domain.Entities;
using BusinessPlugins.SalesModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductionModule.Domain.Entities
{
    /// <summary>
    /// 生产订单项目
    /// </summary>
    [ExportMany]
    public class ProductionOrderItem : IFullAudit<ProductionOrderItem, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 生产订单行基本信息

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
        /// 生产数量
        /// </summary>
        public double ProductionQty { get; set; }
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
        /// 上线日期   ScheduleDate
        /// </summary>
        public DateTime OnlineDate { get; set; }
        /// <summary>
        /// 完工日期
        /// </summary>
        public DateTime CompletionDate { get; set; }
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
        /// 产品
        /// </summary>
        public Guid ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        /// <summary>
        /// 生产订单
        /// </summary>
        public Guid ProductionOrderId { get; set; }
        public ProductionOrder ProductionOrder { get; set; }

        public Nullable<Guid> PlanProductionOrderItemId { get; set; }
        public PlannedOrderItem PlanProductionOrderItem { get; set; }

        /// <summary>
        /// 工序(作业/流程)订单集合
        /// </summary>
        public List<ProcessOrderItem> ProcessOrderItems { get; set; }

        ///// <summary>
        ///// 销售订单行
        ///// </summary>
        //public Nullable<Guid> SalesOrderItemId { get; set; }
        //public SaleOrderItem SalesOrderItem { get; set; }
        /// <summary>
        /// 主需求计划行
        /// </summary>
        public Nullable<Guid> MdsItemId { get; set; }
        public MdsItem MdsItem { get; set; }

        #endregion


        public void Configure(IEntityMappingBuilder<ProductionOrderItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
            //生产订单行
            builder.HasMany(i => i.ProductionOrder, i => i.Items, i => i.ProductionOrderId);
            //产品
            builder.HasMany(i => i.ProductVersion, i => i.ProductVersionId);
            //计划生产订单行
            builder.HasMany(i => i.PlanProductionOrderItem, p => p.ProductionOrderItems, i => i.PlanProductionOrderItemId);

            //销售订单Item
            //builder.HasMany(i => i.SalesOrderItem, s => s.ProductionOrderItems, i => i.SalesOrderItemId);
            //MdsItem
            builder.HasMany(i => i.MdsItem, mdsItem => mdsItem.ProdOrdItems, i => i.MdsItemId);

            //计算列
            nativeBuilder.Property(i => i.RemainingQty)
                .HasComputedColumnSql("[ProductionQty] - [FinishQty]");
            nativeBuilder.Property(i => i.TaxAmount)
                .HasComputedColumnSql("[TaxPrice]*[ProductionQty]");
            nativeBuilder.Property(i => i.Amount)
              .HasComputedColumnSql("[Price]*[ProductionQty]");

            nativeBuilder.Property(i => i.Weight)
                .HasComputedColumnSql("[SingleWeight] * [ProductionQty] * [UnitRate]");
        }
    }
}
