﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using InfrastructurePlugins.BaseModule.Application.Dtos;

namespace BusinessPlugins.OrganizationModule.Application.Dtos
{
    [Description("用户传入信息")]
    public class UserInputDto : IInputDto
    {
        [Description("用户Id")]
        public Guid Id { get; set; }
        [Description("用户类型"), Required]
        public string Type { get; set; }
        [Description("用户名"), Required]
        public string Username { get; set; }
        [Description("密码")]
        public string Password { get; set; }
        [Description("确认密码")]
        public string ConfirmPassword { get; set; }
        [Description("备注")]
        public string Remark { get; set; }
        [Description("角色Id列表")]
        public IList<Guid> RoleIds { get; set; }

        public UserInputDto()
        {
            RoleIds = new List<Guid>();
        }
    }
}
