
export class SaleOrderTemplate {

     template=`
        export class {{templateName}} {
            dataFields = {
                {{dataFields}}
            };

            actions = {
                {{actions}}
            };

            filters = {
                {{filters}}
            }
        }
        `;

        
    /**数据字段 */
    dataFieldTemp = ` 
        {{dataFieldName}}: {
         queryable: {{queryableValue}}, required: {{requiredValue}}, visible: {{visibleValue}}, editable: {{editableValue}}, text: {{textValue}},
         default: {{defaultValue}}, dataType: {{dataTypeValue}}, componentType: {{compTypeValue}}
     }`;
    dataFields = {
        /**订单编码 */
        orderNo: {
            queryable: true, required: true, visible: true, editable: true, text: "订单编码",
            default: "SO-170531-0001", dataType: "string", componentType: "z-input"
        },
        /**订单日期 */
        orderDate: {
            queryable: true, required: true, visible: true, editable: true, text: "订单日期",
            default: "17-05-31", dataType: "date", componentType: "z-datetime"
        }
    };

    action = `
        {{actionName}}:{ enable:{{enableValue}}, text:"{{TextValue}}, default:{{defaultValue}} }
    `;
    /** 功能 */
    actions = {
        /**创建单据 */
        createBill: { enable: true, text: "创建单据", default: true },
        /**编辑单据 */
        editBill: { enable: true, text: "编辑单据", default: true }
    }

    filters = {
    };
    test() {
        this.dataFields.orderDate.componentType;
        this.actions.createBill.enable;
    }
}