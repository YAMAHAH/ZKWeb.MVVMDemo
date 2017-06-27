using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 库存地点多个地址
    /// 表示一个库存地点可以存放不同的地址
    /// </summary>
    [ExportMany]
    public class StorageLocationAddress : IFullAudit<StorageLocationAddress, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion

        #region 依赖对象引用
        /// <summary>
        /// 存储地点
        /// </summary>
        public Guid StorageLocationId { get; set; }
        public StorageLocation StorageLocation { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<StorageLocationAddress> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            nativeBuilder.HasOne(i => i.StorageLocation)
                .WithMany(l => l.StorageLocationAddresses)
                .HasForeignKey(i => i.StorageLocationId)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Restrict);
        }
        #endregion
    }
}
