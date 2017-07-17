using BusinessPlugins.BaseModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Entities
{
    [ExportMany]
    public class ProductMrpData : IFullAudit<ProductMrpData, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 产品MRP数据属性
        /// <summary>
        /// 产品来源类型
        /// </summary>
        public ProductSourceType ProductSourceType { get; set; }
        /// <summary>
        /// 经济订货量
        /// </summary>
        public decimal EconomicOrderQty { get; set; }
        /// <summary>
        /// 安全库存量
        /// </summary>
        public decimal SafetyStock { get; set; }

        /// <summary>
        /// 最大库存量
        /// </summary>
        public decimal MaximumStock { get; set; }
        /// <summary>
        /// 最低库存量
        /// </summary>
        public decimal MinimumStock { get; set; }

        /// <summary>
        /// 采购周期
        /// 计划交货时间
        /// </summary>
        public double PurchaseCycle { get; set; }
        /// <summary>
        /// 生产周期
        /// </summary>
        public double ProductionCycle { get; set; }
        /// <summary>
        /// 加工周期
        /// </summary>
        public double ProcessCycle { get; set; }
        /// <summary>
        /// 收货处理时间
        /// </summary>
        public double GRProcessTime { get; set; }

        #endregion
        #region 依赖对象引用
        /// <summary>
        /// 工厂
        /// </summary>
        public Guid PlantId { get; set; }
        public Plant Plant { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        /// <summary>
        /// 计划边际码
        /// </summary>
        public Nullable<Guid> MarginKeyId { get; set; }
        public ScheduleMarginKey ScheduleMarginKey { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<ProductMrpData> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //Product
            builder.HasOne(m => m.Product, p => p.MrpData, p => p.ProductId);
            //计划边际码
            builder.HasMany(m => m.ScheduleMarginKey, m => m.MarginKeyId);
        }
        #endregion
    }
}
