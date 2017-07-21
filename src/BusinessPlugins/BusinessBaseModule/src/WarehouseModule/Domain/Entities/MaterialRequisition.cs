using BusinessPlugins.BaseModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductionModule.Domain.Entities;
using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using BusinessPlugins.PurchaseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 领料申请单
    /// 基于采购订单的领料单,可以作为采购订单发料的参考输入
    /// 基于计划订单的领料单,可以当作采购订单的参考输入
    /// </summary>
    [ExportMany]
    public class MaterialRequisition : IFullAudit<MaterialRequisition, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion

        #region 领料申请基本属性
        /// <summary>
        /// 移动类型
        /// </summary>
        public MovementType MovementType { get; set; }
        /// <summary>
        /// 登记日期
        /// </summary>
        public DateTime RecordDate { get; set; }
        /// <summary>
        /// 申请单号
        /// </summary>
        public string RequisitionNumber { get; set; }
        /// <summary>
        /// 申请日期
        /// </summary>
        public DateTime RequisitionDate { get; set; }
        /// <summary>
        /// 源单据号
        /// </summary>
        public string OriginalTicketNo { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsDone { get; set; }
        /// <summary>
        /// 是否取消
        /// </summary>
        public bool IsCancel { get; set; }

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
        /// 库存地点
        /// </summary>
        public Nullable<Guid> StorLocId { get; set; }
        /// <summaryStorageLocation
        /// 库存地点
        /// </summary>
        public StorageLocation StorageLocation { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public Nullable<Guid> VendorId { get; set; }
        public Partner Vendor { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid DptmId { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public Department Department { get; set; }
        /// <summary>
        /// 计划订单
        /// </summary>
        public Nullable<Guid> PldOrdId { get; set; }
        public PlannedOrder PldOrder { get; set; }
        /// <summary>
        /// 生产订单
        /// </summary>
        public Nullable<Guid> MfdOrdId { get; set; }

        public ManufactureOrder MfdOrder { get; set; }
        /// <summary>
        /// 计划采购
        /// </summary>
        public Nullable<Guid> PldPurId { get; set; }
        public PlannedPurchase PldPur { get; set; }
        /// <summary>
        /// 采购订单
        /// </summary>
        public Nullable<Guid> PurOrdId { get; set; }

        public PurchaseOrder PurOrd { get; set; }

        /// <summary>
        /// 领料单行
        /// </summary>
        public List<MaterialRequisitionItem> Items { get; set; } = new List<MaterialRequisitionItem>();

        #endregion

        #region 实体关系配置
        public virtual void Configure(IEntityMappingBuilder<MaterialRequisition> builder)
        {
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(g => g.OwnerTenant, g => g.OwnerTenantId);
            //工厂
            builder.HasMany(g => g.Plant, g => g.PlantId);
            //库存地点
            builder.HasMany(g => g.StorageLocation, g => g.StorLocId);
            //部门
            builder.HasMany(g => g.Department, g => g.DptmId);
            //计划订单
            builder.HasMany(g => g.PldOrder, g => g.PldOrdId);
            //生产订单
            builder.HasMany(g => g.MfdOrder, g => g.MfdOrdId);
            //计划采购
            builder.HasMany(g => g.PldPur, g => g.PldPurId);
            //采购订单
            builder.HasMany(g => g.PurOrd, g => g.PurOrdId);
        }
        #endregion

    }
}
