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
    /// <summary>
    /// 客户信息
    /// </summary>
    [ExportMany]
    public class CustomerMaterialInfo : IFullAudit<CustomerMaterialInfo, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 客户信息信息
        /// <summary>
        /// 客户零件编号
        /// </summary>
        public string CustomerPartNo { get; set; }
        /// <summary>
        /// 客户零件名称
        /// </summary>
        public string CustomerPartName { get; set; }
        /// <summary>
        /// 客户零件规格
        /// </summary>
        public string CustomerSpecification { get; set; }
        /// <summary>
        /// 备注
        /// 获取最新版次的客户信息和当前版次的信息
        /// </summary>
        public string Remark { get; set; }
        #endregion
        #region 依赖对象引用
        /// <summary>
        /// 产品版次
        /// </summary>
        public Guid ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        /// <summary>
        /// 合作伙伴
        /// </summary>
        public Guid PartnerId { get; set; }
        public Partner Partner { get; set; }
        #endregion

        #region 实体配置
        public void Configure(IEntityMappingBuilder<CustomerMaterialInfo> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

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
 
            //合作伙伴
            nativeBuilder.HasOne(i => i.Partner)
                .WithMany()
                .HasForeignKey(i => i.PartnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        #endregion
    }
}
