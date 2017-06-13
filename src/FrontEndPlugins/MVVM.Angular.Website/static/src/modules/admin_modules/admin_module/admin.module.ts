import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { AdminAboutMeComponent } from './components/admin-about-me.component';
import { AdminAboutWebsiteComponent } from './components/admin-about-website.component';
import { AdminIndexComponent } from './components/admin-index.component';
import { AdminLoginComponent } from './components/admin-login.component';
import { AdminContainerComponent } from '../admin_base_module/components/admin-container.component';
import { InputTextModule } from 'primeng/components/inputtext/inputtext';
import { PanelModule } from 'primeng/components/panel/panel';
import { ButtonModule } from 'primeng/components/button/button';
import { MessagesModule } from 'primeng/components/messages/messages';
import { BlockUIModule } from 'primeng/components/blockui/blockui';
import { DataTableModule } from 'primeng/components/datatable/datatable';
import { TabViewModule } from "primeng/components/tabview/tabview";
import { AuthGuard } from "@auth_module/auth/auth-guard";
import { UserTypes } from "@generated_module/privileges/user-types";
import { AdminBaseModule } from "@admin_modules/admin_base_module/admin_base.module";
import { GeneratedModule } from "@generated_module/generated.module";
import { AuthModule } from "@auth_module/auth.module";
import { adminRoutesConfig } from './admin.routes.config';
import { BaseModule } from "@business_bases/desktop/base_module/base.module";

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        InputTextModule,
        PanelModule,
        ButtonModule,
        MessagesModule,
        BlockUIModule,
        DataTableModule,
        TabViewModule,
        BaseModule,
        AdminBaseModule,
        GeneratedModule,
        AuthModule,
        RouterModule.forChild(adminRoutesConfig)
    ],
    declarations: [
        AdminAboutMeComponent,
        AdminAboutWebsiteComponent,
        AdminIndexComponent,
        AdminLoginComponent
    ],
    exports: [
        RouterModule,
    ]
})
export class AdminModule { }
