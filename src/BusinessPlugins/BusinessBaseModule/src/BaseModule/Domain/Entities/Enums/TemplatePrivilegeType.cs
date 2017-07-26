using System;

namespace BusinessPlugins.BaseModule.Domain.Entities
{
    /// <summary>
    /// 模板权限类型
    /// </summary>
    [Flags]
    public enum TemplatePrivilegeType
    {
        Visible = 1,
        Enable = 2,
        Editable = 4,
        Queryable = 8
    }
}
