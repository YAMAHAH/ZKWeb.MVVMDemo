using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SubcontractModule.Domain.Entities
{
    /// <summary>
    /// 外包订单项目
    /// </summary>
    [ExportMany]
    public class SubcontractOrderItem : IFullAudit<SubcontractOrderItem, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 外包订单行基本信息
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
        /// 金额
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// 外包数量
        /// </summary>
        public double OutsourcingQty { get; set; }
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
        /// 交货日期
        /// </summary>

        public DateTime DeliveryDate { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>

        public bool IsDone { get; set; }

        /// <summary>
        /// 是否取消
        /// </summary>

        public bool IsCancel { get; set; }
        /// <summary>
        /// 前制程
        /// </summary>
        public Nullable<Guid> PreprocessId { get; set; }
        public ProcessStep PreProcess { get; set; }
        /// <summary>
        /// 后制程
        /// </summary>
        public Nullable<Guid> PostprocessId { get; set; }
        public ProcessStep PostProcess { get; set; }
        /// <summary>
        /// 材质
        /// </summary>
        public string Texture { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion

        #region 依赖对象引用
        /// <summary>
        /// 产品
        /// </summary>
        public Nullable<Guid> ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        /// <summary>
        /// 外包订单
        /// </summary>
        public Guid SubcontractOrderId { get; set; }
        public SubcontractOrder SubcontractOrder { get; set; }
        /// <summary>
        /// 采购请求
        /// </summary>
        public Nullable<Guid> PurchaseRequestId { get; set; }
        public PlannedPurchase PurchaseRequest { get; set; }
        ///// <summary>
        ///// 销售订单行
        ///// </summary>
        //public Nullable<Guid> SaleOrderItemId { get; set; }
        //public SaleOrderItem SaleOrderItem { get; set; }
        #endregion
        public void Configure(IEntityMappingBuilder<SubcontractOrderItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            //主从表
            builder.HasMany(i => i.SubcontractOrder, i => i.Items, i => i.SubcontractOrderId);
            //产品
            builder.HasMany(i => i.ProductVersion, i => i.ProductVersionId);
            //销售订单行
            // builder.HasMany(i => i.SaleOrderItem, s => s.SubcontractOrderItems, i => i.SaleOrderItemId);

            //前制程
            nativeBuilder.HasOne(i => i.PreProcess)
                .WithMany()
                .HasForeignKey(i => i.PreprocessId)
                .OnDelete(DeleteBehavior.Restrict);

            //后制程
            nativeBuilder.HasOne(i => i.PostProcess)
                .WithMany()
                .HasForeignKey(i => i.PostprocessId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
