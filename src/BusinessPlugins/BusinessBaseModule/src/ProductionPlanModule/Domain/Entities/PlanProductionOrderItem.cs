using BusinessPlugins.OrganizationModule.Domain;
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
    /// 计划生产订单行
    /// </summary>
    [ExportMany]
    public class PlanProductionOrderItem : IFullAudit<PlanProductionOrderItem, Guid>
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
        /// 需求日期
        /// </summary>
        public DateTime NeedDate { get; set; }
        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQuantity { get; set; }
        /// <summary>
        /// 已下达数量
        /// </summary>
        public decimal IssuedQuantity { get; set; }

        public decimal IssuedRemainQty { get; set; }
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
        /// 计划生产订单抬头
        /// </summary>
        public Guid PlanProductionOrderId { get; set; }
        public PlanProductionOrder PlanProductionOrder { get; set; }
        /// <summary>
        /// MRP物料需求
        /// </summary>
        public Guid MrpMaterialItemId { get; set; }
        public MrpMaterialItem MrpMaterialItem { get; set; }
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
        /// <summary>
        /// 工序
        /// 生产部门 + 工序 => 生产订单[内部生产订单]
        /// </summary>
        public Nullable<Guid> ProcessStepId { get; set; }
        public ProcessStep ProcessStep { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<PlanProductionOrderItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //计划生产订单
            builder.HasMany(p => p.PlanProductionOrder, i => i.Items, p => p.PlanProductionOrderId);
            //计划物料需求行
            builder.HasMany(p => p.MrpMaterialItem, p => p.MrpMaterialItemId);
            //销售订单
            builder.HasMany(i => i.SaleOrderItem, s => s.PlanProductionOrderItems, i => i.SaleOrderItemId);
            //产品版次
            builder.HasMany(i => i.ProductVersion, i => i.ProductVersionId);
            //工序
            builder.HasMany(i => i.ProcessStep, i => i.ProcessStepId);
        }
        #endregion
    }
}
