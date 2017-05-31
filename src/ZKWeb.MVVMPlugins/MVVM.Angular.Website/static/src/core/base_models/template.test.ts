import { TemplateDataField } from "@core/base_models/template-data-filed";
import { TemplateActionOperation } from "@core/base_models/template-action-operation";

export class SaleOrderPrivileges {
    /**数据字段 */
    /**订单编码 */
    orderNo: TemplateDataField = {
        queryable: true, required: true, visible: true, editable: true, text: "订单编码",
        default: "SO-170531-0001", dataType: "string", componentType: "z-input"
    }
    /**订单日期 */
    orderDate: TemplateDataField = {
        queryable: true, required: true, visible: true, editable: true, text: "订单日期",
        default: "17-05-31", dataType: "date", componentType: "z-datetime"
    }

    /** 功能 */

    /**创建单据 */
    createBill: TemplateActionOperation = { enable: true, text: "创建单据", default: true }
    /**编辑单据 */
    editBill: TemplateActionOperation = { enable: true, text: "编辑单据", default: true }
}