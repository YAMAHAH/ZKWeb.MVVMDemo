using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductionScheduleModule.Domain.Entities;
using BusinessPlugins.WarehouseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 转储订单
        /// </summary>
        public Nullable<Guid> TransferOrderId { get; set; }
        public TransferOrder TransferOrder { get; set; }
        /// <summary>
        /// 领料申请单
        /// </summary>
        public Nullable<Guid> MatReqId { get; set; }
        public MaterialRequisition MatReq { get; set; }
        /// <summary>
        /// 计划订单
        /// </summary>
        public Nullable<Guid> PldOrdId { get; set; }
        public PlannedOrder PldOrd { get; set; }

        #endregion

        #region 依赖对象集合引用
        public List<ManufactureOrderItem> Items { get; set; }
        #endregion

        public void Configure(IEntityMappingBuilder<ManufactureOrder> builder)
        {
            //主键
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(p => p.OwnerTenant, p => p.OwnerTenantId);
            //车间
            builder.HasMany(i => i.Department, i => i.DptmId);
            //工厂
            builder.HasMany(i => i.Plant, i => i.PlantId);
            //领料申请单
            builder.HasMany(i => i.MatReq, i => i.MatReqId);
            //转储订单
            builder.HasMany(i => i.TransferOrder, i => i.TransferOrderId);
            //计划订单
            builder.HasMany(i => i.PldOrd, i => i.PldOrdId);
        }
    }
}
