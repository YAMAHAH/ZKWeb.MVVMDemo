using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 产品组
    /// </summary>
    [ExportMany]
    public class SalesDivision : IFullAudit<SalesDivision, Guid>
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
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        #endregion
        #region 依赖对象引用集合
        public List<SalesOrgToDivision> SalesOrganizations { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<SalesDivision> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
            builder.HasMany(d => d.SalesOrganizations);
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
