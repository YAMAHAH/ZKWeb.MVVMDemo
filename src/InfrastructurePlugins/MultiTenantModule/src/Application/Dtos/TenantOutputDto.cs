using System;
using System.ComponentModel;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.MultiTenantModule.Domain.Entities;

namespace InfrastructurePlugins.MultiTenantModule.Application.Dtos
{
    [Description("租户传出信息")]
    [ModelTypeMapper(typeof(Tenant))]
    public class TenantOutputDto : IOutputDto
    {
        [Description("租户Id")]
        public Guid Id { get; set; }

        [Description("租户名称"), ComponentProperty(Text = "租户名称", required = true)]
        public string Name { get; set; }

        [Description("是否主租户"), ComponentProperty(Text = "是否主租户", required = true, Name = "IsMaster")]
        public bool IsMaster { get; set; }

        [Description("超级管理员名称"), ComponentProperty(Text = "超级管理员名称", required = true, Name = "SuperAdminName")]
        public string SuperAdminName { get; set; }

        [Description("创建时间"), ComponentProperty(Text = "创建时间", required = true, Name = "CreateTime")]
        public string CreateTime { get; set; }

        [Description("更新时间"), ComponentProperty(Text = "更新时间", required = true, Name = "UpdateTime")]
        public string UpdateTime { get; set; }

        [Description("备注"), ComponentProperty(Text = "备注", Name = "Remark")]
        public string Remark { get; set; }
    }
}
