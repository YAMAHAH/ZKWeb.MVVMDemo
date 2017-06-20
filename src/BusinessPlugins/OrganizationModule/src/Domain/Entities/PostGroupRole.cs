using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.ORM.EFCore;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    /// <summary>
    /// 岗位组角色
    /// </summary>
    [ExportMany]
    public class PostGroupRole : IFullAudit<PostGroupRole, Guid>
    {
        public Guid Id { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }

        public Guid PostGroupId { get; set; }
        public PostGroup PostGroup { get; set; }
        public Guid RoleId { get; set; }
        public Roles Role { get; set; }

        public void Configure(IEntityMappingBuilder<PostGroupRole> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            nativeBuilder.HasOne(pr => pr.PostGroup)
                .WithMany(p => p.PostGroupRoles)
                .HasForeignKey(pr => pr.PostGroupId);

            nativeBuilder.HasOne(pr => pr.Role)
                .WithMany(r => r.PostGroupRoles)
                .HasForeignKey(pr => pr.RoleId);
        }
    }
}
