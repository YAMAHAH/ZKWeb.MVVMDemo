using BusinessPlugins.BaseModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductionModule.Domain.Entities;
using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using BusinessPlugins.PurchaseModule.Domain.Entities;
using BusinessPlugins.SalesModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Entities
{
    /// <summary>
    /// 预留单抬头
    /// </summary>
    [ExportMany]
    public class Reservation : IFullAudit<Reservation, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 基本属性
        /// <summary>
        /// 基准日期
        /// </summary>
        public DateTime BaseDate { get; set; }
        /// <summary>
        /// 预留类型
        /// </summary>
        public ReservationType ReservationType { get; set; }

        //移动类型 
        //引用预留
        //业务范围
        //成本中心
        #endregion

        #region 依赖对象引用
        /// <summary>
        /// 工厂
        /// </summary>
        public Nullable<Guid> PlantId { get; set; }
        public Plant Plant { get; set; }
        /// <summary>
        /// 销售订单
        /// </summary>
        public Nullable<Guid> SalOrdItemId { get; set; }
        public SaleOrderItem SalOrdItem { get; set; }
        /// <summary>
        /// 采购申请
        /// </summary>
        public Nullable<Guid> PurReqMatItemId { get; set; }
        public PldPurMaterialItem PurReqMatItem { get; set; }
        /// <summary>
        /// 计划订单
        /// </summary>
        public Nullable<Guid> PldOrdMatItemId { get; set; }
        public PldOrdMaterialItem PldOrdMatItem { get; set; }
        /// <summary>
        /// 采购订单
        /// </summary>
        public Nullable<Guid> PurOrdMatItemId { get; set; }
        public PurchaseMaterialItem PurOrdMatItem { get; set; }
        /// <summary>
        /// 生产订单
        /// </summary>
        public Nullable<Guid> ProdOrdMatItemId { get; set; }
        public ManufactureMaterailItem ProdOrdMatItem { get; set; }
        /// <summary>
        /// 预留明细
        /// </summary>
        public List<ReservationItem> ReservationItems { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<Reservation> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //销售订单行
            builder.HasOne(r => r.SalOrdItem, p => p.Reservation, r => r.SalOrdItemId);
            //计划订单行
            builder.HasOne(r => r.PldOrdMatItem, p => p.Reservation, r => r.PldOrdMatItemId);
            //采购请求
            builder.HasOne(r => r.PurReqMatItem, p => p.Reservation, r => r.PurReqMatItemId);
            //生产订单
            builder.HasOne(r => r.ProdOrdMatItem, p => p.Reservation, r => r.ProdOrdMatItemId);
            //采购订单
            builder.HasOne(r => r.PurOrdMatItem, p => p.Reservation, r => r.PurOrdMatItemId);
        }
        #endregion
    }
}
