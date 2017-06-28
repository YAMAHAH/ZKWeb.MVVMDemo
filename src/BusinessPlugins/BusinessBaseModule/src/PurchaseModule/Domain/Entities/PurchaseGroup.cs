using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.PurchaseModule.Domain.Entities
{
    /// <summary>
    /// 采购组
    /// 不与任何其它组织机构分配,只应用于主数据和凭证中,如物料主数据,采购信息记录,采购订单等
    /// </summary>
    [ExportMany]
    public class PurchaseGroup : IFullAudit<PurchaseGroup, Guid>
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
        /// 采购组编码
        /// 整个集团中唯一的值
        /// </summary>
        public string PurchaseGroupCode { get; set; }
        public string PurchaseGroupName { get; set; }
        public string Remark { get; set; }
        #endregion

        #region 依赖对象引用
   
        #endregion


        public void Configure(IEntityMappingBuilder<PurchaseGroup> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
           
        }
    }
}
