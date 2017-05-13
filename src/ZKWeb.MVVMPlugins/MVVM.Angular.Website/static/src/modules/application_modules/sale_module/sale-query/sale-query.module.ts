// Angular Imports
import { NgModule } from '@angular/core';

// This Module's Components
import { SaleQueryComponent } from './sale-query.component';
import { RouterModule, Routes } from '@angular/router';
import { DataTableModule } from 'primeng/components/datatable/datatable';


const saleQueryRouterConfig: Routes = [
    { path: "", component: SaleQueryComponent }
];

@NgModule({
    imports: [
        DataTableModule,
        RouterModule.forChild(saleQueryRouterConfig)
    ],
    declarations: [
        SaleQueryComponent,
    ],
    exports: [
        SaleQueryComponent,
    ]
})
export class SaleQueryModule {

}
