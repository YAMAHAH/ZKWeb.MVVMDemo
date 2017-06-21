using System;
using System.Collections.Generic;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Entities
{
    /// <summary>
    /// 产品
    /// </summary>
    [ExportMany]
    public class Product :
        IEntity<Guid>,
        IHaveCreateTime,
        IHaveUpdateTime,
        IHaveDeleted,
        IHaveOwnerTenant,
        IEntityMappingProvider<Product>
    {
        public bool Deleted { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid Id { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }
        public string ProductNo { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string Dw { get; set; }
        public List<ProductVersion> ProductVersions { get; set; } = new List<ProductVersion>();

        public void Configure(IEntityMappingBuilder<Product> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });
            //nativeBuilder.ToTable("products");

            nativeBuilder.Property(p => p.ProductNo).IsRequired();
            nativeBuilder.Property(p => p.ProductName).IsRequired();
            nativeBuilder.Property(p => p.ProductDesc).IsRequired();
            nativeBuilder.Property(p => p.Dw).IsRequired();
        }
    }
}
