﻿using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.SalesModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductionPlanModule.Domain.Entities
{
    /// <summary>
    /// 主生产计划明细
    /// </summary>
    [ExportMany]
    public class MpsItem : IFullAudit<MpsItem, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion
     
        #region 主生产计划行主数据属性
        /// <summary>
        /// 排程数量
        /// </summary>
        public decimal ScheduleQuantity { get; set; }

        /// <summary>
        /// 生产完成数量
        /// </summary>
        public decimal ProductionFinishQty { get; set; }
        /// <summary>
        /// 生产剩余数量
        /// </summary>
        public decimal ProductionRemainQty { get; set; }
        /// <summary>
        /// MRP完成数量
        /// </summary>
        public decimal MrpFinishQty { get; set; }
        /// <summary>
        /// MRP剩余数量
        /// </summary>
        public decimal MrpRemainQty { get; set; }
        /// <summary>
        /// 是否生产完成
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

        public Guid MpsId { get; set; }
        public Mps Mps { get; set; }

        /// <summary>
        /// 销售订单行
        /// </summary>
        public Nullable<Guid> SaleOrderItemId { get; set; }
        public SaleOrderItem SaleOrderItem { get; set; }

        /// <summary>
        /// 产品版次
        /// </summary>
        public Guid ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<MpsItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //ProductVersion
            builder.HasMany(i => i.ProductVersion, i => i.ProductVersionId);
            //MPS
            builder.HasMany(i => i.Mps, m => m.Items, i => i.MpsId);
            //SalesOrder
            builder.HasMany(i => i.SaleOrderItem, soItem => soItem.MpsItems, i => i.SaleOrderItemId);
        }
        #endregion
    }
}
