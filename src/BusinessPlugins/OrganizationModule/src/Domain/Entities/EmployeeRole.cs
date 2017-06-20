using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.Interfaces;
using ZKWeb.ORM.EFCore;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    /// <summary>
    /// 员工角色
    /// </summary>
    [ExportMany]
    public class EmployeeRole : IEntity<Guid>,
         IHaveCreateTime,
         IHaveUpdateTime,
         IHaveDeleted,
         IHaveOwnerTenant,
         IEntityMappingProvider<EmployeeRole>

    {
        public Guid Id { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }


        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public Guid RoleId { get; set; }
        public Roles Role { get; set; }

        public void Configure(IEntityMappingBuilder<EmployeeRole> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            nativeBuilder.HasOne(er => er.Employee)
                .WithMany(e => e.EmployeeRoles)
                .HasForeignKey(er => er.EmployeeId);

            nativeBuilder.HasOne(er => er.Role)
                .WithMany(e => e.EmployeeRoles)
                .HasForeignKey(er => er.RoleId);
        }

    }
}
