using BusinessPlugins.BaseModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using System;
using ZKWeb.Database;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    public class MaterialDocumentItem : IFullAudit<MaterialDocumentItem, Guid>
    {

        //凭证日期

        //过账日期
        //交货单
        //提货单
        //备注
        #region FullAudit接口实现

        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Guid OwnerTenantId { get; set; }
        public Tenant OwnerTenant { get; set; }
        #endregion

        #region 主生产计划行主数据属性
        // 料号 + 特征码  数量,单位,库存地点,批次,移动类型,库存类型,工厂

        //数量
        public decimal Quantity { get; set; }
        //单位
        public string Unit { get; set; }
        /// <summary>
        /// 移动类型
        /// </summary>
        public MovementType MovementType { get; set; }
        /// <summary>
        /// 库存类型
        /// </summary>
        public InventoryType InventoryType { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNumber { get; set; }
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
        /// 产品版次
        /// </summary>
        public Guid ProdVerId { get; set; }
        public ProductVersion ProdVer { get; set; }
        /// <summary>
        /// 产品特性值
        /// </summary>
        public Nullable<Guid> ProdFeatValGrpId { get; set; }

        public ProductFeatureValueGroup ProdFeatValGrp { get; set; }
        /// <summary>
        /// 物料凭证
        /// </summary>
        public Guid MatDocId { get; set; }
        public MaterialDocument MatDoc { get; set; }

        #endregion
        #region 实体关系配置
        public void Configure(IEntityMappingBuilder<MaterialDocumentItem> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);
            //工厂
            builder.HasMany(m => m.Plant, m => m.PlantId);
            //ProductVersion
            builder.HasMany(i => i.ProdVer, i => i.ProdVerId);
            //产品特性值
            builder.HasOne(i => i.ProdFeatValGrp, i => i.ProdFeatValGrpId);
            //物料凭证
            builder.HasMany(m => m.MatDoc, m => m.MatDocItems, m => m.MatDocId);
        }
        #endregion
    }
}
