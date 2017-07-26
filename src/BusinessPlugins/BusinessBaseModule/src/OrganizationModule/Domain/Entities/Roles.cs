using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    /// <summary>
    /// 角色
    /// </summary>
    [ExportMany]
    public class Roles : IFullAudit<Roles, Guid>, ITreeStructType<Roles, Guid>
    {
        #region IFullAduit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }
        #endregion

        public string RoleName { get; set; }
        public string Remark { get; set; }

        #region 树型结构
        public List<Roles> Childs { get; set; }
        public Guid RootId { get; set; }
        public Guid? ParentId { get; set; }
        public Roles Parent { get; set; }
        #endregion

        #region 预排序遍历树算法
        /// <summary>
        /// 左结点序号
        /// Modified Preorder Tree Traversal
        /// </summary>
        public int Lft { get; set; }
        /// <summary>
        /// 右结点序号
        /// Modified Preorder Tree Traversal
        /// </summary>
        public int Rgt { get; set; }
        /// <summary>
        /// 结点所在的层次
        /// </summary>
        public int Level { get; set; }
        #endregion

        /// <summary>
        /// 角色所拥有的数据字段权限
        /// </summary>
        public List<TemplatePrivilege> TemplatePrivileges { get; set; }
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
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
        }
    }
}
