using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.MVVM.Common.Organization.Domain.Entities
{
    /// <summary>
    /// 岗位部门
    /// </summary>
    [ExportMany]
    public class PostGroupDepartment : IFullAudit<PostGroupDepartment, Guid>
    {
        public Guid Id { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }

        public Guid PostGroupId { get; set; }
        public PostGroup PostGroup { get; set; }

        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public void Configure(IEntityMappingBuilder<PostGroupDepartment> builder)
        {
            var nativeBuilder = ((EntityTypeBuilder<PostGroupDepartment>)builder.NativeBuilder);
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            nativeBuilder.HasOne(pd => pd.PostGroup)
                .WithMany(p => p.PostGroupDepartments)
                .HasForeignKey(pd => pd.PostGroupId);

            nativeBuilder.HasOne(pd => pd.Department)
                .WithMany(d => d.PostGroupDepartments)
                .HasForeignKey(pd => pd.DepartmentId);
        }
    }
}
