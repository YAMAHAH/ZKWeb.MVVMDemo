using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SaleManagerSystem.src.Domain.Entities
{

    [ExportMany]
    public class SaleOrderHeader : IEntity<Guid>,
    IHaveCreateTime,
    IHaveUpdateTime,
    IHaveDeleted,
    IHaveOwnerTenant,
    IEntityMappingProvider<SaleOrderHeader>
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }
        public string SaleOrderNo { get; set; }
        public string CustomerOrderNo { get; set; }
        public DateTime?  SaleOrderDelivery { get; set; }
        public Guid? PartnerId { get; set; }
        public bool SF_Finish { get; set; }
        public bool SF_Cancel { get; set; }
        public List<SaleOrderDetail> SaleOrderDetails { get; set; }

        public void Configure(IEntityMappingBuilder<SaleOrderHeader> builder)
        {
            var nativeBuilder = ((EntityTypeBuilder<SaleOrderHeader>)builder.NativeBuilder);
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            //nativeBuilder.Property(p => p.CreateTime)
            //    .ValueGeneratedOnAdd();
            //nativeBuilder.Property(p => p.UpdateTime)
            //    .ValueGeneratedOnAddOrUpdate();
        }
    }
}
