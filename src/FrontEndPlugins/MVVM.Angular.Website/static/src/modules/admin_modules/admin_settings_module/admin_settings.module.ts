﻿import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { AdminWebsiteSettingsComponent } from './components/admin-website-settings.component';
import { InputTextModule } from 'primeng/components/inputtext/inputtext';
import { PanelModule } from 'primeng/components/panel/panel';
import { ButtonModule } from 'primeng/components/button/button';
import { MessagesModule } from 'primeng/components/messages/messages';
import { BlockUIModule } from 'primeng/components/blockui/blockui';
import { AuthGuard } from "@auth_module/auth/auth-guard";
import { Privileges } from "@generated_module/privileges/privileges";
import { UserTypes } from '@generated_module/privileges/user-types';
import { AdminBaseModule } from '@admin_modules/admin_base_module/admin_base.module';
import { BaseModule } from '@business_bases/desktop/base_module/base.module';

const routes: Routes = [
    { path: '', redirectTo: 'website_settings', pathMatch: 'full' },
    {
        path: 'website_settings',
        component: AdminWebsiteSettingsComponent,
        canActivate: [AuthGuard],
        data: {
            auth: {
                requireUserType: UserTypes.IAmAdmin,
                requirePrivileges: [Privileges.Settings_WebsiteSettings]
            }
        }
    }
];

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
        BaseModule,
        AdminBaseModule,
        RouterModule.forChild(routes)
    ],
    declarations: [
        AdminWebsiteSettingsComponent,
    ],
    exports: [
        RouterModule
    ]
})
export class AdminSettingsModule { }