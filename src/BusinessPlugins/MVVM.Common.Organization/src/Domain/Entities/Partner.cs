using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.MVVM.Common.Organization.Domain.Entities
{
    /// <summary>
    /// 合作企业
    /// </summary>
    [ExportMany]
    public class Partner : IFullAudit<Partner, Guid>,
        ITreeStructType<Partner, Guid>
    {
        public Guid Id { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }

        /// <summary>
        /// 合作企业类型：客户，供应商，代理商等
        /// </summary>
        public string Type { get; set; }
        public string Ptnno { get; set; }
        public string Ptncname { get; set; }
        public string Ptnename { get; set; }
        public string Smpcname { get; set; }
        public string Smpename { get; set; }
        public string Remark { get; set; }


        public List<Partner> Childs { get; set; }
        public Guid RootId { get; set; }
        public Nullable<Guid> ParentId { get; set; }
        public Partner Parent { get; set; }

        public void Configure(IEntityMappingBuilder<Partner> builder)
        {
            var nativeBuilder = ((EntityTypeBuilder<Partner>)builder.NativeBuilder);
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            //builder.Map(p => p.ParentId, new EntityMappingOptions() { Nullable = true });
            //nativeBuilder.Property(p => p.ParentId).IsRequired(false);
        }
    }
}
