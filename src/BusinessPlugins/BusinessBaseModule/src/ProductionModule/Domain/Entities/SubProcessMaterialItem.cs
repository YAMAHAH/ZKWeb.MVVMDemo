using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.SalesModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductionModule.Domain.Entities
{
    /// <summary>
    ///工序订单物料
    /// </summary>
    [ExportMany]
    public class SubProcessMaterialItem : IFullAudit<SubProcessMaterialItem, Guid>
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
        /// 加工数量
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// 领料完成数量
        /// </summary>
        public decimal FinishQty { get; set; }
        /// <summary>
        /// 领料剩余数量
        /// </summary>
        public decimal RemainQty { get; set; }
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
        public Guid SubProcessOrderItemId { get; set; }
        public SubProcessOrderItem SubProcessOrderItem { get; set; }
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

        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<SubProcessMaterialItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //子工序订单
            builder.HasMany(p => p.SubProcessOrderItem, i => i.SubProcessOrderMaterialItems, p => p.SubProcessOrderItemId);
            //产品版次
            builder.HasMany(i => i.ProductVersion, i => i.ProductVersionId);
        }
        #endregion
    }
}
