// Angular Imports
import { NgModule } from '@angular/core';

// This Module's Components
import { SaleOrderComponent } from './sale-order.component';
import { RouterModule, Routes } from '@angular/router';
import { DataTableModule } from 'primeng/components/datatable/datatable';

const saleOrderRouterConfig: Routes = [
    { path: "", component: SaleOrderComponent }
];

@NgModule({
    imports: [
        DataTableModule,
        RouterModule.forChild(saleOrderRouterConfig)
    ],
    declarations: [
        SaleOrderComponent,
    ],
    exports: [
        SaleOrderComponent,
    ]
})
export class SaleOrderModule {

}
