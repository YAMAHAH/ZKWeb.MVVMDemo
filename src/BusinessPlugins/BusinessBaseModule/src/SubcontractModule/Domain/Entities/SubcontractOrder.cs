using BusinessPlugins.BasicModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.SalesModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SubcontractModule.Domain.Entities
{
    /// <summary>
    /// 外包订单
    /// </summary>
    [ExportMany]
    public class SubcontractOrder : IFullAudit<SubcontractOrder, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 外包订单信息

        /// <summary>
        /// 外包单号
        /// </summary>
        public string OutsourcingNo { get; set; }
        /// <summary>
        /// 登记日期
        /// </summary>
        public DateTime RecordDate { get; set; }
        /// <summary>
        /// 交货日期
        /// </summary>

        public DateTime? DeliveryDate { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsDone { get; set; }
        /// <summary>
        /// 是否取消
        /// </summary>
        public bool IsCancel { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion

        #region 依赖对象引用

        /// <summary>
        /// 伙伴ID
        /// </summary>
        public Guid SubcontractorId { get; set; }
        /// <summary>
        /// 合作伙伴
        /// </summary>
        public Partner Subcontractor { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid DptmId { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public Department Department { get; set; }
        /// <summary>
        /// 销售订单
        /// </summary>
        public Nullable<Guid> SaleOrderItemId { get; set; }
        public SalesOrderItem SaleOrderItem { get; set; }

        /// <summary>
        /// 外包行
        /// </summary>
        public List<SubcontractOrderItem> Items { get; set; }

        #endregion

        public void Configure(IEntityMappingBuilder<SubcontractOrder> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            ////合作合伴
            nativeBuilder.HasOne(i => i.Subcontractor)
                .WithMany()
                .HasForeignKey(i => i.SubcontractorId)
                .OnDelete(DeleteBehavior.Restrict);

            ////部门
            nativeBuilder.HasOne(i => i.Department)
                .WithMany()
                .HasForeignKey(i => i.DptmId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
