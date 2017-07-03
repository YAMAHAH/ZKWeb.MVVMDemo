using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BaseModule.Domain.Entities
{
    /// <summary>
    /// 计划边际码
    /// </summary>
    [ExportMany]
    public class ScheduleMarginKey : IFullAudit<Currency, Guid>
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
        /// 计划边际代码
        /// </summary>
        public string MarginKeyCode { get; set; }
        /// <summary>
        /// 计划边际名称
        /// </summary>
        public string MarginKeyName { get; set; }
        /// <summary>
        /// 末清期间,以天数计
        /// </summary>

        public double LateDuringTime { get; set; }
        /// <summary>
        /// 产后缓存时间,以天数计
        /// </summary>
        public double PostnatalCacheTime { get; set; }
        /// <summary>
        /// 产前缓存时间,以天数计
        /// </summary>
        public double PrenatalCacheTime { get; set; }
        /// <summary>
        /// 下达期间,以天数计
        /// </summary>
        public int IssuedDuringTime { get; set; }
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
