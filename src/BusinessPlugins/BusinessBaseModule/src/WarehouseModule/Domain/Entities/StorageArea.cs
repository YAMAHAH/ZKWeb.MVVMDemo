using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 存储区域
    /// </summary>
    [ExportMany]
    public class StorageArea : IFullAudit<StorageArea, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion
        #region 存储区域基本信息
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        #endregion

        #region 依赖对象引用

        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        #endregion
        #region 依赖对象集合引用
        public List<StorageSection> StorageSections { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<StorageArea> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            nativeBuilder.HasOne(a => a.Warehouse)
                .WithMany(w => w.StorageAreas)
                .HasForeignKey(a => a.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        #endregion
    }
}
