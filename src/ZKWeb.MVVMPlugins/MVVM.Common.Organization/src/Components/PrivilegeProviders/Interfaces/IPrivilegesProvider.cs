using System.Collections.Generic;

namespace ZKWeb.Plugins.MVVM.Common.Organization.src.Components.PrivilegeProviders.Interfaces
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
        List<TemplateObjectInfo> GetTemplateObjects();

        /// <summary>
        /// 获取模板的功能
        /// </summary>
        /// <returns></returns>
        List<TempActionInfo> GetTemplateActions();
        /// <summary>
        /// 获取模板的数据字段
        /// </summary>
        /// <returns></returns>
        List<TempModelInfo> GetTemplateDataFileds();
        /// <summary>
        /// 获取模板的过滤器
        /// </summary>
        /// <returns></returns>
        List<TempFilterInfo> GetTemplateFilters();
    }
}
