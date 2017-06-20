using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.Interfaces;
using ZKWeb.ORM.EFCore;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Entities
{
    /// <summary>
    /// BOM(Bill Of Material，物料清单)
    /// </summary>
    [ExportMany]
    public class Bom : IEntity<Guid>,
        IHaveCreateTime,
        IHaveUpdateTime,
        IHaveDeleted,
        IHaveOwnerTenant,
        IEntityMappingProvider<Bom>
    {

        //一个产品结构必须一个版次,一个结构有零或多个自身结点
        public Guid RootId { get; set; }
        public int Ord { get; set; }
        public int Level { get; set; }
        public double SingleTotal { get; set; }
        public double Total { get; set; } = 1;
        public double Cost { get; set; }
        public decimal NeedTotal { get; set; }
        public decimal MaoTotal { get; set; }
        public Decimal MrpTotal { get; set; }
        public decimal UsedKc { get; set; }
        public int Lft { get; set; }
        public int Rgt { get; set; }
        public bool IsVirtualPart { get; set; }
        public Nullable<Guid> ParentId { get; set; }
        public Bom Parent { get; set; }
        public List<Bom> Childs { get; set; } = new List<Bom>();
        public Guid NodeVersionId { get; set; }
        public ProductVersion NodeVersion { get; set; }
        public Guid RootVersionId { get; set; }
        public ProductVersion RootVersion { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }
        public bool Deleted { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }

        //主键
        public Guid Id { get; set; }

        public void Configure(IEntityMappingBuilder<Bom> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            nativeBuilder.HasOne(b => b.Parent)
                .WithMany(b => b.Childs)
                .HasForeignKey(b => b.ParentId);

            nativeBuilder.HasOne(b => b.NodeVersion)
                .WithMany(pv => pv.NodeRefs)
                .HasForeignKey(b => b.NodeVersionId);

            nativeBuilder.HasOne(b => b.RootVersion)
                .WithMany(pv => pv.RootRefs)
                .HasForeignKey(b => b.RootVersionId);
        }
    }
}
