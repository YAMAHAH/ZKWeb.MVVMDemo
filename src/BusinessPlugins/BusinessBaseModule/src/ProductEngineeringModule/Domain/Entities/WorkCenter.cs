using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Entities
{
    /// <summary>
    /// 工作中心
    /// </summary>
    [ExportMany]
    public class WorkCenter : IFullAudit<WorkCenter, Guid>
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
        public void Configure(IEntityMappingBuilder<WorkCenter> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });

            //工厂
            nativeBuilder.HasOne(i => i.Plant)
                .WithMany()
                .HasForeignKey(i => i.PlantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        #endregion
        #region 工作中心基本信息
        public string WorkCenterNo { get; set; }
        public string WorkCenterName { get; set; }
        /// <summary>
        /// 工序类别:机器/机器组/工时/工时组/生产线/供应商
        /// </summary>
        public string WorkCenterType { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        public string Usage { get; set; }
        /// <summary>
        /// 固定损耗值
        /// 物料损耗率 = (投入-产出)/投入
        /// 损耗率 = 耗损量/总消耗量*100%
        /// 单位损耗值: 1 / (100-20) * 100
        /// </summary>
        public double FixedLossRate { get; set; }

        public bool IsDisabled { get; set; }
        public string Remark { get; set; }
        #endregion

        #region 依赖对象引用
        /// <summary>
        /// 工厂ID
        /// </summary>
        public Guid PlantId { get; set; }
        public Plant Plant { get; set; }
        /// <summary>
        /// 供应仓库
        /// </summary>
        public Nullable<Guid> WarehouseId { get; set; }
        /// <summary>
        /// 工位
        /// </summary>
        public Nullable<Guid> LocationId { get; set; }
        /// <summary>
        /// 员工
        /// </summary>
        public Nullable<Guid> EmployeeId { get; set; }
        /// <summary>
        /// 生产车间
        /// </summary>
        public Nullable<Guid> DepartmentId { get; set; }
        /// <summary>
        /// 外协厂商
        /// </summary>
        public Nullable<Guid> PartnerId { get; set; }
        #endregion
    }
}
