using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 客供料行
    /// </summary>
    [ExportMany]
    public class ConsignMaterialItem : IFullAudit<ConsignMaterialItem, Guid>
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
        /// 明细序号
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 单位换算率 
        /// =辅助单位/基本单位 1h=100pc
        /// </summary>
        public double UnitRate { get; set; }
        /// <summary>
        /// 单重
        /// </summary>
        public double SingleWeight { get; set; }

        /// <summary>
        /// 重量 
        /// weight = SingleWeight *  ProdctionQty*UnitRate
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// 生产数量
        /// </summary>
        public double ConsignQty { get; set; }
        /// <summary>
        /// 完成数量
        /// </summary>
        public double FinishQty { get; set; }
        /// <summary>
        /// 剩余数量 计算字段
        /// </summary>
        public double RemainingQty { get; set; }

        /// <summary>
        /// 完成率 计算字段
        /// </summary>
        public double FinishRate { get; set; }

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
        /// 产品版次
        /// </summary>
        public Guid ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; }
        public Guid ConsignMaterialOrderId { get; set; }
        public ConsignMaterialOrder ConsignMaterialOrder { get; set; }
        public Nullable<Guid> SalesOrderItemId { get; set; }
        public SaleOrderItem SaleOrderItem { get; set; }

        #endregion

        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<ConsignMaterialItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //builder.References(p => p.OwnerTenant, new EntityMappingOptions()
            //{
            //    Nullable = false,
            //    CascadeDelete = false
            //});
            builder.HasMany(p => p.OwnerTenant, p => p.OwnerTenantId);
            //客供料订单
            builder.HasMany(i => i.ConsignMaterialOrder, o => o.Items, i => i.ConsignMaterialOrderId);
            //销售订单Item
            builder.HasMany(i => i.SaleOrderItem, i => i.SalesOrderItemId);
            //产品版次
            builder.HasMany(i => i.ProductVersion, i => i.ProductVersionId);
            //剩余数量
            nativeBuilder.Property(i => i.RemainingQty)
              .HasComputedColumnSql("[ConsignQty] - [FinishQty]");
            //重量
            nativeBuilder.Property(i => i.Weight)
                .HasComputedColumnSql("[SingleWeight] * [ConsignQty] * [UnitRate]");
            //完成率
            nativeBuilder.Property(i => i.FinishRate)
                .HasComputedColumnSql("[FinishQty] / [ConsignQty]");

            nativeBuilder.Property(i => i.Unit).HasMaxLength(10);
        }
        #endregion
    }
}
