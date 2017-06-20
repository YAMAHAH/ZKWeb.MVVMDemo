using System;
using System.Collections.Generic;
using System.ComponentModel;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.OrganizationModule.src.Application.Dtos
{
    [Description("用户传出信息")]
    public class UserOutputDto : IOutputDto
    {
        [Description("用户Id")]
        [ComponentProperty(Text = "用户Id", required = true)]
        public Guid Id { get; set; }
        [Description("用户类型")]
        [ComponentProperty(Text = "用户类型", required = true)]
        public string Type { get; set; }
        [Description("用户名")]
        [ComponentProperty(Text = "用户名", required = true)]
        public string Username { get; set; }
        [Description("租户Id")]
        [ComponentProperty(Text = "租户Id", required = true)]
        public Guid OwnerTenantId { get; set; }
        [Description("创建时间")]
        [ComponentProperty(Text = "创建时间", required = true)]
        public string CreateTime { get; set; }
        [Description("更新时间")]
        [ComponentProperty(Text = "更新时间", required = true)]
        public string UpdateTime { get; set; }
        [Description("备注")]
        [ComponentProperty(Text = "备注", required = true)]
        public string Remark { get; set; }
        [Description("已删除")]
        [ComponentProperty(Text = "已删除", required = true)]
        public bool Deleted { get; set; }
        [Description("角色Id列表")]
        [ComponentProperty(Text = "角色Id列表", required = true)]
        public IList<Guid> RoleIds { get; set; }
        [Description("角色列表")]
        [ComponentProperty(Text = "角色列表", required = true)]
        public IList<RoleOutputDto> Roles { get; set; }

        [Description("租户名")]
        [ComponentProperty(Text = "租户名称", required = true)]
        public string OwnerTenantName { get; set; }
        [Description("租户是主租户")]
        [ComponentProperty(Text = "主租户", required = true)]
        public bool OwnerTenantIsMasterTenant { get; set; }
        [Description("头像图片的Base64")]
        [ComponentProperty(Text = "头像图片", required = true)]
        public string AvatarImageBase64 { get; set; }

        [Description("实现的用户类型列表")]
        [ComponentProperty(Text = "用户类型列表", required = true)]
        public IList<string> ImplementedTypes { get; set; }
        [Description("权限列表")]
        [ComponentProperty(Text = "权限列表", required = true)]
        public IList<string> Privileges { get; set; }

        public UserOutputDto()
        {
            Roles = new List<RoleOutputDto>();
            ImplementedTypes = new List<string>();
            Privileges = new List<string>();
        }
    }
}
