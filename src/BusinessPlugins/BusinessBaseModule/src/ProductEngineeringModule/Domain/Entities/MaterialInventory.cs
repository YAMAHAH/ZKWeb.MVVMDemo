using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.WarehouseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Entities
{
    /// <summary>
    /// 物料库存表
    /// </summary>
    [ExportMany]
    public class MaterialInventory : IFullAudit<MaterialInventory, Guid>
    {
        #region FullAudit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion
        #region 库存表基本属性
        /// <summary>
        /// 库存数量
        /// </summary>
        public decimal InventoryQuantity { get; set; }
        #endregion

        #region 依赖对象引用
        /// <summary>
        /// 工厂
        /// </summary>
        public Nullable<Guid> PlantId { get; set; }
        public Plant Plant { get; set; }
        /// <summary>
        /// 产品版本
        /// </summary>
        public Guid ProdVerId { get; set; }
        public ProductVersion ProductVer { get; set; }
        /// <summary>
        /// 产品特性值组
        /// </summary>
        public Nullable<Guid> ProdFeatValGrpId { get; set; }
        public ProductFeatureValueGroup ProdFeatValGrp { get; set; }
        ///// <summary>
        ///// 物料凭证
        ///// </summary>
        //public List<MaterialDocumentItem> MaterialDocumentItems { get; set; }
        //实物库存(+)
        //计划采购(+)
        //采购订单(+)
        //计划订单(+)
        //生产订单(+)
        //库存申请(+)
        //库存订单(+)
        //销售预测(+)
        //质检库存(+)
        //冻结库存(+)
        //安全库存(-)
        //销售订单(-)
        //生产物料预留(-)
        //采购物料预留(-)
        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<MaterialInventory> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //租户
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //产品特性值
            builder.HasOne(i => i.ProdFeatValGrp, i => i.ProdFeatValGrpId);
            ////物料凭证
            //nativeBuilder.HasMany(m => m.MaterialDocumentItems)
            //    .WithOne()
            //    .HasForeignKey(m => new { m.ProdVerId, m.ProdFeatValGrpId })
            //    .HasPrincipalKey(m => new { m.ProdVerId, m.ProdFeatValGrpId })
            //    .OnDelete(DeleteBehavior.Restrict);
        }
        #endregion
    }
}
