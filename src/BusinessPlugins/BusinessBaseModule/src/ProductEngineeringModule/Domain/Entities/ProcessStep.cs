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
    /// 工序
    /// </summary>
    [ExportMany]
    public class ProcessStep : IFullAudit<ProcessStep, Guid>
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
        public void Configure(IEntityMappingBuilder<ProcessStep> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });

            //工厂
            nativeBuilder.HasOne(i => i.Plant)
                .WithMany()
                .HasForeignKey(i => i.PlantId)
                .OnDelete(DeleteBehavior.Restrict);

            //工作中心
            nativeBuilder.HasOne(i => i.WorkCenter)
                .WithOne()
                .HasForeignKey<ProcessStep>(i => i.WorkCenterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        #endregion

        #region 工作中心基本信息
        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessNo { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }
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
        /// <summary>
        /// 固定附加值
        /// </summary>
        public double FixedAddedValue { get; set; }

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
        /// 工作中心
        /// </summary>
        public Nullable<Guid> WorkCenterId { get; set; }
        public WorkCenter WorkCenter { get; set; }

        #endregion
    }
}
