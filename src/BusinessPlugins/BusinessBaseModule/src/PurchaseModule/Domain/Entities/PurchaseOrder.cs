using BusinessPlugins.BaseModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using BusinessPlugins.WarehouseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.PurchaseModule.Domain.Entities
{
    /// <summary>
    /// 采购订单
    /// </summary>
    [ExportMany]
    public class PurchaseOrder : IFullAudit<PurchaseOrder, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 采购订单信息
        /// <summary>
        /// 采购类型
        /// </summary>
        public PurchaseType PurchaseType { get; set; }
        /// <summary>
        /// 采购日期
        /// </summary>
        public DateTime PurchaseDate { get; set; }
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
        /// 工厂
        /// </summary>
        public Guid PlantId { get; set; }
        public Plant Plant { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public Guid VendorId { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public Partner Vendor { get; set; }
        /// <summary>
        /// 计划采购
        /// </summary>
        public Nullable<Guid> PldPurId { get; set; }
        public PlannedPurchase PldPur { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public Guid DptmId { get; set; }
        public Department Department { get; set; }
        /// <summary>
        /// 采购组
        /// </summary>
        public Guid PurGrpId { get; set; }
        public PurchaseGroup PurchaseGroup { get; set; }
        /// <summary>
        /// 采购组织
        /// </summary>
        public Guid PurOrgId { get; set; }
        public PurchaseOrganization PurchaseOrganization { get; set; }
        /// <summary>
        /// 转储订单
        /// </summary>
        public Nullable<Guid> TransferOrderId { get; set; }
        public TransferOrder TransferOrder { get; set; }
        /// <summary>
        /// 领料申请单
        /// </summary>
        public Nullable<Guid> MatReqId { get; set; }
        public MaterialRequisition MatReq { get; set; }
        /// <summary>
        /// 采购行
        /// </summary>
        public List<PurchaseOrderItem> Items { get; set; }
        #endregion

        public void Configure(IEntityMappingBuilder<PurchaseOrder> builder)
        {
            //主键
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(p => p.OwnerTenant, p => p.OwnerTenantId);
            //供应商
            builder.HasMany(i => i.Vendor, i => i.VendorId);
            //部门
            builder.HasMany(i => i.Department, i => i.DptmId);
            //采购组织
            builder.HasMany(i => i.PurchaseOrganization, i => i.PurOrgId);
            //采购组
            builder.HasMany(i => i.PurchaseGroup, i => i.PurGrpId);
            //领料申请单
            builder.HasMany(i => i.MatReq, i => i.MatReqId);
            //转储订单
            builder.HasMany(i => i.TransferOrder, i => i.TransferOrderId);
            //
            builder.HasMany(i => i.PldPur, i => i.PldPurId);
        }
    }
}
