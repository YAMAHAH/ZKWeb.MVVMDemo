﻿using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductionPlanModule.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [ExportMany]
    public class PlanPurchaseOrder : IFullAudit<PlanPurchaseOrder, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 计划生产订单主数据属性
        /// <summary>
        /// 计划生产订单号
        /// </summary>
        public string PlanPurchaseOrderNo { get; set; }
        /// <summary>
        /// 计划日期
        /// </summary>
        public DateTime PlanDate { get; set; }
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
        /// 计划外包商
        /// </summary>
        public Guid VendorId { get; set; }
        public Partner Vendor { get; set; }
        /// <summary>
        /// 明细行
        /// </summary>
        public List<PlanPurchaseOrderItem> Items { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<PlanPurchaseOrder> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //供应商
            builder.HasMany(s => s.Vendor, s => s.VendorId);
        }
        #endregion
    }
}
