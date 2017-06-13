import { Component, OnInit } from '@angular/core';
import { AESUtils } from '@core/utils/aes-utils';

@Component({
    moduleId: module.id,
    selector: 'sale-order',
    templateUrl: 'sale-order.component.html',
    styleUrls: ['sale-order.component.scss']
})
export class SaleOrderComponent implements OnInit {
    saleOrder = "Sale-Order-Module";
    chiperText;
    plainText;
    /**
     *
     */
    constructor() {

    }

    ngOnInit() {
        this.chiperText = AESUtils.EncryptToBase64String("99b3ad6e", this.saleOrder);
        this.plainText = AESUtils.decryptToUtf8String("99b3ad6e", this.chiperText);
    }
}
