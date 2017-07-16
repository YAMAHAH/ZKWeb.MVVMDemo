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
    /// 采购申请物料
    /// </summary>
    [ExportMany]
    public class PurReqMatItem : IFullAudit<PurReqMatItem, Guid>
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
        /// 采购申请行
        /// </summary>
        public Guid PurchaseRequestItemId { get; set; }
        public PurReqItem PurchaseRequestItem { get; set; }

        /// <summary>
        /// 产品版次
        /// </summary>
        public Guid ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        ///// <summary>
        ///// 销售订单行
        ///// </summary>
        //public Nullable<Guid> SaleOrderItemId { get; set; }
        //public SaleOrderItem SaleOrderItem { get; set; }

        /// <summary>
        /// 主需求计划行
        /// </summary>
        public Nullable<Guid> MdsItemId { get; set; }
        public MdsItem MdsItem { get; set; }

        /// <summary>
        /// 预留
        /// </summary>
        public Reservation Reservation { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<PurReqMatItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //采购申请行
            builder.HasMany(p => p.PurchaseRequestItem, p => p.PurchaseRequestItemId);
            //销售订单
            //builder.HasMany(i => i.SaleOrderItem, s => s.PurchaseRequestMaterialItems, i => i.SaleOrderItemId);
            //MdsItem
            builder.HasMany(i => i.MdsItem, mdsItem => mdsItem.PurReqMatItems, i => i.MdsItemId);
            //产品版次
            builder.HasMany(p => p.ProductVersion, p => p.ProductVersionId);
        }
        #endregion
    }
}
