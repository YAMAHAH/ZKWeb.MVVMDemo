using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Domain.Entities
{
    /// <summary>
    /// 模块
    /// </summary>
    [ExportMany]
    public class LazyModule : IFullAudit<LazyModule, Guid>
    {
        #region FullAudit接口实现

        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 模板基本属性
        /// <summary>
        /// 模板代号
        /// </summary>
        public string LazyModuleCode { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string LazyModuleName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion
        #region 依赖对象引用
        public List<Template> Templates { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<LazyModule> builder)
        {
            //主键
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
        }
        #endregion
    }
}
