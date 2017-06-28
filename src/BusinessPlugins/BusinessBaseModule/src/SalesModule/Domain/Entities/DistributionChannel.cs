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

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 分销渠道
    /// 批发/零售/代销/直销
    /// </summary>
    [ExportMany]
    public class DistributionChannel : IFullAudit<DistributionChannel, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion

        #region 实体基本信息
        public string ChannelCode { get; set; }
        public string ChannelName { get; set; }
        #endregion
        #region 分销渠道
        public List<SalesOrgToDistr> SalesOrganizations { get; set; }
        /// <summary>
        /// 工厂
        /// </summary>
        public List<Plant> Plants { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<DistributionChannel> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
            builder.HasMany(c => c.SalesOrganizations);
        }
        #endregion
    }
}
