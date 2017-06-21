using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities.Interfaces;
using ZKWeb.ORM.EFCore;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    [ExportMany]
    public class SaleOrderDetail : IEntity<Guid>,
     IHaveCreateTime,
     IHaveUpdateTime,
     IHaveDeleted,
     IHaveOwnerTenant,
     IEntityMappingProvider<SaleOrderDetail>
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }

        public int Order { get; set; }
        public Guid ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        public double OrderTotal { get; set; }
        public double OrderFinishTotal { get; set; }
        public double OrderLeftTotal { get; set; }
        public Guid SaleOrderHeaderId { get; set; }
        public SaleOrderHeader SaleOrderHeader { get; set; }
        public double Price { get; set; }
        public double CostPrice { get; set; }
        public string Rem { get; set; }
        public bool SF_MxFin { get; set; }

        public void Configure(IEntityMappingBuilder<SaleOrderDetail> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            nativeBuilder.HasOne(mx => mx.SaleOrderHeader)
                .WithMany(header => header.SaleOrderDetails)
                .HasForeignKey(h => h.SaleOrderHeaderId);
        }
    }
}
