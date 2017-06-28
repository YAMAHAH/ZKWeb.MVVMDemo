using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 销售办公室
    /// </summary>
    [ExportMany]
    public class SalesOffice : IFullAudit<SalesOffice, Guid>
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
        public string SalesOfficeCode { get; set; }
        public string SalesOfficeName { get; set; }
        #endregion
        #region 依赖对象引用
        public List<SalesRegionToOffice> SalesRegions { get; set; }
        public List<SalesGroup> SalesGroups { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<SalesOffice> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
            builder.HasMany(o => o.SalesRegions);
            ////工厂
            //nativeBuilder.HasOne(i => i.Plant)
            //    .WithMany()
            //    .HasForeignKey(i => i.PlantId)
            //    .OnDelete(DeleteBehavior.Restrict);

            ////工作中心
            //nativeBuilder.HasOne(i => i.WorkCenter)
            //    .WithOne()
            //    .HasForeignKey<ProcessStep>(i => i.WorkCenterId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
        #endregion
    }
}
