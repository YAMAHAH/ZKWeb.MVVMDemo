import { Routes } from '@angular/router';
import { MainComponent } from './main.component';
import { LazyLoadContainer } from './lazy-load-container';

export const applicationMainRoutesConfig: Routes = [
    {
        path: "", redirectTo: "main", pathMatch: "full"
    },
    {
        path: "main", component: MainComponent
    },
    {
        path: "sale-order",
        component: LazyLoadContainer,
        outlet: "sale-o",
        loadChildren: "../sale_module/sale-order/sale-order.module#SaleOrderModule"
    },
    {
        path: "sale-order-query",
        component: LazyLoadContainer,
        outlet: "sale-o-q",
        loadChildren: "../sale_module/sale-order-query/sale-order-query.module#SaleOrderQueryModule"
    }

];