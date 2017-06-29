using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;
using ZKWeb.ORM.EFCore;
using ZKWebStandard.Ioc;
using BusinessPlugins.WarehouseModule.Domain.Entities;

namespace BusinessPlugins.OrganizationModule.Domain.Entities
{
    /// <summary>
    /// 部门
    /// </summary>
    [ExportMany]
    public class Department : IFullAudit<Department, Guid>,
         ITreeStructType<Department, Guid>
    {
        #region IFullAduit接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Deleted { get; set; }
        public Tenant OwnerTenant { get; set; }
        public Guid OwnerTenantId { get; set; }
        #endregion

        #region 依赖对象引用
        /// <summary>
        /// 父结点
        /// </summary>
        public Guid? ParentId { get; set; }
        public Department Parent { get; set; }
        /// <summary>
        /// 子结点集合
        /// </summary>
        public List<Department> Childs { get; set; }
        /// <summary>
        /// 部门所拥有的岗位
        /// </summary>
        public IList<PostGroupDepartment> PostGroupDepartments { get; set; } = new List<PostGroupDepartment>();
        #endregion

        #region 主数据基本属性
        /// <summary>
        /// 部门编码
        /// </summary>
        public string DepartmentCode { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion

        #region 预排序遍历树算法
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
        #endregion

        public void Configure(IEntityMappingBuilder<Department> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
        }
    }
}
