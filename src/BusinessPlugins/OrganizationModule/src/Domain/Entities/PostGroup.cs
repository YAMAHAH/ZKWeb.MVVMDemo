using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.ORM.EFCore;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    /// <summary>
    /// 岗位组
    /// </summary>
    [ExportMany]
    public class PostGroup : IFullAudit<PostGroup, Guid>,
        ITreeStructType<PostGroup, Guid>
    {
        #region IFullAduit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }
        #endregion

        #region 树型结构
        public List<PostGroup> Childs { get; set; }
        public Guid RootId { get; set; }
        public Guid? ParentId { get; set; }
        public PostGroup Parent { get; set; }
        #endregion
        /// <summary>
        /// 岗位组所拥有的数据字段权限
        /// </summary>
        public IList<DataFieldPrivilege> DataFieldPrivileges { get; set; } = new List<DataFieldPrivilege>();
        /// <summary>
        /// 岗位组所拥有的功能权限
        /// </summary>
        public IList<ActionPrivilege> ActionPrivileges { get; set; } = new List<ActionPrivilege>();
        /// <summary>
        /// 岗位所拥有的角色
        /// </summary>
        public IList<PostGroupRole> PostGroupRoles { get; set; } = new List<PostGroupRole>();
        /// <summary>
        /// 岗位所拥有的员工
        /// </summary>
        public IList<EmployeePostGroup> EmployeePostGroups { get; set; } = new List<EmployeePostGroup>();
        /// <summary>
        /// 岗位所拥有的部门
        /// </summary>

        public IList<PostGroupDepartment> PostGroupDepartments { get; set; } = new List<PostGroupDepartment>();

        public void Configure(IEntityMappingBuilder<PostGroup> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });
        }
    }
}
