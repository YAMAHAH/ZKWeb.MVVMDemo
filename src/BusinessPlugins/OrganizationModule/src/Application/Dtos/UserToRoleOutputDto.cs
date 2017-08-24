using System;
using System.Collections.Generic;
using System.ComponentModel;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using BusinessPlugins.OrganizationModule.Domain.Entities;

namespace BusinessPlugins.OrganizationModule.Application.Dtos
{
    [Description("角色传出信息")]
    [ModelTypeMapper(typeof(UserToRole))]
    public class UserToRoleOutputDto : IOutputDto
    {
        [Description("Id")]
        public Guid Id { get; set; }
        [Description("用户")]
        public UserOutputDto User { get; set; }
        [Description("角色")]
        public RoleOutputDto Role { get; set; }
    }
}
