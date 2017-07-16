using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.ProductionPlanModule.Domain.Entities;
using BusinessPlugins.SalesModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.PurchaseModule.Domain.Entities
{
    /// <summary>
    /// 采购订单项目
    /// </summary>
    [ExportMany]
    public class PurchaseOrderItem : IFullAudit<PurchaseOrderItem, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 采购订单项目基本信息

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
        /// 单价
        /// </summary>
        public double Pirce { get; set; }

        /// <summary>
        /// 采购数量
        /// </summary>
        public double PurchaseQty { get; set; }
        /// <summary>
        /// 完成数量
        /// </summary>
        public double FinishQty { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public double RemainingQty { get; set; }
        /// <summary>
        /// 完成率
        /// </summary>
        public double FinishRate { get; set; }

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
        /// 产品ID
        /// </summary>
        public Guid ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        /// <summary>
        /// 采购订单抬头
        /// </summary>
        public Guid PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        ///// <summary>
        ///// 销售订单行
        ///// </summary>
        //public Nullable<Guid> SaleOrderItemId { get; set; }
        //public SaleOrderItem SaleOrderItem { get; set; }
        /// <summary>
        /// 主需求计划行
        /// </summary>
        public Nullable<Guid> MdsItemId { get; set; }
        public MdsItem MdsItem { get; set; }
        #endregion

        public void Configure(IEntityMappingBuilder<PurchaseOrderItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
            //主从表
            builder.HasMany(i => i.PurchaseOrder, s => s.Items, i => i.PurchaseOrderId);
            //产品
            builder.HasMany(i => i.ProductVersion, i => i.ProductVersionId);
            //销售订单

            //builder.HasMany(i => i.SaleOrderItem, s => s.PurchaseOrderItems, i => i.SaleOrderItemId);
            //MdsItem
            builder.HasMany(i => i.MdsItem, mdsItem => mdsItem.PurOrdItems, i => i.MdsItemId);

        }
    }
}
