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

namespace BusinessPlugins.ProductionPlanModule.Domain.Entities
{
    /// <summary>
    /// MRP物料清单
    /// </summary>
    [ExportMany]
    public class MrpMaterialItem : IFullAudit<MrpMaterialItem, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 物料需求清单基本信息
        /// <summary>
        /// 毛需求量
        /// </summary>
        public decimal GrossRequirements { get; set; }
        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal DemandQuantity { get; set; }
        /// <summary>
        /// 净需求量
        /// </summary>
        public decimal NetRequirements { get; set; }
        /// <summary>
        /// 可用库存量
        /// </summary>
        public decimal AvailableInventory { get; set; }

        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime NeedDate { get; set; }
        /// <summary>
        /// 备货日期
        /// </summary>
        public DateTime PrepareDate { get; set; }
        /// <summary>
        /// 固定损耗率
        /// </summary>
        public double FixedLossRate { get; set; }
        /// <summary>
        /// 损耗率
        /// </summary>
        public double LossRate { get; set; }
        /// <summary>
        /// 附加值
        /// </summary>
        public double AddedValues { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 根ID
        /// </summary>
        public Nullable<Guid> RootId { get; set; }
        /// <summary>
        /// 左结点序号
        /// 预排序遍历树算法
        /// </summary>
        public int Lft { get; set; }
        /// <summary>
        /// 右结点序号
        /// 预排序遍历树算法
        /// </summary>
        public int Rgt { get; set; }
        /// <summary>
        /// 结点所在的层次
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 低阶码
        /// </summary>
        public int LowLevelCode { get; set; }
        /// <summary>
        /// 是否库存物料
        /// </summary>
        public bool IsInventoryItem { get; set; }
        /// <summary>
        /// 是否客供料
        /// </summary>
        public bool IsConsignItem { get; set; }
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
        public ProductVersion ProdVer { get; set; }
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
        /// MRP行
        /// </summary>
        public Guid MrpItemId { get; set; }
        public MrpItem MrpItem { get; set; }
        /// <summary>
        /// 父亲行
        /// </summary>
        public Nullable<Guid> ParentId { get; set; }
        public MrpMaterialItem Parent { get; set; }
        /// <summary>
        /// 子行集合
        /// </summary>
        public List<MrpMaterialItem> Childs { get; set; }

        /// <summary>
        /// 生产部门 + 采购类型(客供品/采购/外协/生产)
        /// </summary>
        public Nullable<Guid> DeptId { get; set; }
        public Department ProdDept { get; set; }
        /// <summary>
        /// 工序
        /// 生产部门 + 工序 => 生产订单[内部生产订单,外协加工订单]
        /// 供应商 + 工序 => 外包订单
        /// </summary>
        public Nullable<Guid> ProcessStepId { get; set; }
        public ProcessStep ProcessStep { get; set; }
        /// <summary>
        /// 供应商
        /// 客供料 + 供应商 =>客供料订单
        /// 不是客供料 + 供应商 => 采购订单
        /// </summary>
        public Nullable<Guid> VendId { get; set; }
        public Partner Vendor { get; set; }

        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<MrpMaterialItem> builder)
        {
            //var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //ProductVersion
            builder.HasMany(i => i.ProdVer, i => i.ProductVersionId);
            //计划部门
            builder.HasOne(m => m.ProdDept, m => m.DeptId);
            //MdsItem
            builder.HasMany(i => i.MdsItem, m => m.MrpMatItems, i => i.MdsItemId);
            //productFeatureValueGroup
            builder.HasMany(i => i.ProdFeatValGrp, i => i.ProdFeatValGrpId);
            //MrpMaterialItem
            builder.HasMany(i => i.MrpItem, mrp => mrp.MrpMaterialItems, i => i.MrpItemId);
            //ProcessStep
            builder.HasMany(i => i.ProcessStep, i => i.ProcessStepId);
            //vendor
            builder.HasMany(i => i.Vendor, i => i.VendId);
            //Parent
            builder.HasMany(i => i.Parent, i => i.Childs, i => i.ParentId);

        }
        #endregion
    }
}
