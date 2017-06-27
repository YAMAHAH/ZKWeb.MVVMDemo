using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 仓库
    /// 一个工厂有多个仓库,一个仓库有多个区域,
    /// 一个区域有多个分区,一个分区有多个储位
    /// </summary>
    [ExportMany]
    public class Warehouse : IFullAudit<Warehouse, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion
        #region 仓库基本信息
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 仓库位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// inventory available库存可用
        /// </summary>
        public bool IsAvailableInventory { get; set; }
        /// <summary>
        /// 停止使用
        /// </summary>
        public bool IsDisabled { get; set; }
        /// <summary>
        /// 备注
        /// </summary>

        public string Remark { get; set; }
        #endregion

        #region 依赖对象引用
    
        #endregion

        #region 依赖对象集合引用
        /// <summary>
        /// 存储区域
        /// </summary>
        public List<StorageArea> StorageAreas { get; set; }
        /// <summary>
        /// 库存地点
        /// </summary>
        public List<StorageLocation> StorageLocations { get; set; }
        #endregion
        public void Configure(IEntityMappingBuilder<Warehouse> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

        }
    }
}
