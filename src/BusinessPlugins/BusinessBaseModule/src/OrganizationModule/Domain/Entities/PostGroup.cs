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
    /// 岗位组
    /// </summary>
    [ExportMany]
    public class PostGroup : IFullAudit<PostGroup, Guid>, ITreeStructType<PostGroup, Guid>
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
        /// 岗位组所拥有的数据字段权限
        /// </summary>
        public IList<TemplatePrivilege> TemplatePrivileges { get; set; } = new List<TemplatePrivilege>();
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
            //主键
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(p => p.OwnerTenant, p => p.OwnerTenantId);
            //创建索引
            nativeBuilder.HasIndex(r => r.RootId);
        }
    }
}
