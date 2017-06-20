﻿using System.ComponentModel;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.OrganizationModule.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.OrganizationModule.src.Application.Dtos
{
    [Description("当前会话信息")]
    public class SessionInfoDto : IOutputDto
    {
        [Description("用户信息")]
        public UserOutputDto User { get; set; }
    }
}
