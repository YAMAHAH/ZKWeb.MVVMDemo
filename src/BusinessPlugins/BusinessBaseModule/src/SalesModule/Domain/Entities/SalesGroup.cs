using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 销售组
    /// </summary>
    [ExportMany]
    public class SalesGroup : IFullAudit<SalesGroup, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion

        #region 主数据属性
        public string SalesGroupCode { get; set; }
        public string SalesGroupName { get; set; }
        #endregion


        #region 依赖对象引用
        public Guid SalesOfficeId { get; set; }
        public SalesOffice SalesOffice { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<SalesGroup> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            nativeBuilder.HasOne(g => g.SalesOffice)
                .WithMany(o => o.SalesGroups)
                .HasForeignKey(g => g.SalesOfficeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        #endregion
    }
}
