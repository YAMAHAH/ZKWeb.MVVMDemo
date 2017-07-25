using BusinessPlugins.BasicModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;


namespace BusinessPlugins.SubcontractModule.Domain.Entities
{
    /// <summary>
    /// 外包价目表
    /// </summary>
    [ExportMany]
    public class SubcontractInfoRecord : IFullAudit<SubcontractInfoRecord, Guid>
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
        /// 起始批量
        /// </summary>
        public double StartRange { get; set; }
        /// <summary>
        /// 结束批量
        /// </summary>
        public double EndRange { get; set; }
        /// <summary>
        /// 最新生效日期
        /// 业务日期大于等于生效日期
        /// </summary>
        public Nullable<DateTime> FromDate { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 单位转换率
        /// </summary>
        public double UnitRate { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion

        #region 依赖对象引用

        /// <summary>
        ///  外包商
        /// </summary>
        public Guid SubcontractorId { get; set; }
        public Partner Subcontractor { get; set; }
        /// <summary>
        /// 工序
        /// </summary>
        public Guid ProcessStepId { get; set; }
        public ProcessStep ProcessStep { get; set; }
        /// <summary>
        /// 货币
        /// </summary>
        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; }

        #endregion

        #region 实体配置
        public void Configure(IEntityMappingBuilder<SubcontractInfoRecord> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();

            nativeBuilder.HasKey(p => new { p.Id, p.SubcontractorId, p.ProcessStepId, p.Unit, p.FromDate, p.StartRange })
               .HasName("SubcontractPriceId");

            builder.Map(p => p.Unit, new EntityMappingOptions() { Length = 10 });

            builder.HasMany(p => p.OwnerTenant, p => p.OwnerTenantId);

            builder.HasMany(p => p.Subcontractor, p => p.SubcontractorId);

            builder.HasMany(p => p.Currency, p => p.CurrencyId);

            builder.HasMany(p => p.ProcessStep, p => p.ProcessStepId);
        }
        #endregion
    }
}
