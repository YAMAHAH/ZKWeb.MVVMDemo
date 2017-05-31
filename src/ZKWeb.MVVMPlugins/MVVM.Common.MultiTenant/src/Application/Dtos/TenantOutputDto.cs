﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.Attributes;

namespace ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Application.Dtos
{
    [Description("租户传出信息")]
    public class TenantOutputDto : IOutputDto
    {
        [Description("租户Id")]
        public Guid Id { get; set; }

        [Description("租户名称"), TempDataField(Text = "租户名称", required = true)]
        public string Name { get; set; }

        [Description("是否主租户"), TempDataField(Text = "是否主租户", required = true)]
        public bool IsMaster { get; set; }

        [Description("超级管理员名称"), TempDataField(Text = "超级管理员名称", required = true)]
        public string SuperAdminName { get; set; }

        [Description("创建时间"), TempDataField(Text = "创建时间", required = true)]
        public string CreateTime { get; set; }

        [Description("更新时间"), TempDataField(Text = "更新时间", required = true)]
        public string UpdateTime { get; set; }

        [Description("备注"), TempDataField(Text = "备注")]
        public string Remark { get; set; }
    }
}
