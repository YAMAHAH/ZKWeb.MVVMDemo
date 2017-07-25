using BusinessPlugins.BasicModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    /// <summary>
    /// 角色权限
    /// </summary>
    [ExportMany]
    public class TemplatePrivilege : IFullAudit<TemplatePrivilege, Guid>
    {
        #region IFullAudit接口实现
        public Guid Id { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }
        #endregion
        #region 依赖对象引用
        /// <summary>
        /// 模板
        /// </summary>
        public Guid TemplateId { get; set; }
        public Template Template { get; set; }

        /// <summary>
        /// 模板对象
        /// </summary>
        public Guid TemplateObjectId { get; set; }
        public TemplateObject TemplateObject { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public Nullable<Guid> RoleId { get; set; }
        public Roles Role { get; set; }
        /// <summary>
        /// 员工
        /// </summary>
        public Nullable<Guid> EmployeeId { get; set; }
        public Employee Employee { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        public Nullable<Guid> PostGroupId { get; set; }
        public PostGroup PostGroup { get; set; }
        #endregion

        #region 模板权限基本属性
        /// <summary>
        /// 模板对象类型
        /// 功能/数据字段
        /// </summary>
        public TemplateObjectType TemplateObjectType { get; set; }
        /// <summary>
        /// 可查
        /// </summary>
        public bool Queryable { get; set; }
        /// <summary>
        /// 可视
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// 可编辑
        /// </summary>
        public bool Editable { get; set; }
        /// <summary>
        /// 可用
        /// </summary>
        public bool Enable { get; set; }
        #endregion
        public void Configure(IEntityMappingBuilder<TemplatePrivilege> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            //主键
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(p => p.OwnerTenant, p => p.OwnerTenantId);
            //员工
            builder.HasMany(dp => dp.Employee, e => e.TemplatePrivileges, dp => dp.EmployeeId);
            //岗位
            builder.HasMany(dp => dp.PostGroup, p => p.TemplatePrivileges, pd => pd.PostGroupId);
            //角色
            builder.HasMany(dp => dp.Role, r => r.TemplatePrivileges, pd => pd.RoleId);
        }
    }
}
