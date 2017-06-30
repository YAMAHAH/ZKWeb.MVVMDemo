using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BaseModule.Domain.Entities
{
    /// <summary>
    /// 货币
    /// </summary>
    [ExportMany]
    public class Currency : IFullAudit<Currency, Guid>
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
        /// 货币代码
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// 货币名称
        /// </summary>
        public string CurrencyName { get; set; }
        /// <summary>
        /// 汇率
        /// 主结算货币:人民币
        /// 1美金 = ? 人民币
        /// </summary>
        public double ExchangeRate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion

        #region 实体配置
        public void Configure(IEntityMappingBuilder<Currency> builder)
        {
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(p => p.OwnerTenant, p => p.OwnerTenantId);

        }
        #endregion
    }
}
