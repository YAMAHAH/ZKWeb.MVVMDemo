﻿using System.Collections.Generic;
using InfrastructurePlugins.BaseModule.Module;

namespace ZKWeb.Plugins.OrganizationModule.Components.PrivilegeProviders.Interfaces
{
    /// <summary>
    /// 权限提供器
    /// 推荐使用的权限字符串的格式
    /// - Group:Name
    /// 例如
    /// - AdminManage:View
    /// - AdminManage:Edit
    /// - AdminManage:Delete
    /// - AdminManage:DeleteForever
    /// 不使用此格式时权限会被归到"其他"分组下
    /// </summary>
    public interface IPrivilegesProvider
    {
        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetPrivileges();
        /// <summary>
        /// 获取模板类对象
        /// </summary>
        List<ComponentClassInfo> GetModuleComponentClassInfos();
    }
}