using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.ORM.EFCore;
using ZKWebStandard.Ioc;

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

        #region 树型结构
        public List<Department> Childs { get; set; }
        public Guid RootId { get; set; }
        public Guid? ParentId { get; set; }
        public Department Parent { get; set; }
        #endregion

        public string DepartmentNo { get; set; }
        public string DepartmentName { get; set; }
        public string Remark { get; set; }

        /// <summary>
        /// 部门所拥有的岗位
        /// </summary>
        public IList<PostGroupDepartment> PostGroupDepartments { get; set; } = new List<PostGroupDepartment>();

        public void Configure(IEntityMappingBuilder<Department> builder)
        {
            var nativeBuilder = builder.GetNativeBuilder();
            builder.Id(p => p.Id);
            builder.References(p => p.OwnerTenant, new EntityMappingOptions() { Nullable = false });
        }
    }
}
