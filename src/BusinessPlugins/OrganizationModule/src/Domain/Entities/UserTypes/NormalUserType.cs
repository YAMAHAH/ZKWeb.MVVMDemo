﻿using ZKWeb.MVVMPlugins.OrganizationModule.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.OrganizationModule.src.Domain.Entities.UserTypes
{
    /// <summary>
    /// 用户类型: 普通用户
    /// </summary>
    [ExportMany]
    public class NormalUserType : IAmUser
    {
        public const string ConstType = "User";
        public string Type { get { return ConstType; } }
    }
}
