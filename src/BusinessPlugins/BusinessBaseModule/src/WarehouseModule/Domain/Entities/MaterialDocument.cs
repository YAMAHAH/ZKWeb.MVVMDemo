using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 物料凭证
    /// 收货=>物料凭证=>转储请求=>转储订单
    /// </summary>
    public class MaterialDocument : IFullAudit<MaterialDocument, Guid>
    {
        #region FullAudit接口实现

        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 主生产计划行主数据属性
        /// <summary>
        /// 过账日期
        /// </summary>
        public DateTime PostDate { get; set; }
        //交货单
        public string DeliveryNumber { get; set; }
        //提货单
        public string PickupNumber { get; set; }
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
        /// 合作伙伴
        /// </summary>
        public Nullable<Guid> PtnId { get; set; }
        public Partner Partner { get; set; }

        public List<MaterialDocumentItem> MatDocItems { get; set; }
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<MaterialDocument> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Partner, m => m.PtnId);
        }
        #endregion
    }

  
}
