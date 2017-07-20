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
    public class PurReqMaterialItem : IFullAudit<PurReqMaterialItem, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 采购请求主数据属性
        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal NeedQuantity { get; set; }
        /// <summary>
        /// 下达剩余量
        /// </summary>
        public decimal IssuedRemainQuantity { get; set; }
        /// <summary>
        /// 拣货完成量
        /// </summary>
        public decimal PickFinishQuantity { get; set; }
        /// <summary>
        ///  拣货剩余量
        /// </summary>
        public decimal PickRemainQuantity { get; set; }
        /// <summary>
        /// 是否拣货完成
        /// </summary>
        public bool IsPickDone { get; set; }
        /// <summary>
        /// 是否取消拣货
        /// </summary>
        public bool IsPickCancel { get; set; }
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
        public Guid PurReqItemId { get; set; }
        public PurReqItem PurReqItem { get; set; }

        /// <summary>
        /// 产品版次
        /// </summary>
        public Guid ProdVerId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        /// <summary>
        /// 产品特性值
        /// </summary>
        public Nullable<Guid> ProdFeatValGrpId { get; set; }

        public ProductFeatureValueGroup ProdFeatValGrp { get; set; }

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
        public void Configure(IEntityMappingBuilder<PurReqMaterialItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //采购申请行
            builder.HasMany(p => p.PurReqItem, p => p.PurReqItemId);
            //MdsItem
            builder.HasMany(i => i.MdsItem, mdsItem => mdsItem.PurReqMatItems, i => i.MdsItemId);
            //产品版次
            builder.HasMany(p => p.ProductVersion, p => p.ProdVerId);
            //productFeatureValueGroup
            builder.HasMany(i => i.ProdFeatValGrp, i => i.ProdFeatValGrpId);
        }
        #endregion
    }
}
