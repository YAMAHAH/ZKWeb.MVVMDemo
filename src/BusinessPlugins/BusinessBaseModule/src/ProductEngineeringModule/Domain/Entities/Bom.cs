using BusinessPlugins.OrganizationModule.Domain;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Entities
{
    /// <summary>
    /// BOM(Bill Of Material，物料清单)
    /// 成本BOM,销售BOM,生产BOM/包装BOM,工程BOM的基类
    /// </summary>
    [ExportMany]
    public class Bom : IFullAudit<Bom, Guid>
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
        /// 一个父阶所要的数量
        /// </summary>
        public double SingleTotal { get; set; }
        /// <summary>
        /// 单位数量
        /// 一个根产品所要的数量
        /// </summary>
        public double Total { get; set; } = 1;
        /// <summary>
        /// 成本
        /// </summary>
        //public double Cost { get; set; }
        //public decimal NeedTotal { get; set; }
        //public decimal MaoTotal { get; set; }
        //public Decimal MrpTotal { get; set; }
        //public decimal UsedKc { get; set; }
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
        public Bom Parent { get; set; }
        /// <summary>
        /// 根结点版次
        /// </summary>
        public Guid RootVersionId { get; set; }
        public ProductVersion RootVersion { get; set; }
        /// <summary>
        /// 结点版次
        /// </summary>
        public Guid NodeVersionId { get; set; }
        public ProductVersion NodeVersion { get; set; }

        /// <summary>
        /// 子结点集合
        /// </summary>
        public List<Bom> Childs { get; set; } = new List<Bom>();
        #endregion


        public void Configure(IEntityMappingBuilder<Bom> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);

            //Tenant
            builder.HasMany(m => m.OwnerTenant, m => m.OwnerTenantId);

            //parent
            builder.HasMany(b => b.Parent, b => b.Childs, b => b.ParentId);

            nativeBuilder.HasOne(b => b.NodeVersion)
                .WithMany(pv => pv.NodeRefs)
                .HasForeignKey(b => b.NodeVersionId)
                .OnDelete(DeleteBehavior.Restrict);

            nativeBuilder.HasOne(b => b.RootVersion)
                .WithMany(pv => pv.RootRefs)
                .HasForeignKey(b => b.RootVersionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
