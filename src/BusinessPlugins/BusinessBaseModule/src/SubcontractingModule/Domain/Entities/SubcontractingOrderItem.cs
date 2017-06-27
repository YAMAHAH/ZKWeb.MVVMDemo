using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.SalesModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SubcontractingModule.Domain.Entities
{
    /// <summary>
    /// 外包订单项目
    /// </summary>
    [ExportMany]
    public class SubcontractingOrderItem : IFullAudit<SubcontractingOrderItem, Guid>
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
        /// 产品ID
        /// </summary>
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        /// <summary>
        /// 外包订单
        /// </summary>
        public Guid SubcontractingOrderId { get; set; }
        public SubcontractingOrder SubcontractingOrder { get; set; }
        #endregion

        #region 销售订单行关联
        public Nullable<Guid> SaleOrderItemId { get; set; }
        public SaleOrderDetail SaleOrderItem { get; set; }
        #endregion
        public void Configure(IEntityMappingBuilder<SubcontractingOrderItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            //主从表
            nativeBuilder.HasOne(i => i.SubcontractingOrder)
                .WithMany(i => i.Items)
                .HasForeignKey(i => i.SubcontractingOrderId);
            //产品
            nativeBuilder.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId);
            //销售订单项目
            nativeBuilder.HasOne(i => i.SaleOrderItem)
                .WithMany()
                .HasForeignKey(i => i.SaleOrderItemId)
                .OnDelete(DeleteBehavior.Restrict);
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
