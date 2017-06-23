using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    /// <summary>
    /// 公司代码
    /// </summary>
    [ExportMany]
    public class CompanyCode : IFullAudit<CompanyCode, Guid>
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
        /// 公司
        /// </summary>
        public Nullable<Guid> CompanyId { get; set; }
        public Company Company { get; set; }
        #endregion

        #region 依赖对象集合引用
        public List<Plant> plants { get; set; }
        #endregion
        #region 实体配置
        public void Configure(IEntityMappingBuilder<CompanyCode> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            nativeBuilder.HasOne(c => c.Company)
                .WithMany(c => c.CompanyCodes)
                .HasForeignKey(cc => cc.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        #endregion
    }
}
