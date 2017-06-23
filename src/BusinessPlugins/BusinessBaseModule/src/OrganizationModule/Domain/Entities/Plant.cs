using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    [ExportMany]
    public class Plant : IFullAudit<Plant, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 工厂基本信息
        /// <summary>
        /// 工厂编码
        /// </summary>
        public string PlantNo { get; set; }
        /// <summary>
        /// 工厂名称
        /// </summary>
        public string PlantName { get; set; }
        #endregion

        #region 公司代码基本信息
        public Guid CompanyCodeId { get; set; }
        public CompanyCode CompanyCode { get; set; }
        #endregion

        #region 实体配置
        public void Configure(IEntityMappingBuilder<Plant> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            nativeBuilder.HasOne(p => p.CompanyCode)
                .WithMany(cc => cc.plants)
                .HasForeignKey(p => p.CompanyCodeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        #endregion
    }
}
