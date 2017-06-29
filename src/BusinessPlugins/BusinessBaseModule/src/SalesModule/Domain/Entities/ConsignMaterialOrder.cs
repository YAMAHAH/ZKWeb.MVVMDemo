using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 客供料订单
    /// </summary>
    [ExportMany]
    public class ConsignMaterialOrder : IFullAudit<ConsignMaterialOrder, Guid>
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
        public string ConsignMaterialCode { get; set; }
        /// <summary>
        /// 登记日期
        /// </summary>
        public DateTime RecordDate { get; set; }
        /// <summary>
        /// 需求日期
        /// </summary>

        public DateTime NeedDate { get; set; }
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
        /// 供货商
        /// </summary>
        public Guid VendorId { get; set; }
        public Partner Vendor { get; set; }
        /// <summary>
        /// 销售部门
        /// </summary>
        public Nullable<Guid> DptmId { get; set; }
        public Department SalesDepartment { get; set; }
        public List<ConsignMaterialItem> Items { get; set; }
        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<ConsignMaterialOrder> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            // builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
            builder.HasMany(p => p.OwnerTenant, p => p.OwnerTenantId);
            //工厂
            builder.HasMany(i => i.Plant, i => i.PlantId);
            //客户物料提供商
            builder.HasMany(i => i.Vendor, i => i.VendorId);
            //部门
            builder.HasMany(o => o.SalesDepartment, o => o.DptmId);

        }
        #endregion
    }

}
