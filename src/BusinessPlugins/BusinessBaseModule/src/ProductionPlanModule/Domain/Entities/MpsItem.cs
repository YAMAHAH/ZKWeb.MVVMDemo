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
        /// 子订单号码
        /// 表内唯一
        /// 根据这个号码可以找出对应的项
        /// </summary>
        public string ChildNo { get; set; }
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
        /// 主需求计划行
        /// </summary>
        public Nullable<Guid> MdsItemId { get; set; }
        public MdsItem MdsItem { get; set; }
        /// <summary>
        /// 产品特性值
        /// </summary>
        public Nullable<Guid> ProdFeatValGrpId { get; set; }

        public ProductFeature ProdFeatValGrp { get; set; }

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
            //MdsItem
            builder.HasMany(i => i.MdsItem, mdsItem => mdsItem.MpsItems, i => i.MdsItemId);
            //productFeatureValueGroup
            builder.HasMany(i => i.ProdFeatValGrp, i => i.ProdFeatValGrpId);
        }
        #endregion
    }
}
