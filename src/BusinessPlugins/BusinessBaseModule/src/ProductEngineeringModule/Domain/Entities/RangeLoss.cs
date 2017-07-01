using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Entities
{
    /// <summary>
    /// 批量损耗
    /// </summary>
    [ExportMany]
    public class RangeLoss : IFullAudit<RangeLoss, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }

        #endregion
        #region 损耗范围基本信息
        /// <summary>
        /// 序号
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 开始范围
        /// </summary>
        public double StartRange { get; set; }
        /// <summary>
        /// 结束范围
        /// </summary>
        public double EndRange { get; set; }
        /// <summary>
        /// 损耗率
        /// </summary>
        public double LossRate { get; set; }
        /// <summary>
        /// 固定附加值
        /// </summary>
        public double FixedAddedValue { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<RangeLoss> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
        }
        #endregion
    }
}
