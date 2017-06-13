export interface TemplateDataField {
    queryable: boolean;
    required: boolean;
    visible: boolean;
    editable: boolean;
    text: string;
    default: string;
    dataType: string;
    componentType:string;
}


export class SaleOrderTemplate{
    actions:any[];
    dataFields: TemplateDataField[] = [];
}