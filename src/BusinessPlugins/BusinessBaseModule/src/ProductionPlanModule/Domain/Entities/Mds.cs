using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;


namespace BusinessPlugins.ProductionPlanModule.Domain.Entities
{
    /// <summary>
    /// 主需求计划
    /// 来源:销售订单,销售预测,库存补充请求单,手工录入
    /// MasterDemandSchedule
    /// 提供两种方案:
    /// 按SO + PO + 交期 + 产品 + 特性组 + SOBomId汇总
    /// 按交期 + 产品 + 特性组 + SOBomId汇总
    /// </summary>
    [ExportMany]
    public class Mds : IFullAudit<Mds, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<Mds> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //计划部门
            builder.HasOne(m => m.PlanDept, m => m.PlanDeptId);

        }
        #endregion
        #region 主需求计划主数据属性
        public string MdsNumber { get; set; }
        /// <summary>
        /// 需求计划日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 排程日期
        /// 什么时候开始编制的计划
        /// </summary>
        public DateTime ProgramDate { get; set; }
        /// <summary>
        /// 是否生产完成
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
        /// 计划部门
        /// </summary>
        public Nullable<Guid> PlanDeptId { get; set; }
        public Department PlanDept { get; set; }
        /// <summary>
        /// 主需求计划行
        /// </summary>
        public List<MdsItem> Items { get; set; }
        #endregion
    }
}
