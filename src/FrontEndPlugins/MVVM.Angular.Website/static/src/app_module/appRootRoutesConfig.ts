import { Routes } from '@angular/router';
import { RouterOutletComponent } from "@global_module/components/router-outlet.component";
import { PageNotFoundComponent } from "@global_module/components/page_not_found.component";

export const appRootRoutesConfig: Routes = [
    { path: '', redirectTo: '/admin', pathMatch: 'full' },
    { path: 'admin', loadChildren: '../modules/admin_modules/admin_module/admin.module#AdminModule' },
    {
        path: 'business',
        component: RouterOutletComponent,
        loadChildren: '../modules/business_modules/main_module/main.module#MainModule'
    },
    { path: '**', component: PageNotFoundComponent }
];