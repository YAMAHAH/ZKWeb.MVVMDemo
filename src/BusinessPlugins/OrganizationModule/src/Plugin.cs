﻿using BusinessPlugins.OrganizationModule.Domain.Services;
using ZKWeb.Plugin;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule
{
    /// <summary>
    /// 插件载入时的处理
    /// </summary>
    [ExportMany]
    public class Plugin : IPlugin
    {
        /// <summary>
        /// 确保超级管理员存在
        /// </summary>
        public Plugin(AdminManager adminManager)
        {
            adminManager.EnsureSuperAdmin();
        }
    }
}
