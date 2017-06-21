using System;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    /// <summary>
    /// 通用配置
    /// </summary>
    [ExportMany]
    public class GenericConfig :
        IEntity<Guid>,
        IHaveCreateTime,
        IHaveUpdateTime,
        IHaveOwnerTenant,
        IEntityMappingProvider<GenericConfig>
    {
        /// <summary>
        /// 配置Id
        /// </summary>
        public virtual Guid Id { get; set; }
        /// <summary>
        /// 配置键，同一租户中应该唯一
        /// </summary>
        public virtual string Key { get; set; }
        /// <summary>
        /// 配置值（json）
        /// </summary>
        public virtual string Value { get; set; }
        /// <summary>
        /// 所属的租户
        /// </summary>
        public virtual Tenant OwnerTenant { get; set; }
        public virtual Guid OwnerTenantId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }

        /// <summary>
        /// 配置数据库结构
        /// </summary>
        public virtual void Configure(IEntityMappingBuilder<GenericConfig> builder)
        {
            builder.Id(c => c.Id);
            builder.Map(c => c.Key);
            builder.Map(c => c.Value);
            builder.References(c => c.OwnerTenant, new EntityMappingOptions() { Nullable = false });
            builder.Map(c => c.CreateTime);
            builder.Map(c => c.UpdateTime);
        }
    }
}
