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

namespace BusinessPlugins.ProductionModule.Domain.Entities
{
    /// <summary>
    /// 生产订单
    /// 下达日期[排产日期] 下达期间 基本开始日期 产前缓存时间 生产开始时间 加工时间 生产完成时间 产后缓存时间 基本完成时间[需求日期]
    /// </summary>
    [ExportMany]
    public class ManufactureOrder : IFullAudit<ManufactureOrder, Guid>
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
        public string MoCode { get; set; }
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
        /// 下达日期[排产日期]
        /// </summary>
        public DateTime IssuedDate { get; set; }
        /// <summary>
        /// 基本开始日期 =下达日期 + 计划边际码的下达期间
        /// </summary>
        public DateTime BasicStartDate { get; set; }
        /// <summary>
        /// 生产开始日期] = 基本开始日期 + 产向缓存时间
        /// </summary>
        public DateTime ProductionStartDate { get; set; }
        /// <summary>
        /// 生产完成日期 = 生产开始日期 + 加工时间
        /// </summary>
        public DateTime ProductionFinishDate { get; set; }
        /// <summary>
        /// 基本完成日期 = 生产完成日期 + 产后缓存时间
        /// </summary>
        public DateTime BasicFinishDate { get; set; }
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
        public List<ManufactureOrderItem> Items { get; set; }
        #endregion

        public void Configure(IEntityMappingBuilder<ManufactureOrder> builder)
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
