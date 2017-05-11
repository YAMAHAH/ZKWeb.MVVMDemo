﻿import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router';
import { BaseModule } from '../base_module/base.module';

import { AppComponent } from './components/app.component';
import { PageNotFoundComponent } from './components/page_not_found.component';
import { GeneratedModule } from '../generated_module/generated.module';
import { GlobalModule } from '../global_module/global.module';

const routes: Routes = [
    { path: '', redirectTo: '/admin', pathMatch: 'full' },
    { path: 'admin', loadChildren: '../admin_module/admin.module#AdminModule' },
    { path: '**', component: PageNotFoundComponent }
];

@NgModule({
    imports: [
        BrowserModule,
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
