using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.WarehouseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWeb.Database;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 入库单基类
    /// </summary>
    public class StockIn : IFullAudit<StockIn, Guid>
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        /// <summary>
        /// 登记日期
        /// </summary>
        public DateTime RecordDate { get; set; }
        /// <summary>
        /// 入库单号
        /// </summary>
        public string StockInNo { get; set; }
        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime StockInDate { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public Guid Ckid { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse { get; set; }
        /// <summary>
        /// 源单据号
        /// </summary>
        public string OriginalTicketNo { get; set; }
        /// <summary>
        /// 伙伴ID
        /// </summary>
        public Nullable<Guid> PartnerId { get; set; }
        /// <summary>
        /// 合作伙伴
        /// </summary>
        public Partner Partner { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid DptmId { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public Department Department { get; set; }
        /// <summary>
        /// 单据备注
        /// </summary>
        public string Remark { get; set; }

        public void Configure(IEntityMappingBuilder<StockIn> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

        }
    }
}
