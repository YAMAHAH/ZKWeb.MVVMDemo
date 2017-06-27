using BusinessPlugins.OrganizationModule.Domain;
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
    /// 存储分区,存储块
    /// </summary>
    [ExportMany]
    public class StorageSection : IFullAudit<StorageSection, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion

        #region 分区基本信息

        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string Remark { get; set; }
        #endregion

        #region 依赖对象引用
        public Guid StorageAreaId { get; set; }
        public StorageArea StorageArea { get; set; }
        #endregion

        #region 依赖对象引用集合
        /// <summary>
        /// 储位
        /// </summary>
        public List<StoragePosition> StoragePositions { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<StorageSection> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            nativeBuilder.HasOne(s => s.StorageArea)
                .WithMany(a => a.StorageSections)
                .HasForeignKey(s => s.StorageAreaId)
                .OnDelete(DeleteBehavior.SetNull);
        }
        #endregion
    }
}
