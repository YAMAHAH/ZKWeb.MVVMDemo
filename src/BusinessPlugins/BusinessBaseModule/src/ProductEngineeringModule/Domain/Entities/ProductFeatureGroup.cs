using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Entities
{
    /// <summary>
    /// 产品特性组
    /// 有多个产品特性
    /// </summary>
    [ExportMany]
    public class ProductFeatureGroup : IFullAudit<ProductFeatureGroup, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 产品特性值行数据属性
        /// <summary>
        /// 特性组
        /// </summary>
        public string FeatureGroupNumber { get; set; }
        /// <summary>
        /// 特性组名称
        /// </summary>
        public string FeatureGroupName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion
        #region 依赖对象引用
        /// <summary>
        /// 产品特性
        /// </summary>
        public Guid ProdFeatId { get; set; }
        public List<ProductFeature> ProductFeature { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<ProductFeatureGroup> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);


        }
        #endregion
    }
}
