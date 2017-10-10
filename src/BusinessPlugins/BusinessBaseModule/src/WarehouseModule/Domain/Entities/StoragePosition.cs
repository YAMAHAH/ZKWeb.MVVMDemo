using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 存储位置
    /// 与存储分区是1对多关系
    /// </summary>
    [ExportMany]
    public class StoragePosition : IFullAudit<StoragePosition, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion

        #region 储位基本信息
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        #endregion

        #region 依赖对象引用
        /// <summary>
        /// 存储分区
        /// </summary>
        public Guid StorageSectionId { get; set; }
        public StorageSection StorageSection { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<StoragePosition> builder)
        {
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(i => i.OwnerTenant, i => i.OwnerTenantId);

            //存储分区
            builder.HasMany(i => i.StorageSection,
                d => d.StoragePositions,
                i => i.StorageSectionId,
                DeleteBehavior.Restrict);

        }
        #endregion
    }
}
