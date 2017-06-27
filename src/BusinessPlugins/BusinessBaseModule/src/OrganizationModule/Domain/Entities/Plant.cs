﻿using BusinessPlugins.WarehouseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    /// <summary>
    /// 工厂
    /// </summary>
    [ExportMany]
    public class Plant : IFullAudit<Plant, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 工厂基本信息
        /// <summary>
        /// 工厂编码
        /// </summary>
        public string PlantCode { get; set; }
        /// <summary>
        /// 工厂名称
        /// </summary>
        public string PlantName { get; set; }

        public string Remark { get; set; }
        #endregion

        #region 依赖对象引用
        /// <summary>
        /// 公司代码
        /// </summary>
        public Guid CompanyCodeId { get; set; }
        public CompanyCode CompanyCode { get; set; }
        #endregion

        #region 依赖对象集合引用
        /// <summary>
        /// 存储地点
        /// 一个工厂有多个存储地点
        /// </summary>
        public List<StorageLocation> StorageLocations { get; set; }
        #endregion

        #region 实体配置
        public void Configure(IEntityMappingBuilder<Plant> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            nativeBuilder.HasOne(p => p.CompanyCode)
                .WithMany(cc => cc.plants)
                .HasForeignKey(p => p.CompanyCodeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
        #endregion
    }
}
