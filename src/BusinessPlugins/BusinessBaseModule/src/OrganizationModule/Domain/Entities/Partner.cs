using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;
using Microsoft.EntityFrameworkCore;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    /// <summary>
    /// 合作企业
    /// </summary>
    [ExportMany]
    public class Partner : IFullAudit<Partner, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }
        #endregion

        #region 合作伙伴基本信息
        public string Ptnno { get; set; }
        public string Ptncname { get; set; }
        public string Ptnename { get; set; }
        public string Smpcname { get; set; }
        public string Smpename { get; set; }
        public string Remark { get; set; }
        #endregion

        public void Configure(IEntityMappingBuilder<Partner> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            nativeBuilder.HasDiscriminator<string>("Type")
                .HasValue<Supplier>("Supplier")
                .HasValue<Customer>("Customer");
        }
    }
}
