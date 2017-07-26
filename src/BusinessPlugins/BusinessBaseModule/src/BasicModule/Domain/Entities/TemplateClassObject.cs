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
    /// 模板类对象
    /// </summary>
    [ExportMany]
    public class TemplateClassObject : IFullAudit<TemplateClassObject, Guid>
    {
        #region FullAudit接口实现

        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion
        #region 模板类对象基本属性

        public TemplateObjectType ObjectType { get; set; }
        /// <summary>
        /// 对象名称
        /// </summary>
        public string ObjectName { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string ObjectAlias { get; set; }
        /// <summary>
        /// 对象标题
        /// </summary>
        public string ObjectTitle { get; set; }
        /// <summary>
        /// 可查
        /// </summary>
        public bool Queryable { get; set; }
        /// <summary>
        /// 必选
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 可视
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// 可用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 可编
        /// </summary>
        public bool Editable { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 默认组件类型
        /// </summary>
        public string CommponentType { get; set; }
        #endregion
        #region 依赖对象引用
        public Guid TempClsId { get; set; }
        public TemplateClass TemplateClass { get; set; }

        public Nullable<Guid> ParentId { get; set; }
        public TemplateClassObject Parent { get; set; }
        /// <summary>
        /// 子对象
        /// </summary>
        public List<TemplateClassObject> Childs { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<TemplateClassObject> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //模板类
            builder.HasMany(tc => tc.TemplateClass, tc => tc.TempClsId);
            //自身结点
            builder.HasMany(to => to.Parent, to => to.Childs, to => to.ParentId);
        }
        #endregion
    }

}
