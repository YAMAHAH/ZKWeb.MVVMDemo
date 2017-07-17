using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.SalesModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductionModule.Domain.Entities
{
    /// <summary>
    ///  计划工序订单行
    ///  MRP->计划生产订单->生产订单->工序订单->子工序订单
    ///  MRP->计划外包订单->外包订单
    ///  MRP->计划采购订单->采购订单
    /// </summary>
    [ExportMany]
    public class ProcessOrdItem : IFullAudit<ProcessOrdItem, Guid>
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
        public decimal  Quantity { get; set; }
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
        public Guid ProductionOrderItemId { get; set; }
        public MfdOrdItem ProductionOrderItem { get; set; }
        /// <summary>
        /// 工序订单物料行集合
        /// </summary>
        public List<ProcessMaterialItem> ProcessOrderMaterialItems { get; set; }
        /// <summary>
        /// 子工序集合
        /// </summary>
        public List<SubProcessOrdItem> SubProcessOrderItems { get; set; }

        /// <summary>
        /// 销售订单行
        /// </summary>
        public Nullable<Guid> SaleOrderItemId { get; set; }
        public SaleOrderItem SaleOrderItem { get; set; }
        /// <summary>
        /// 工序
        /// </summary>
        public Nullable<Guid> ProcessStepId { get; set; }
        public ProcessStep ProcessStep { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<ProcessOrdItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //计划生产订单行
            builder.HasMany(p => p.ProductionOrderItem, i => i.ProcessOrdItems, p => p.ProductionOrderItemId);
            //工序
            builder.HasMany(i => i.ProcessStep, i => i.ProcessStepId);
        }
        #endregion
    }
}
