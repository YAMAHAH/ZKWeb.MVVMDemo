using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.SalesModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductionPlanModule.Domain.Entities
{
    /// <summary>
    /// MRP明细
    /// </summary>
    [ExportMany]
    public class MrpItem : IFullAudit<MrpItem, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion
   
        #region MRP行主数据属性
        /// <summary>
        /// 序号
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQuantity { get; set; }
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
        /// MRP抬头
        /// </summary>
        public Guid MrpId { get; set; }
        public Mrp Mrp { get; set; }
        /// <summary>
        /// 工厂
        /// </summary>
        public Guid PlantId { get; set; }
        public Plant Plant { get; set; }
        /// <summary>
        /// 产品版次
        /// </summary>
        public Guid ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        /// <summary>
        /// 销售订单行
        /// </summary>
        public Nullable<Guid> SaleOrderItemId { get; set; }
        public SaleOrderItem SaleOrderItem { get; set; }

        public List<MrpMaterialItem> MrpMaterialItems { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<MrpItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //ProductVersion
            builder.HasMany(i => i.ProductVersion, i => i.ProductVersionId);
            //MPS
            builder.HasMany(i => i.Mrp, m => m.Items, i => i.MrpId);
            //SalesOrder
            builder.HasMany(i => i.SaleOrderItem, soItem => soItem.MrpItems, i => i.SaleOrderItemId);
        }
        #endregion
    }
}
