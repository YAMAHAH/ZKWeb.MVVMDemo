﻿using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 库存订单
    /// </summary>
    [ExportMany]
    public class InventoryOrder : IFullAudit<InventoryOrder, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion

        #region 库存申请基本属性
        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime NeedDate { get; set; }
        /// <summary>
        /// 库存订单号码
        /// </summary>
        public string StockOrderNumber { get; set; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDate { get; set; }
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
        public StorageLocation StorageLocation { get; set; }

        /// <summary>
        /// 申请行
        /// </summary>
        public List<InventoryOrderItem> Items { get; set; } = new List<InventoryOrderItem>();

        #endregion

        #region 实体关系配置
        public virtual void Configure(IEntityMappingBuilder<InventoryOrder> builder)
        {
            //主键
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(g => g.OwnerTenant, g => g.OwnerTenantId);
            //工厂
            builder.HasMany(g => g.Plant, g => g.PlantId);
            //库存地点
            builder.HasMany(g => g.StorageLocation, g => g.StorLocId);
        }
        #endregion

    }
}