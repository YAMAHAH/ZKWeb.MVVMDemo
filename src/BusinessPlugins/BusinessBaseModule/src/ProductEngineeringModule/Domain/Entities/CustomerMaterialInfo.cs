using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.SalesModule.Domain.Entities;
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
        /// 销售组织
        /// </summary>
        public Guid SalesOrgId { get; set; }
        public SalesOrganization SalesOrganization { get; set; }
        /// <summary>
        /// 分销渠道
        /// </summary>
        public Guid DistrChanelId { get; set; }
        public DistributionChannel DistributionChannel { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public Guid CustomerId { get; set; }
        public Partner Customer { get; set; }

        /// <summary>
        /// 产品版次
        /// </summary>
        public Guid ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        #endregion

        #region 实体配置
        public void Configure(IEntityMappingBuilder<CustomerMaterialInfo> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            //产品版次
            builder.HasMany(i => i.ProductVersion, i => i.ProductVersionId);

            //合作伙伴
            builder.HasMany(i => i.Customer, i => i.CustomerId);

        }
        #endregion
    }
}
