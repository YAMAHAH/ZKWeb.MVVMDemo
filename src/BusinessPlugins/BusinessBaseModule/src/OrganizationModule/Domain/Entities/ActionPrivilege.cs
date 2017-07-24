using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    /// <summary>
    /// 功能权限
    /// </summary>
    [ExportMany]
    public class ActionPrivilege : IFullAudit<ActionPrivilege, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion
        #region 基本属性
        /// <summary>
        /// 可视
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// 可用
        /// </summary>
        public bool Enable { get; set; }
        #endregion
        #region 依赖对象引用
        /// <summary>
        /// 模板
        /// </summary>
        public Guid TemplateId { get; set; }
        /// <summary>
        /// 模板对象
        /// </summary>
        public Guid TemplateObjectId { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public Guid RoleId { get; set; }
        public Roles Role { get; set; }
        /// <summary>
        /// 员工
        /// </summary>
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public Guid PostGroupId { get; set; }
        public PostGroup PostGroup { get; set; }
        #endregion

        public void Configure(IEntityMappingBuilder<ActionPrivilege> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            //主键
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(p => p.OwnerTenant, p => p.OwnerTenantId);
            //员工
            builder.HasMany(dp => dp.Employee, e => e.ActionPrivileges, dp => dp.EmployeeId);
            //岗位
            builder.HasMany(dp => dp.PostGroup, p => p.ActionPrivileges, pd => pd.PostGroupId);
            //角色
            builder.HasMany(dp => dp.Role, r => r.ActionPrivileges, pd => pd.RoleId);
        }
    }
}
