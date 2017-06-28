using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 销售组织分销渠道
    /// </summary>
    [ExportMany]
    public class SalesOrgDistrChannel :  IFullAudit<SalesOrgDistrChannel, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion
        #region 主数据属性

        #endregion


        #region 依赖对象引用
        public List<SalesOrgDistrToPlant> Plants { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<SalesOrgDistrChannel> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
            builder.HasMany(o => o.Plants);
           
        }
        #endregion
    }
}
