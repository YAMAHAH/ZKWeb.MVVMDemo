using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 客户订单
    /// 客户,客户单号,客户交期
    /// </summary>
    [ExportMany]
    public class SaleOrder : IFullAudit<SaleOrder, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion


        #region 主数据属性
        /// <summary>
        /// 登记日期
        /// </summary>
        public DateTime RecordDate { get; set; }
        /// <summary>
        /// 销售订单号码
        /// </summary>
        public string SaleOrderNo { get; set; }
        /// <summary>
        /// 客户订单号码
        /// </summary>
        public string CustomerOrderNumber { get; set; }
        /// <summary>
        /// 订单交期
        /// </summary>
        public DateTime OrderDelivery { get; set; }
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
        /// 销达方
        /// </summary>
        public Guid SoldToId { get; set; }
        public Partner SoldTo { get; set; }
        /// <summary>
        /// 送达方
        /// 如果为空则取售达方
        /// </summary>
        public Nullable<Guid> ShipToId { get; set; }
        public Partner ShipTo { get; set; }
        /// <summary>
        /// 销售部门
        /// </summary>
        public Nullable<Guid> DptmId { get; set; }
        public Department SalesDepartment { get; set; }
        /// <summary>
        /// 销售区域
        /// </summary>
        public Guid SalesRegionId { get; set; }
        public SalesRegion SalesRegion { get; set; }
        /// <summary>
        /// 销售组
        /// </summary>
        public Nullable<Guid> SalesGroupId { get; set; }
        public SalesGroup SalesGroup { get; set; }

        /// <summary>
        /// 项目行
        /// </summary>
        public List<SaleOrderItem> Items { get; set; }
        #endregion


        public void Configure(IEntityMappingBuilder<SaleOrder> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
            //售达方
            builder.HasMany(o => o.SoldTo, o => o.SoldToId);
            //送达方
            builder.HasMany(o => o.ShipTo, o => o.ShipToId);
            //部门
            builder.HasMany(o => o.SalesDepartment, o => o.DptmId);
            //销售区域
            builder.HasMany(o => o.SalesRegion, o => o.SalesRegionId);
            //销售组
            builder.HasMany(o => o.SalesGroup, o => o.SalesGroupId);
            //nativeBuilder.Property(p => p.CreateTime)
            //    .ValueGeneratedOnAdd();
            //nativeBuilder.Property(p => p.UpdateTime)
            //    .ValueGeneratedOnAddOrUpdate();
        }
    }
}
