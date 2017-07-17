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
    /// 外包订单行
    /// </summary>
    [ExportMany]
    public class PurReqItem : IFullAudit<PurReqItem, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 采购申请主数据属性
        /// <summary>
        /// 子订单号码
        /// 表内唯一
        /// 根据这个号码可以找出对应的项
        /// </summary>
        public string ChildNo { get; set; }
        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime NeedDate { get; set; }
        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal RequestQuantity { get; set; }
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
        /// 采购申请抬头
        /// </summary>
        public Guid PurReqId { get; set; }
        public PurReq PurReq { get; set; }


        /// <summary>
        /// MRP物料需求
        /// </summary>
        public Guid MrpMatItemId { get; set; }
        public MrpMaterialItem MrpMatItem { get; set; }

        /// <summary>
        /// 主需求计划行
        /// </summary>
        public Nullable<Guid> MdsItemId { get; set; }
        public MdsItem MdsItem { get; set; }
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
        /// 工序
        /// 生产部门 + 工序 => 生产订单[内部生产订单]
        /// </summary>
        public Nullable<Guid> ProcessStepId { get; set; }
        public ProcessStep ProcessStep { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<PurReqItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //计划外包订单
            builder.HasMany(p => p.PurReq, i => i.Items, p => p.PurReqId);
            //计划物料需求行
            builder.HasMany(p => p.MrpMatItem, p => p.MrpMatItemId); 
            //MdsItem
            builder.HasMany(i => i.MdsItem, mdsItem => mdsItem.PurReqItems, i => i.MdsItemId);
            //产品版次
            builder.HasMany(i => i.ProductVersion, i => i.ProdVerId);
            //productFeatureValueGroup
            builder.HasMany(i => i.ProdFeatValGrp, i => i.ProdFeatValGrpId);
            //工序
            builder.HasMany(i => i.ProcessStep, i => i.ProcessStepId);
        }
        #endregion
    }
}
