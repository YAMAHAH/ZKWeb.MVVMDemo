import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router';
import { BaseModule } from '../base_module/base.module';

import { AppComponent } from './components/app.component';
import { PageNotFoundComponent } from './components/page_not_found.component';
import { GeneratedModule } from '../generated_module/generated.module';
import { GlobalModule } from '../global_module/global.module';
import { DataTableModule } from "primeng/components/datatable/datatable";

const routes: Routes = [
    { path: '', redirectTo: '/admin', pathMatch: 'full' },
    { path: 'admin', loadChildren: '../admin_module/admin.module#AdminModule' },
    // {
    //     path: 'admin2',
    //     loadChildren: () => new Promise(function (resolve) {
    //         (require as any).ensure([], function (require: any) {
    //             resolve(require('../admin_module/admin.module')['AdminModule']);
    //         });
    //     })
    // },
    { path: '**', component: PageNotFoundComponent }
];

@NgModule({
    imports: [
        BrowserModule, DataTableModule,
        BrowserAnimationsModule,
        GeneratedModule,
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
