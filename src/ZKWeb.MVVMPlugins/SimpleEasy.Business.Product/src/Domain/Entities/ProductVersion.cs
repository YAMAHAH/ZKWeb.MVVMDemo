using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.Interfaces;

namespace ZKWeb.MVVMPlugins.SimpleEasy.Business.Product.src.Domain.Entities
{
    [Table("productversions")]
    public class ProductVersion : IEntity<Guid>,
        IHaveCreateTime,
        IHaveUpdateTime,
        IHaveDeleted,
        IHaveOwnerTenant,
        IEntityMappingProvider<ProductVersion>
    {
        //一个产品版次需要一个产品,有零个或一个产品层结构

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string Version { get; set; } = "A0";
        public DateTime VersionDate { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }
        public bool Deleted { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid Id { get; set; }

        public void Configure(IEntityMappingBuilder<ProductVersion> builder)
        {
            var nativeBuilder = ((EntityTypeBuilder<ProductVersion>)builder.NativeBuilder);
            builder.Id(pv => pv.Id);
            builder.References(pv => pv.OwnerTenant);
            nativeBuilder.HasKey(pv => pv.Id);
            nativeBuilder.HasOne(pv => pv.Product)
                         .WithMany(p => p.ProductVersions)
                         .HasForeignKey(p => p.ProductId);
        }
    }
}
