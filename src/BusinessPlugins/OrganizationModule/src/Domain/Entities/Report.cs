using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.Interfaces;
using ZKWeb.ORM.EFCore;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    /// <summary>
    /// 报表
    /// </summary>
    [ExportMany]
    public class Report : IEntity<Guid>,
        IHaveCreateTime,
        IHaveUpdateTime,
        IHaveDeleted,
        IHaveOwnerTenant,
        IEntityMappingProvider<Report>
    {
        public string ReportName { get; set; }
        public string Creator { get; set; }
        public string Remark { get; set; }
        public bool IsReport { get; set; }
        public bool IsVirtualPart { get; set; }
        public bool IsLocal { get; set; }
        public byte[] ReportData { get; set; } = new byte[1];
        public Nullable<Guid> ParentId { get; set; }
        public Report Parent { get; set; }
        public Guid RootId { get; set; }
        public int Lft { get; set; }
        public int Rgt { get; set; }
        public int Level { get; set; }
        public int Ord { get; set; }
        public List<Report> Childs { get; set; } = new List<Report>();
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid Id { get; set; }
        public bool Deleted { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }

        public void Configure(IEntityMappingBuilder<Report> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(pv => pv.Id);
            builder.References(pv => pv.OwnerTenant);

            nativeBuilder
                .HasOne(r => r.Parent)
                .WithMany(r => r.Childs)
                .HasForeignKey(r => r.ParentId);
        }
    }
}
