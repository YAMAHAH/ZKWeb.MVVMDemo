using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Entities
{
    [ExportMany]
    public class ProductProcessRoute : IFullAudit<ProductProcessRoute, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        public void Configure(IEntityMappingBuilder<ProductProcessRoute> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            //产品版次
            nativeBuilder.HasOne(i => i.ProductVersion)
                .WithMany()
                .HasForeignKey(i => i.ProductVersionId)
                .OnDelete(DeleteBehavior.Restrict);
            //产品
            nativeBuilder.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            //车间
            nativeBuilder.HasOne(i => i.Department)
                .WithMany()
                .HasForeignKey(i => i.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
            //外协厂商
            nativeBuilder.HasOne(i => i.Partner)
                .WithMany()
                .HasForeignKey(i => i.PartnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        #endregion

        #region 产品工艺路线基本信息
        public string Remark { get; set; }
        #endregion
        #region 依赖对象引用

        public Nullable<Guid> PreProcessId { get; set; }
        public ProcessStep PreProcess { get; set; }
        public Nullable<Guid> PostProcessId { get; set; }
        public ProcessStep PostProcess { get; set; }
        /// <summary>
        /// 产品版次
        /// </summary>
        public Nullable<Guid> ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public Nullable<Guid> ProductId { get; set; }
        public Product Product { get; set; }
        /// <summary>
        /// 生产车间
        /// </summary>
        public Nullable<Guid> DepartmentId { get; set; }
        public Department Department { get; set; }
        /// <summary>
        /// 外协厂商
        /// </summary>
        public Nullable<Guid> PartnerId { get; set; }
        public Partner Partner { get; set; }
        #endregion
    }
}
