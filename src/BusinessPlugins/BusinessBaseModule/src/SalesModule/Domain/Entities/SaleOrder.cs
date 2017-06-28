using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{

    [ExportMany]
    public class SaleOrder : IEntity<Guid>,
    IHaveCreateTime,
    IHaveUpdateTime,
    IHaveDeleted,
    IHaveOwnerTenant,
    IEntityMappingProvider<SaleOrder>
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }
        public string SaleOrderNo { get; set; }
        public string CustomerOrderNo { get; set; }
        public DateTime? SaleOrderDelivery { get; set; }
        public Guid? PartnerId { get; set; }
        public bool IsDone { get; set; }
        public bool IsCancel { get; set; }
        public List<SaleOrderItem> SaleOrderDetails { get; set; }

        public void Configure(IEntityMappingBuilder<SaleOrder> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            //nativeBuilder.Property(p => p.CreateTime)
            //    .ValueGeneratedOnAdd();
            //nativeBuilder.Property(p => p.UpdateTime)
            //    .ValueGeneratedOnAddOrUpdate();
        }
    }
}
