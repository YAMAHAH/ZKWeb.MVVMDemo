using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities.Interfaces;
using ZKWeb.ORM.EFCore;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Entities
{
    /// <summary>
    /// 产品版次
    /// </summary>
    [ExportMany]
    // [Table("productversions")]
    public class ProductVersion : IEntity<Guid>,
        IHaveCreateTime,
        IHaveUpdateTime,
        IHaveDeleted,
        IHaveOwnerTenant,
        IEntityMappingProvider<ProductVersion>
    {
        //一个产品版次需要一个产品,有零个或一个产品层结构
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string Version { get; set; } = "A0";
        public DateTime VersionDate { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }
        public bool Deleted { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }

        public List<Bom> RootRefs { get; set; } = new List<Bom>();
        public List<Bom> NodeRefs { get; set; } = new List<Bom>();


        public void Configure(IEntityMappingBuilder<ProductVersion> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(pv => pv.Id);
            builder.References(pv => pv.OwnerTenant);

            nativeBuilder.HasKey(pv => pv.Id);
            nativeBuilder.HasOne(pv => pv.Product)
                         .WithMany(p => p.ProductVersions)
                         .HasForeignKey(p => p.ProductId);
        }
    }
}
