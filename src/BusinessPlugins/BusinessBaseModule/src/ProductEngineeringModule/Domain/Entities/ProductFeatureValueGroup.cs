﻿using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Entities
{
    /// <summary>
    /// 产品特性值
    /// 特性A 值1
    /// 特性B 值2
    /// </summary>
    [ExportMany]
    public class ProductFeatureValueGroup : IFullAudit<ProductFeatureValueGroup, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion
        #region 产品特性值数据属性

        #endregion
        #region 依赖对象引用

        /// <summary>
        /// 产品特性值行
        /// </summary>
        public List<ProductFeatureValueItem> Items { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<ProductFeatureValueGroup> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
        }
        #endregion
    }
}
