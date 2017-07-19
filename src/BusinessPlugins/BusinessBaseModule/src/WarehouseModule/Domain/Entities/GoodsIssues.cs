using BusinessPlugins.BaseModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductionModule.Domain.Entities;
using BusinessPlugins.PurchaseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 发货->确认->过账
    /// </summary>
    [ExportMany]
    public class GoodsIssues : IFullAudit<GoodsIssues, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion

        #region 收货基本属性
        /// <summary>
        /// 收货类型
        /// </summary>
        public MovementType MovementType { get; set; }
        /// <summary>
        /// 登记日期
        /// </summary>
        public DateTime RecordDate { get; set; }
        /// <summary>
        /// 发货单号
        /// </summary>
        public string GINumber { get; set; }
        /// <summary>
        /// 发货日期
        /// </summary>
        public DateTime GIDate { get; set; }
        /// <summary>
        /// 源单据号
        /// </summary>
        public string OriginalTicketNo { get; set; }
        /// <summary>
        /// 单据备注
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
        /// 库存地点
        /// </summary>
        public Nullable<Guid> StorLocId { get; set; }
        /// <summaryStorageLocation
        /// 库存地点
        /// </summary>
        public StorageLocation StorageLocation { get; set; }

        /// <summary>
        /// 伙伴ID
        /// </summary>
        public Nullable<Guid> PartnerId { get; set; }
        /// <summary>
        /// 合作伙伴
        /// </summary>
        public Partner Partner { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid DptmId { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public Department Department { get; set; }

        /// <summary>
        /// 生产订单
        /// </summary>
        public Nullable<Guid> MfdOrdId { get; set; }

        public ManufactureOrder MfdOrder { get; set; }
        /// <summary>
        /// 采购订单
        /// </summary>
        public Nullable<Guid> PurOrdId { get; set; }

        public PurchaseOrder PurOrd { get; set; }

        /// <summary>
        /// 发货Item
        /// </summary>
        public List<GoodsIssuesItem> Items { get; set; } = new List<GoodsIssuesItem>();

        #endregion

        #region 实体关系配置
        public virtual void Configure(IEntityMappingBuilder<GoodsIssues> builder)
        {
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(g => g.OwnerTenant, g => g.OwnerTenantId);
            //工厂
            builder.HasMany(g => g.Plant, g => g.PlantId);
            //库存地点
            builder.HasMany(g => g.StorageLocation, g => g.StorLocId);
            //合作合伴
            builder.HasMany(g => g.Partner, g => g.PartnerId);
            //部门
            builder.HasMany(g => g.Department, g => g.DptmId);
            //生产订单
            builder.HasMany(g => g.MfdOrder, g => g.MfdOrdId);
            //采购订单
            builder.HasMany(g => g.PurOrd, g => g.PurOrdId);
        }
        #endregion

    }
}
