// Angular Imports
import { NgModule } from '@angular/core';

// This Module's Components
import { SaleOrderQueryComponent } from './sale-order-query.component';
import { RouterModule, Routes } from '@angular/router';
import { DataTableModule } from 'primeng/components/datatable/datatable';


const saleQueryRouterConfig: Routes = [
    { path: "", component: SaleOrderQueryComponent }
];

@NgModule({
    imports: [
        DataTableModule,
        RouterModule.forChild(saleQueryRouterConfig)
    ],
    declarations: [
        SaleOrderQueryComponent,
    ],
    exports: [
        SaleOrderQueryComponent,
    ]
})
export class SaleOrderQueryModule {

}
