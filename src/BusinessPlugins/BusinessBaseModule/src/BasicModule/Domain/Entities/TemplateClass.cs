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
    /// 模板类
    /// </summary>
    [ExportMany]
    public class TemplateClass : IFullAudit<TemplateClass, Guid>
    {
        #region FullAudit接口实现

        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion
        #region 模板类基本属性
        /// <summary>
        /// 模板类名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 模板类标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        #endregion
        #region 依赖对象引用
        /// <summary>
        /// 模块
        /// </summary>
        public Guid ModuleId { get; set; }
        public Module Module { get; set; }
        /// <summary>
        /// 类对象
        /// </summary>
        public List<TemplateClassObject> ClassObjects { get; set; }
        /// <summary>
        /// 模板集合
        /// </summary>
        public List<Template> Templetes { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<TemplateClass> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //模块
            builder.HasMany(tc => tc.Module, m => m.TemplateClasses, tc => tc.ModuleId);
        }
        #endregion
    }
}
