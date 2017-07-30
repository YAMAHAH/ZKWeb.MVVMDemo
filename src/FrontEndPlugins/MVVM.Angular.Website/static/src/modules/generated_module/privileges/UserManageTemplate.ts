
export class UserManageTemplate {

    /** 属性 */
    TemplateId = "ffa69635-e65d-f953-1941-94cbfd151092";
    TempName = "UserManage";
    ModuleId = "c2791da9-0434-c9c1-4fa1-67c25844a57a";
    ModuleName = "UserManager";
    /** 数据字段 */
    dataFields = {
        User_Id: {
            name: "Id", queryable: true, required: true, visible: true, editable: true, text: "用户Id",
            default: "", dataType: "Guid", componentType: ""
        },
        User_Type: {
            name: "Type", queryable: true, required: true, visible: true, editable: true, text: "用户类型",
            default: "", dataType: "String", componentType: ""
        },
        User_Username: {
            name: "Username", queryable: true, required: true, visible: true, editable: true, text: "用户名",
            default: "", dataType: "String", componentType: ""
        },
        User_OwnerTenantId: {
            name: "OwnerTenantId", queryable: true, required: true, visible: true, editable: true, text: "租户Id",
            default: "", dataType: "Guid", componentType: ""
        },
        User_CreateTime: {
            name: "CreateTime", queryable: true, required: true, visible: true, editable: true, text: "创建时间",
            default: "", dataType: "String", componentType: ""
        },
        User_UpdateTime: {
            name: "UpdateTime", queryable: true, required: true, visible: true, editable: true, text: "更新时间",
            default: "", dataType: "String", componentType: ""
        },
        User_Remark: {
            name: "Remark", queryable: true, required: true, visible: true, editable: true, text: "备注",
            default: "", dataType: "String", componentType: ""
        },
        User_Deleted: {
            name: "Deleted", queryable: true, required: true, visible: true, editable: true, text: "已删除",
            default: "", dataType: "Boolean", componentType: ""
        },
        User_RoleIds: {
            name: "RoleIds", queryable: true, required: true, visible: true, editable: true, text: "角色Id列表",
            default: "", dataType: "Guid", componentType: ""
        },
        Role_Name: {
            name: "Name", queryable: true, required: true, visible: true, editable: true, text: "角色名称",
            default: "", dataType: "String", componentType: ""
        },
        Role_Privileges: {
            name: "Privileges", queryable: true, required: true, visible: true, editable: true, text: "权限列表",
            default: "", dataType: "String", componentType: ""
        },
        Role_PrivilegeNames: {
            name: "PrivilegeNames", queryable: true, required: true, visible: true, editable: true, text: "权限名称列表",
            default: "", dataType: "String", componentType: ""
        },
        Role_OwnerTenantName: {
            name: "OwnerTenantName", queryable: true, required: true, visible: true, editable: true, text: "租户名",
            default: "", dataType: "String", componentType: ""
        },
        Role_OwnerTenantId: {
            name: "OwnerTenantId", queryable: true, required: true, visible: true, editable: true, text: "租户Id",
            default: "", dataType: "Guid", componentType: ""
        },
        Role_CreateTime: {
            name: "CreateTime", queryable: true, required: true, visible: true, editable: true, text: "创建时间",
            default: "", dataType: "String", componentType: ""
        },
        Role_UpdateTime: {
            name: "UpdateTime", queryable: true, required: true, visible: true, editable: true, text: "更新时间",
            default: "", dataType: "String", componentType: ""
        },
        Role_Remark: {
            name: "Remark", queryable: true, required: true, visible: true, editable: true, text: "备注",
            default: "", dataType: "String", componentType: ""
        },
        Role_Deleted: {
            name: "Deleted", queryable: true, required: true, visible: true, editable: true, text: "已删除",
            default: "", dataType: "Boolean", componentType: ""
        },
        User_OwnerTenantName: {
            name: "OwnerTenantName", queryable: true, required: true, visible: true, editable: true, text: "租户名称",
            default: "", dataType: "String", componentType: ""
        },
        User_OwnerTenantIsMasterTenant: {
            name: "OwnerTenantIsMasterTenant", queryable: true, required: true, visible: true, editable: true, text: "主租户",
            default: "", dataType: "Boolean", componentType: ""
        },
        User_AvatarImageBase64: {
            name: "AvatarImageBase64", queryable: true, required: true, visible: true, editable: true, text: "头像图片",
            default: "", dataType: "String", componentType: ""
        },
        User_ImplementedTypes: {
            name: "ImplementedTypes", queryable: true, required: true, visible: true, editable: true, text: "用户类型列表",
            default: "", dataType: "String", componentType: ""
        },
        User_Privileges: {
            name: "Privileges", queryable: true, required: true, visible: true, editable: true, text: "权限列表",
            default: "", dataType: "String", componentType: ""
        }
    };

    /** 功能 */
    actions = {
        Search: {
            enable: true, text: "搜索", default: true 
        },
        Test: {
            enable: true, text: "测试", default: true 
        },
        TestGet: {
            enable: true, text: "测试空参数", default: true 
        },
        TestObject: {
            enable: true, text: "测试复杂对象", default: true 
        },
        Edit: {
            enable: true, text: "编辑", default: true 
        },
        Remove: {
            enable: true, text: "删除", default: true 
        },
        GetAllUserTypes: {
            enable: true, text: "获取用户类型", default: true 
        }
    };

    /** 过滤器 */
    filters = {

    };
}