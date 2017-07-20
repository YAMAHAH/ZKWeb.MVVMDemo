using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using BusinessPlugins.ProductionModule.Domain.Entities;
using BusinessPlugins.PurchaseModule.Domain.Entities;
using BusinessPlugins.SalesModule.Domain.Entities;
using BusinessPlugins.WarehouseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;


namespace BusinessPlugins.ProductionPlanModule.Domain.Entities
{
    /// <summary>
    /// 主需求项目
    /// 
    /// </summary>
    [ExportMany]
    public class MdsItem : IFullAudit<MdsItem, Guid>
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
        public decimal Quantity { get; set; }
        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime NeedDate { get; set; }


        /// <summary>
        /// Mrp完成数量
        /// </summary>
        public decimal MrpFinishQty { get; set; }
        /// <summary>
        /// Mrp剩余数量
        /// </summary>
        public decimal MrpRemainQty { get; set; }
        /// <summary>
        /// Mps完成数量
        /// </summary>
        public decimal MpsFinishQty { get; set; }
        /// <summary>
        /// Mps剩余数量
        /// </summary>
        public decimal MpsRemainQty { get; set; }
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

        public Guid MdsId { get; set; }
        public Mds Mds { get; set; }
        /// <summary>
        /// 产品版次
        /// </summary>
        public Guid ProdVerId { get; set; }
        public ProductVersion ProdVer { get; set; }
        /// <summary>
        /// 销售订单行
        /// </summary>
        public Nullable<Guid> SalOrdItemId { get; set; }
        public SaleOrderItem SalOrdItem { get; set; }
        /// <summary>
        /// 交货行
        /// </summary>
        public List<DeliveryOrderItem> DeliveryOrdItems { get; set; }
        /// <summary>
        /// 产品特性值
        /// </summary>
        public Nullable<Guid> ProdFeatValGrpId { get; set; }

        public ProductFeatureValueGroup ProdFeatValGrp { get; set; }

        /// <summary>
        /// 生产订单BOM
        /// </summary>

        public Nullable<Guid> ProdOrdBomId { get; set; }

        public ManufactureBom ProdOrdBom { get; set; }

        /// <summary>
        /// MPS行
        /// </summary>
        public List<MpsItem> MpsItems { get; set; } = new List<MpsItem>();
        /// <summary>
        /// MRP行
        /// </summary>
        public List<MrpItem> MrpItems { get; set; } = new List<MrpItem>();
        /// <summary>
        /// MRP行
        /// </summary>
        public List<MrpMaterialItem> MrpMatItems { get; set; } = new List<MrpMaterialItem>();
        /// <summary>
        /// 计划生产订单行
        /// </summary>
        public List<PldOrdItem> PldOrdItems { get; set; }
        /// <summary>
        /// 计划生产订单物料行
        /// </summary>
        public List<PldOrdMaterialItem> PldOrdMatItems { get; set; }

        /// <summary>
        /// 生产订单行
        /// </summary>
        public List<ManufactureOrderItem> ProdOrdItems { get; set; }
        /// <summary>
        /// 生产订单物料行
        /// </summary>
        public List<ManufactureMaterailItem> ProdOrdMatItems { get; set; }
        /// <summary>
        /// 流程订单行
        /// </summary>
        public List<ProcessOrderItem> ProcessOrdItems { get; set; }
        /// <summary>
        /// 流程订单物料行
        /// </summary>
        public List<ProcessMaterialItem> ProcessMatItems { get; set; }
        /// <summary>
        /// 子工序行
        /// </summary>
        public List<SubProcessOrderItem> SubProcessItems { get; set; }
        /// <summary>
        /// 子工序物料行
        /// </summary>
        public List<SubProcessMaterialItem> SubProcessMatItems { get; set; }
        /// 采购申请行
        /// </summary>
        public List<PurReqItem> PurReqItems { get; set; }
        /// <summary>
        /// 采购申请物料行
        /// </summary>
        public List<PurReqMaterialItem> PurReqMatItems { get; set; }

        /// <summary>
        /// 采购订单行
        /// </summary>
        public List<PurchaseOrderItem> PurOrdItems { get; set; }
        /// <summary>
        /// 采购物料行
        /// </summary>
        public List<PurchaseMaterialItem> PurMatItems { get; set; }
        /// <summary>
        /// 收货行
        /// </summary>
        public List<GoodsReceiptItem> GRItems { get; set; }
        /// <summary>
        /// 发货行
        /// </summary>
        public List<GoodsIssuesItem> GIItems { get; set; }
        /// <summary>
        /// 转储请求行
        /// </summary>
        public List<TransferRequestItem> TransferReqItems { get; set; }
        /// <summary>
        /// 转储订单行
        /// </summary>
        public List<TransferOrderItem> TransferOrdItems { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<MdsItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //ProductVersion
            builder.HasMany(i => i.ProdVer, i => i.ProdVerId);
            //SalesOrder
            builder.HasOne(i => i.SalOrdItem, s => s.MdsItem, i => i.SalOrdItemId);
            //mds
            builder.HasMany(i => i.Mds, m => m.Items, i => i.MdsId);
            //产品特性值
            builder.HasOne(i => i.ProdFeatValGrp, i => i.ProdFeatValGrpId);
            //生产订单BOM
            builder.HasMany(i => i.ProdOrdBom, i => i.ProdOrdBomId);
        }
        #endregion
    }
}
