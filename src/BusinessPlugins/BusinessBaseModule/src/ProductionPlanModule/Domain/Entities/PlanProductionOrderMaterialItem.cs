using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.SalesModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWeb.Database;

namespace BusinessPlugins.ProductionPlanModule.Domain.Entities
{
    /// <summary>
    /// 计划生产物料计划明细
    /// </summary>
    public class PlanProductionOrderMaterialItem : IFullAudit<PlanProductionOrderMaterialItem, Guid>
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
        /// 产品版次
        /// </summary>
        public Guid ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        /// <summary>
        /// 计划生产订单行
        /// </summary>
        public Guid PlanProductionOrderItemId { get; set; }
        public PlanProductionOrder PlanProductionOrderItem { get; set; }
        /// <summary>
        /// 销售订单行
        /// </summary>
        public Nullable<Guid> SaleOrderItemId { get; set; }
        public SaleOrderItem SaleOrderItem { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<PlanProductionOrderMaterialItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //计划生产订单行
            builder.HasMany(p => p.PlanProductionOrderItem, p => p.PlanProductionOrderItemId);
            //销售订单
            builder.HasMany(i => i.SaleOrderItem, s => s.PlanProductionOrderMaterialItems, i => i.SaleOrderItemId);
            //产品版次
            builder.HasMany(p => p.ProductVersion, p => p.ProductVersionId);
        }
        #endregion
    }
}
