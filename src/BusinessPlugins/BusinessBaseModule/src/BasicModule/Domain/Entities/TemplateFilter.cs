using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Domain.Entities
{
    /// <summary>
    /// 模板查询过滤器
    /// </summary>
    [ExportMany]
    public class TemplateFilter : IFullAudit<TemplateFilter, Guid>
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
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 查询条件JSON字符串
        /// </summary>
        public string ConditionJson { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 过滤器ID
        /// </summary>
        public Guid FilterId { get; set; }
        public string FilterFullName { get; set; }

        #endregion

        #region 依赖对象
        /// <summary>
        /// 模板
        /// </summary>
        public Guid TempId { get; set; }
        public Template Template { get; set; }
        /// <summary>
        /// 员工
        /// </summary>
        public Nullable<Guid> EmpId { get; set; }
        public Employee Employee { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>

        public Nullable<Guid> PostGroupId { get; set; }
        public PostGroup PostGroup { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public Nullable<Guid> RoleId { get; set; }
        public Roles Role { get; set; }

        #endregion
        #region 实体配置
        public void Configure(IEntityMappingBuilder<TemplateFilter> builder)
        {
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(p => p.OwnerTenant, p => p.OwnerTenantId);
            //模板
            builder.HasMany(t => t.Template, p => p.TempId);
            //员工
            builder.HasMany(t => t.Employee, p => p.EmpId);
            //用户组
            builder.HasMany(t => t.PostGroup, p => p.PostGroupId);
            //角色
            builder.HasMany(t => t.Role, p => p.RoleId);
        }
        #endregion
    }
}
