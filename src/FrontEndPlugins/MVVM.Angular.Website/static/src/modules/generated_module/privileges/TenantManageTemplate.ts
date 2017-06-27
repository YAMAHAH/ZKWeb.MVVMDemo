
export class TenantManageTemplate {

    /** 属性 */
    TemplateId = "c256442e-4c39-a46b-62b3-6e1103d32f57";
    TempName = "TenantManage";
    ModuleId = "dfd5eccb-f04e-ce9e-9e28-37235f9e0de1";
    ModuleName = "MultiTenant";
    /** 数据字段 */
    dataFields = {
        Name: {
            name: "Name", queryable: true, required: true, visible: true, editable: true, text: "租户名称",
            default: "", dataType: "String", componentType: ""
        },
        IsMaster: {
            name: "IsMaster", queryable: true, required: true, visible: true, editable: true, text: "是否主租户",
            default: "", dataType: "Boolean", componentType: ""
        },
        SuperAdminName: {
            name: "SuperAdminName", queryable: true, required: true, visible: true, editable: true, text: "超级管理员名称",
            default: "", dataType: "String", componentType: ""
        },
        CreateTime: {
            name: "CreateTime", queryable: true, required: true, visible: true, editable: true, text: "创建时间",
            default: "", dataType: "String", componentType: ""
        },
        UpdateTime: {
            name: "UpdateTime", queryable: true, required: true, visible: true, editable: true, text: "更新时间",
            default: "", dataType: "String", componentType: ""
        },
        Remark: {
            name: "Remark", queryable: true, required: false, visible: true, editable: true, text: "备注",
            default: "", dataType: "String", componentType: ""
        }
    };

    /** 功能 */
    actions = {
        View: {
            enable: true, text: "搜索", default: true 
        },
        Edit: {
            enable: true, text: "编辑", default: false 
        },
        Remove: {
            enable: true, text: "删除", default: false 
        }
    };

    /** 过滤器 */
    filters = {

    };
}