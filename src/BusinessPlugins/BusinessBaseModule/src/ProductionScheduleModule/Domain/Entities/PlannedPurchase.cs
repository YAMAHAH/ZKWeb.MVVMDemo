using BusinessPlugins.BaseModule.Domain.Entities;
using BusinessPlugins.BasicModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductionScheduleModule.Domain.Entities
{
    /// <summary>
    /// 采购申请
    /// </summary>
    [ExportMany]
    public class PlannedPurchase : IFullAudit<PlannedPurchase, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 采购申请主数据属性
        /// <summary>
        /// 采购申请号码
        /// </summary>
        public string PurchaseRequestNo { get; set; }
        /// <summary>
        /// 请求日期
        /// </summary>
        public DateTime ReqeustDate { get; set; }
        /// <summary>
        /// 需求日期
        /// </summary>

        public DateTime NeedDate { get; set; }
        /// <summary>
        /// 采购请求类型
        /// </summary>
        public PurchaseType PurchaseRequestType { get; set; }
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
        /// 工厂
        /// </summary>
        public Guid PlantId { get; set; }
        public Plant Plant { get; set; }
        /// <summary>
        /// 请求供应商
        /// </summary>
        public Guid RequestVendorId { get; set; }
        public Partner RequestVendor { get; set; }
  
        /// <summary>
        /// 请求行
        /// </summary>
        public List<PlannedPurchaseItem> Items { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<PlannedPurchase> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //请求供应商
            builder.HasMany(r => r.RequestVendor, r => r.RequestVendorId);
        }
        #endregion
    }
}
