using BusinessPlugins.BaseModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.PurchaseModule.Domain.Entities
{
    /// <summary>
    /// 采购订单
    /// </summary>
    [ExportMany]
    public class PurchaseOrder : IFullAudit<PurchaseOrder, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 采购订单信息
        /// <summary>
        /// 采购类型
        /// </summary>
        public PurchaseType PurchaseType { get; set; }
        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime NeedDate { get; set; }
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
        /// 工厂
        /// </summary>
        public Guid PlantId { get; set; }
        public Plant Plant { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public Guid VendorId { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public Partner Vendor { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public Guid DptmId { get; set; }
        public Department Department { get; set; }
       
        /// <summary>
        /// 采购物料行
        /// </summary>
        public List<PurchaseOrderItem> Items { get; set; }
        #endregion

        public void Configure(IEntityMappingBuilder<PurchaseOrder> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            ////合作合伴
            nativeBuilder.HasOne(i => i.Vendor)
                .WithMany()
                .HasForeignKey(i => i.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            ////部门
            nativeBuilder.HasOne(i => i.Department)
                .WithMany()
                .HasForeignKey(i => i.DptmId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
