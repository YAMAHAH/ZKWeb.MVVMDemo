using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 库存地点
    /// </summary>
    [ExportMany]
    public class StorageLocation : IFullAudit<StorageLocation, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion
        #region 仓储地点基本信息

        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string Remark { get; set; }

        #endregion

        #region 依赖对象引用
        /// <summary>
        /// 工厂
        /// </summary>
        public Guid PlantId { get; set; }
        public Plant Plant { get; set; }
        public Nullable<Guid> WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        #endregion

        #region 依赖对象集合引用
        public List<StorageLocationAddress> StorageLocationAddresses { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<StorageLocation> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            nativeBuilder.HasOne(l => l.Warehouse)
                .WithMany(w => w.StorageLocations)
                .HasForeignKey(l => l.WarehouseId)
                .OnDelete(DeleteBehavior.SetNull);

            //工厂
            nativeBuilder.HasOne(l => l.Plant)
                .WithMany(p => p.StorageLocations)
                .HasForeignKey(l => l.PlantId)
                .OnDelete(DeleteBehavior.SetNull);


    }
        #endregion
    }
}
