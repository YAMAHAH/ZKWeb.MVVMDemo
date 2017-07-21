using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
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
    /// 采购申请项目
    /// </summary>
    [ExportMany]
    public class PurchaseRequisitionItem : IFullAudit<PurchaseRequisitionItem, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 采购申请项目基本信息

        /// <summary>
        /// 子订单号码
        /// </summary>
        public string ChildNo { get; set; }
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
        /// 需求日期
        /// </summary>
        public DateTime NeedDate { get; set; }
        /// <summary>
        /// 申请数量
        /// </summary>
        public decimal RequisitionQty { get; set; }
        /// <summary>
        /// 确认数量
        /// </summary>
        public decimal VaildationQty { get; set; }
        /// <summary>
        /// 完成数量
        /// </summary>
        public decimal FinishQty { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainingQty { get; set; }

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
        /// 产品特性值
        /// </summary>
        public Nullable<Guid> ProdFeatValGrpId { get; set; }

        public ProductFeatureValueGroup ProdFeatValGrp { get; set; }
        /// <summary>
        /// 采购申请抬头
        /// </summary>
        public Guid PurchaseRequisitionId { get; set; }
        public PurchaseRequisition PurchaseRequisition { get; set; }
        ///// <summary>
        ///// 主需求计划行
        ///// </summary>
        public Nullable<Guid> MdsItemId { get; set; }
        public MdsItem MdsItem { get; set; }
        #endregion

        public void Configure(IEntityMappingBuilder<PurchaseRequisitionItem> builder)
        {
            //主键
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(p => p.OwnerTenant, i => i.OwnerTenantId);
            //采购申请
            builder.HasMany(i => i.PurchaseRequisition, s => s.Items, i => i.PurchaseRequisitionId);
            //产品版本
            builder.HasMany(i => i.ProductVersion, i => i.ProductVersionId);
            //主需求计划
            builder.HasMany(i => i.MdsItem, m => m.PurReqItems, i => i.MdsItemId);
            //产品特性值
            builder.HasMany(i => i.ProdFeatValGrp, i => i.ProdFeatValGrpId);

        }
    }
}
