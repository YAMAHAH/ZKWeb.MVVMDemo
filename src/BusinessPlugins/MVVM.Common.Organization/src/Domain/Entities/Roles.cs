using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.Interfaces;
using ZKWeb.ORM.EFCore;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.MVVM.Common.Organization.Domain.Entities
{
    /// <summary>
    /// 角色
    /// </summary>
    [ExportMany]
    public class Roles : IEntity<Guid>,
         IHaveCreateTime,
         IHaveUpdateTime,
         IHaveDeleted,
         IHaveOwnerTenant,
         IEntityMappingProvider<Roles>
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }

        public string RoleName { get; set; }
        public string Remark { get; set; }

        /// <summary>
        /// 角色所拥有的数据字段权限
        /// </summary>
        public List<DataFieldPrivilege> DataFieldPrivileges { get; set; }
        /// <summary>
        /// 角色所拥有的功能权限
        /// </summary>
        public List<ActionPrivilege> ActionPrivileges { get; set; }
        /// <summary>
        /// 角色所拥有的员工
        /// </summary>
        public List<EmployeeRole> EmployeeRoles { get; set; }
        /// <summary>
        /// 角色所拥有的岗位
        /// </summary>
        public List<PostGroupRole> PostGroupRoles { get; set; }

        public void Configure(IEntityMappingBuilder<Roles> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();

            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });
        }
    }
}
