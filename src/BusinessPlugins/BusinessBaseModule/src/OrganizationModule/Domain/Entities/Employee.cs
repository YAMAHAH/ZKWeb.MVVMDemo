using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    /// <summary>
    /// 员工
    /// </summary>
    [ExportMany]
    public class Employee : IFullAudit<Employee, Guid>
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public string Remark { get; set; }
        public User User { get; set; }

        /// <summary>
        /// 员工所拥有的数据字段权限
        /// </summary>
        public IList<TemplatePrivilege> TemplatePrivileges { get; set; } = new List<TemplatePrivilege>();
        /// <summary>
        /// 员工所拥有的角色
        /// </summary>
        public IList<EmployeeRole> EmployeeRoles { get; set; } = new List<EmployeeRole>();
        /// <summary>
        /// 员工所拥有的岗位
        /// </summary>
        public IList<EmployeePostGroup> EmployeePostGroups { get; set; } = new List<EmployeePostGroup>();

        public void Configure(IEntityMappingBuilder<Employee> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            //双向配置
            nativeBuilder.HasOne(e => e.User)
               .WithOne(u => u.Employee)
               .HasForeignKey<User>(u => u.EmployeeId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
