﻿import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import {
    InputTextModule,
    PanelModule,
    ButtonModule,
    MessagesModule,
    BlockUIModule,
    DataTableModule,
    DropdownModule,
    MultiSelectModule,
    DialogModule,
    ConfirmDialogModule
} from 'primeng/primeng';

import { BaseModule } from '../base_module/base.module';
import { AdminBaseModule } from '../admin_base_module/admin_base.module';

import { AuthGuard } from '../auth_module/auth/auth-guard';
import { UserTypes } from '../generated_module/privileges/user-types';
import { Privileges } from '../generated_module/privileges/privileges';

import { AdminTenantListComponent } from './components/admin-tenant-list.component';

const routes: Routes = [
    {
        path: '',
        component: AdminTenantListComponent,
        pathMatch: 'full',
        canActivate: [AuthGuard],
        data: {
            auth: {
                requireMasterTenant: true,
                requireUserType: UserTypes.IAmAdmin,
                requirePrivileges: [Privileges.Tenant_View]
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
        DataTableModule,
        DropdownModule,
        MultiSelectModule,
        DialogModule,
        ConfirmDialogModule,
        BaseModule,
        AdminBaseModule,
        RouterModule.forChild(routes)
    ],
    declarations: [
        AdminTenantListComponent,
    ],
    exports: [
        RouterModule
    ]
})
export class AdminTenantsModule { }
