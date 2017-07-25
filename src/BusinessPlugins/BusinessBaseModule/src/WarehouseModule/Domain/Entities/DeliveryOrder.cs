using BusinessPlugins.BasicModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.PurchaseModule.Domain.Entities;
using BusinessPlugins.SalesModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 交货单
    /// 分为采购交货单和销售交货单
    /// </summary>
    [ExportMany]
    public class DeliveryOrder : IFullAudit<DeliveryOrder, Guid>
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
        /// 交货类型
        /// 采购交货单/销售交货单
        /// </summary>
        public DeliveryType DeliveryType { get; set; }
        /// <summary>
        /// 登记日期
        /// </summary>
        public DateTime RecordDate { get; set; }
        /// <summary>
        /// 交货单号
        /// </summary>
        public string DeliveryNumber { get; set; }
        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime DeliveryDate { get; set; }
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
        /// 销售订单
        /// </summary>
        public Nullable<Guid> SaleOrderId { get; set; }

        public SalesOrder SaleOrder { get; set; }
        /// <summary>
        /// 采购订单
        /// </summary>
        public Nullable<Guid> PurchaseOrderId { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }
        /// <summary>
        /// 交货Item
        /// </summary>
        public List<DeliveryOrderItem> Items { get; set; } = new List<DeliveryOrderItem>();

        #endregion

        #region 实体关系配置
        public virtual void Configure(IEntityMappingBuilder<DeliveryOrder> builder)
        {
            //主键
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(d => d.OwnerTenant, d => d.OwnerTenantId);
            //工厂
            builder.HasMany(d => d.Plant, d => d.PlantId);
            ////库存地点
            builder.HasMany(d => d.StorageLocation, d => d.StorLocId);
            ////合作合伴
            builder.HasMany(d => d.Partner, d => d.PartnerId);
            ////部门
            builder.HasMany(d => d.Department, d => d.DptmId);
            //销售订单
            builder.HasMany(d => d.SaleOrder, d => d.SaleOrderId);
            //采购订单
            builder.HasMany(d => d.PurchaseOrder, d => d.PurchaseOrderId);
        }
        #endregion

    }
}
