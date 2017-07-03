﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities.Interfaces;
using ZKWeb.ORM.EFCore;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    /// <summary>
    /// 员工岗位组
    /// </summary>
    [ExportMany]
    public class EmployeePostGroup : IEntity<Guid>,
          IHaveDeleted,
          IHaveCreateTime,
          IHaveUpdateTime,
          IHaveOwnerTenant,
          IEntityMappingProvider<EmployeePostGroup>
    {
        public Guid Id { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        /// <summary>
        ///  租户ID
        /// </summary>
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

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

        public void Configure(IEntityMappingBuilder<EmployeePostGroup> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            nativeBuilder.HasOne(ep => ep.Employee)
                .WithMany(e => e.EmployeePostGroups)
                .HasForeignKey(ep => ep.EmployeeId);

            nativeBuilder.HasOne(ep => ep.PostGroup)
                .WithMany(p => p.EmployeePostGroups)
                .HasForeignKey(ep => ep.PostGroupId);
        }
    }
}