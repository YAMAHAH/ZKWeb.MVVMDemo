using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductionModule.Domain.Entities
{
    /// <summary>
    /// 生产订单
    /// </summary>
    [ExportMany]
    public class ProductionOrder : IFullAudit<ProductionOrder, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion

        #region 生产订单基本信息
        /// <summary>
        /// 生产订单编码
        /// </summary>
        public string ProductionOrderCode { get; set; }
        /// <summary>
        /// 登记日期
        /// </summary>
        public DateTime RecordDate { get; set; }
        /// <summary>
        /// 需求日期
        /// </summary>

        public DateTime NeedDate { get; set; }
        /// <summary>
        /// 排产日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }
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
        /// 工厂ID
        /// </summary>
        public Guid PlantId { get; set; }
        public Plant Plant { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid DptmId { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public Department Department { get; set; }
        #endregion

        #region 依赖对象集合引用
        public List<ProductionOrderItem> Items { get; set; }
        #endregion

        public void Configure(IEntityMappingBuilder<ProductionOrder> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            ////车间
            nativeBuilder.HasOne(i => i.Department)
                .WithMany()
                .HasForeignKey(i => i.DptmId)
                .OnDelete(DeleteBehavior.Restrict);

            //工厂
            nativeBuilder.HasOne(i => i.Plant)
                .WithMany()
                .HasForeignKey(i => i.PlantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
