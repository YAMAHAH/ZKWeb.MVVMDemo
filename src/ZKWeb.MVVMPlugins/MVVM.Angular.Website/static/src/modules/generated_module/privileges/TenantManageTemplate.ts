
export class TenantManageTemplate {

    /**数据字段 */
    dataFields = {
        Name: {
            queryable: true, required: true, visible: true, editable: true, text: "租户名称",
            default: "", dataType: "", componentType: ""
        },
        IsMaster: {
            queryable: true, required: true, visible: true, editable: true, text: "是否主租户",
            default: "", dataType: "", componentType: ""
        },
        SuperAdminName: {
            queryable: true, required: true, visible: true, editable: true, text: "超级管理员名称",
            default: "", dataType: "", componentType: ""
        },
        CreateTime: {
            queryable: true, required: true, visible: true, editable: true, text: "创建时间",
            default: "", dataType: "", componentType: ""
        },
        UpdateTime: {
            queryable: true, required: true, visible: true, editable: true, text: "更新时间",
            default: "", dataType: "", componentType: ""
        },
        Remark: {
            queryable: true, required: false, visible: true, editable: true, text: "备注",
            default: "", dataType: "", componentType: ""
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