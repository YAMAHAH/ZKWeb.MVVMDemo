using BusinessPlugins.BaseModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain;
using BusinessPlugins.OrganizationModule.Domain.Entities;
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
    ///生产订单BOM 
    /// </summary>
    [ExportMany]
    public class ManufactureBom : IFullAudit<ManufactureBom, Guid>
    {
        //一个产品结构必须一个版次,一个结构有零或多个自身结点

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
        /// 结点序号
        /// </summary>
        public int Ord { get; set; }

        /// <summary>
        /// 用量dosage
        /// 一个单位父阶所需要子项的数量
        /// </summary>
        public double SingleTotal { get; set; }
        /// <summary>
        /// 一个单位子项对应父项的数量
        /// </summary>
        public double Radix { get; set; } = 1;
        /// <summary>
        /// 单位数量
        /// 一个根产品所要的数量
        /// </summary>
        public double Quantity { get; set; } = 1;
        /// <summary>
        /// 固定损耗率
        /// </summary>
        public double FixedLossRate { get; set; }
        /// <summary>
        /// 损耗率
        /// </summary>
        public double LossRate { get; set; }
        /// <summary>
        /// 附加值
        /// </summary>
        public double AddedValues { get; set; }
        /// <summary>
        /// 发料工序号码
        /// </summary>
        public int GoodsIssueProcessNo { get; set; }
        /// <summary>
        /// BOM子项状态:未确认,确认,取消
        /// </summary>
        public BomItemStatus BomItemStatus { get; set; }
        /// <summary>
        /// 插件位置
        /// 指明子项放在父项的什么位置
        /// </summary>
        public string PluginLocation { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public Nullable<Guid> VendorId { get; set; }
        public Partner Vendor { get; set; }
        /// <summary>
        /// 生效日期
        /// </summary>
        public Nullable<DateTime> FromDate { get; set; }
        /// <summary>
        /// 失效日期
        /// </summary>
        public Nullable<DateTime> ExpiryDate { get; set; }

        /// <summary>
        /// 开始批号
        /// </summary>
        public string StartBatchNo { get; set; }
        /// <summary>
        /// 结束批号
        /// </summary>
        public string EndsBatchNo { get; set; }
        /// <summary>
        /// 结点的根结点
        /// </summary>
        public Guid RootId { get; set; }
        /// <summary>
        /// 左结点序号
        /// 预排序遍历树算法
        /// </summary>
        public int Lft { get; set; }
        /// <summary>
        /// 右结点序号
        /// 预排序遍历树算法
        /// </summary>
        public int Rgt { get; set; }
        /// <summary>
        /// 结点所在的层次
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion

        #region 依赖对象引用
        /// <summary>
        /// 父阶结点
        /// </summary>
        public Nullable<Guid> ParentId { get; set; }
        public ManufactureBom Parent { get; set; }
        /// <summary>
        /// 根结点版次
        /// </summary>
        public Guid ProdOrdBomRootVerId { get; set; }
        public ProductVersion ProdOrdBomRootVer { get; set; }
        /// <summary>
        /// 结点版次
        /// </summary>
        public Guid ProdOrdBomNodeVerId { get; set; }
        public ProductVersion ProdOrdBomNodeVersion { get; set; }

        /// <summary>
        /// 子结点集合
        /// </summary>
        public List<ManufactureBom> Childs { get; set; } = new List<ManufactureBom>();
        #endregion


        public void Configure(IEntityMappingBuilder<ManufactureBom> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);

            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);

            //parent
            builder.HasMany(b => b.Parent, b => b.Childs, b => b.ParentId);

            nativeBuilder.HasOne(b => b.ProdOrdBomNodeVersion)
                .WithMany(pv => pv.ProdOrdBomNodeRefs)
                .HasForeignKey(b => b.ProdOrdBomNodeVerId)
                .OnDelete(DeleteBehavior.Restrict);

            nativeBuilder.HasOne(b => b.ProdOrdBomRootVer)
                .WithMany(pv => pv.ProdOrdBomRootRefs)
                .HasForeignKey(b => b.ProdOrdBomRootVerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
