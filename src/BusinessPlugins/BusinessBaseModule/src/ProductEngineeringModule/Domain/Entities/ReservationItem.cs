using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.WarehouseModule.Domain.Entities;
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
    /// 预留单明细
    /// </summary>
    [ExportMany]
    public class ReservationItem : IFullAudit<ReservationItem, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 预留明细基本属性
        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal NeedQuantity { get; set; }
        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime NeedDate { get; set; }
        /// <summary>
        /// 批次号码
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 是否允许移动物料
        /// </summary>
        public bool IsMoveAllowed { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion

        #region 依赖对象引用
        /// <summary>
        /// 物料
        /// </summary>
        public Guid ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }

        /// <summary>
        /// 源库存地
        /// </summary>
        public Nullable<Guid> SrcLocId { get; set; }
        public StorageLocation SrcStorageLocation { get; set; }
        /// <summary>
        /// 目标库存地
        /// </summary>
        public Nullable<Guid> DestLocId { get; set; }
        public StorageLocation DestStorageLocation { get; set; }
        /// <summary>
        /// 工厂
        /// </summary>
        public Nullable<Guid> PlantId { get; set; }
        public Plant Plant { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public Nullable<Guid> WhId { get; set; }
        public Warehouse Warehouse { get; set; }
        /// <summary>
        /// 货位
        /// </summary>
        public Nullable<Guid> StorageBinId { get; set; }
        public StoragePosition StoragePosition { get; set; }
        public Guid ReservationId { get; set; }
        /// <summary>
        /// 预留抬头
        /// </summary>
        public Reservation Reservation { get; set; }

        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<ReservationItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //预留单
            builder.HasMany(i => i.Reservation, i => i.ReservationItems, p => p.ReservationId);
            //源库存地点
            builder.HasMany(i => i.SrcStorageLocation, i => i.SrcLocId);
            //目标库存点
            builder.HasMany(i => i.DestStorageLocation, i => i.DestLocId);
            //仓库
            builder.HasMany(p => p.Warehouse, p => p.WhId);
            //储位
            builder.HasMany(p => p.StoragePosition, p => p.StorageBinId);
            //产品版次
            builder.HasMany(i => i.ProductVersion, i => i.ProductVersionId);
        }
        #endregion
    }
}
