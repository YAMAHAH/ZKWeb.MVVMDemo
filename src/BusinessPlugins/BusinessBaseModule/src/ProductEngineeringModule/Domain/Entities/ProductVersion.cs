using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Entities
{
    /// <summary>
    /// 产品版次 Revision Level
    /// </summary>
    [ExportMany]
    public class ProductVersion : IFullAudit<ProductVersion, Guid>
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

        public List<StandardBom> StandBomRootRefs { get; set; } = new List<StandardBom>();
        public List<StandardBom> StandBomNodeRefs { get; set; } = new List<StandardBom>();

        public List<ProductOrderBom> ProdOrdBomRootRefs { get; set; } = new List<ProductOrderBom>();
        public List<ProductOrderBom> ProdOrdBomNodeRefs { get; set; } = new List<ProductOrderBom>();

        public void Configure(IEntityMappingBuilder<ProductVersion> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(v => v.Id);
            builder.References(v => v.OwnerTenant);

            nativeBuilder.HasKey(v => v.Id);
            nativeBuilder.HasOne(v => v.Product)
                         .WithMany(p => p.ProductVersions)
                         .HasForeignKey(p => p.ProductId);
        }
    }
}
