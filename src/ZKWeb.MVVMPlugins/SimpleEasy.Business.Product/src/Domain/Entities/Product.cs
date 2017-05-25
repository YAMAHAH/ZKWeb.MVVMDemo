using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ZKWeb.MVVMPlugins.SimpleEasy.Business.Product.src.Domain.Entities
{
    /// <summary>
    /// 用户
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
            var nativeBuilder = ((EntityTypeBuilder<Product>)builder.NativeBuilder);
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });
            nativeBuilder.ToTable("products");
            nativeBuilder.Property(p => p.ProductNo).IsRequired().HasColumnName("ProductName");
            nativeBuilder.Property(p => p.ProductName).IsRequired();
            nativeBuilder.Property(p => p.ProductDesc).IsRequired();
            nativeBuilder.Property(p => p.Dw).IsRequired();
        }
    }
}
