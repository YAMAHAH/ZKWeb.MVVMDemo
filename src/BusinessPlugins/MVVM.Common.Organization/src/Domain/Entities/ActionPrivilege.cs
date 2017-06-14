﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.MVVM.Common.Organization.Domain.Entities
{
    /// <summary>
    /// 员工权限,跟角色权限一样
    /// </summary>
    [ExportMany]
    public class ActionPrivilege : IFullAudit<ActionPrivilege, Guid>
    {
        public Guid Id { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        public Guid TemplateId { get; set; }
        /// <summary>
        /// 模板对象ID
        /// </summary>
        public Guid TemplateObjectId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; set; }
        public Roles Role { get; set; }
        /// <summary>
        /// 员工ID
        /// </summary>
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        /// <summary>
        /// 岗位ID
        /// </summary>
        public Guid PostGroupId { get; set; }
        public PostGroup PostGroup { get; set; }
        /// <summary>
        /// 可视
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// 可用
        /// </summary>
        public bool Enable { get; set; }
        public void Configure(IEntityMappingBuilder<ActionPrivilege> builder)
        {
            var nativeBuilder = ((EntityTypeBuilder<ActionPrivilege>)builder.NativeBuilder);
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            nativeBuilder.HasOne(dp => dp.Employee)
              .WithMany(e => e.ActionPrivileges)
              .HasForeignKey(dp => dp.EmployeeId);

            nativeBuilder.HasOne(dp => dp.PostGroup)
                .WithMany(p => p.ActionPrivileges)
                .HasForeignKey(pd => pd.PostGroupId);

            nativeBuilder.HasOne(dp => dp.Role)
                .WithMany(r => r.ActionPrivileges)
                .HasForeignKey(pd => pd.RoleId);
        }
    }
}
