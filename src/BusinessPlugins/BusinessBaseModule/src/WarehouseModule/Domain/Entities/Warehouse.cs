using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWeb.Database;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 仓库
    /// </summary>
    public class Warehouse : IFullAudit<Warehouse, Guid>
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string Ckno { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string Ckname { get; set; }

        /// <summary>
        /// inventory available库存可用
        /// </summary>
        public bool SF_kcuse { get; set; }
        /// <summary>
        /// 停止使用
        /// </summary>
        public bool SF_Disuse { get; set; }
        /// <summary>
        /// 备注
        /// </summary>

        public string Remark { get; set; }

        public void Configure(IEntityMappingBuilder<Warehouse> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

        }
    }
}
