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
    /// 计划生产工序行
    /// </summary>
    [ExportMany]
    public class PlanProductionProcessItem : IFullAudit<PlanProductionProcessItem, Guid>
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
        public decimal ProcessQty { get; set; }
        /// <summary>
        /// 完成数量
        /// </summary>
        public decimal FinishQty { get; set; }
        /// <summary>
        /// 乘余数量
        /// </summary>
        public decimal RemainQty { get; set; }
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
        /// 计划生产订单行
        /// </summary>
        public Guid PlanProductionOrderItemId { get; set; }
        public PlanProductionOrderItem PlanProductionOrderItem { get; set; }
        /// <summary>
        /// 工序
        /// 生产部门 + 工序 => 生产订单[内部生产订单]
        /// </summary>
        public Nullable<Guid> ProcessStepId { get; set; }
        public ProcessStep ProcessStep { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<PlanProductionProcessItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //计划生产订单行
            builder.HasMany(p => p.PlanProductionOrderItem, p => p.PlanProductionOrderItemId);
            //工序
            builder.HasMany(i => i.ProcessStep, i => i.ProcessStepId);
        }
        #endregion
    }
}
