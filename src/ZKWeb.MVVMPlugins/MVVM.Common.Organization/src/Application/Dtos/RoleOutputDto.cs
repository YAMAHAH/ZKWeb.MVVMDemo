using System;
using System.Collections.Generic;
using System.ComponentModel;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos
{
    [Description("角色传出信息")]
    public class RoleOutputDto : IOutputDto
    {
        [Description("角色Id")]
        public Guid Id { get; set; }
        [Description("角色名称")]
        [TempDataField(Text = "角色名称", required = true)]
        public string Name { get; set; }
        [Description("权限列表")]
        [TempDataField(Text = "权限列表", required = true)]
        public IList<string> Privileges { get; set; }
        [TempDataField(Text = "权限名称列表", required = true)]
        [Description("权限名称列表")]
        public string PrivilegeNames { get; set; }

        [Description("租户名")]
        [TempDataField(Text = "租户名", required = true)]
        public string OwnerTenantName { get; set; }

        [Description("租户Id")]
        [TempDataField(Text = "租户Id", required = true)]
        public Guid OwnerTenantId { get; set; }

        [Description("创建时间")]
        [TempDataField(Text = "创建时间", required = true)]
        public string CreateTime { get; set; }
        [Description("更新时间")]
        [TempDataField(Text = "更新时间", required = true)]
        public string UpdateTime { get; set; }

        [Description("备注")]
        [TempDataField(Text = "备注", required = true)]
        public string Remark { get; set; }

        [Description("已删除")]
        [TempDataField(Text = "已删除", required = true)]
        public bool Deleted { get; set; }

        public RoleOutputDto()
        {
            Privileges = new List<string>();
        }
    }
}
