﻿using BusinessPlugins.BaseModule.Domain.Entities;
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
    /// 转储申请单
    /// 生产订单->转储申请->打印备料单->申请单确认->发料单
    /// </summary>
    [ExportMany]
    public class TransferRequisition : IFullAudit<TransferRequisition, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion

        #region 转储请求基本属性
        /// <summary>
        /// 移动类型
        /// </summary>
        public MovementType MovementType { get; set; }
        /// <summary>
        /// 登记日期
        /// </summary>
        public DateTime RecordDate { get; set; }
        /// <summary>
        /// 请求单号
        /// </summary>
        public string TRNumber { get; set; }
        /// <summary>
        /// 请求日期
        /// </summary>
        public DateTime TRDate { get; set; }
        /// <summary>
        /// 源单据号
        /// </summary>
        public string OriginalTicketNo { get; set; }
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
        /// 伙伴ID
        /// </summary>
        public Nullable<Guid> PartnerId { get; set; }
        /// <summary>
        /// 合作伙伴
        /// </summary>
        public Partner Partner { get; set; }

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
        /// 收货Item
        /// </summary>
        public List<TransferRequisitionItem> Items { get; set; } = new List<TransferRequisitionItem>();

        #endregion

        #region 实体关系配置
        public virtual void Configure(IEntityMappingBuilder<TransferRequisition> builder)
        {
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(g => g.OwnerTenant, g => g.OwnerTenantId);
            //工厂
            builder.HasMany(g => g.Plant, g => g.PlantId);
            //库存地点
            builder.HasMany(g => g.StorageLocation, g => g.StorLocId);
            //合作合伴
            builder.HasMany(g => g.Partner, g => g.PartnerId);
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