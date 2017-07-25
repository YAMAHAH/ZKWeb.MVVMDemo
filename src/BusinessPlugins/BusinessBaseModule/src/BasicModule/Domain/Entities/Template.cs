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
    /// 模板
    /// </summary>
    [ExportMany]
    public class Template : IFullAudit<Template, Guid>
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

        #endregion
        #region 依赖对象引用
        /// <summary>
        /// 模板类
        /// </summary>
        public Guid TemplateClassId { get; set; }
        public TemplateClass TemplateClass { get; set; }
        /// <summary>
        /// 懒加载模块
        /// </summary>
        public Nullable<Guid> LazyModuleId { get; set; }
        public LazyModule LazyModule { get; set; }
        /// <summary>
        /// 模板对象集合
        /// </summary>
        public List<TemplateObject> TemplateObjects { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<Template> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //模板类
            builder.HasMany(t => t.TemplateClass, tc => tc.Templetes, t => t.TemplateClassId);
            //懒加载模块
            builder.HasMany(t => t.LazyModule, lm => lm.Templates, t => t.LazyModuleId);
        }
        #endregion
    }
}
