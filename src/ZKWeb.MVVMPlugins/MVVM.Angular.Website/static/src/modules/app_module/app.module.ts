import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './components/app.component';
import { PageNotFoundComponent } from './components/page_not_found.component';
import { GlobalModule } from '../global_module/global.module';
import { RouterOutletComponent } from '../global_module/components/router-outlet.component';

const routes: Routes = [
    { path: '', redirectTo: '/admin', pathMatch: 'full' },
    { path: 'admin', loadChildren: '@admin_modules/admin_module/admin.module#AdminModule' },
    {
        path: 'application',
        component: RouterOutletComponent,
        loadChildren: '../application_modules/main_module/main.module#MainModule'
    },
    // { path: 'account', loadChildren: '../sale_module/sale.module#SaleModule' },
    // { path: 'passport', loadChildren: '../sale_module/sale.module#SaleModule' },
    { path: '**', component: PageNotFoundComponent }
];

@NgModule({
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        GlobalModule.forRoot(),
        RouterModule.forRoot(routes)
    ],
    declarations: [
        AppComponent,
        PageNotFoundComponent
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
