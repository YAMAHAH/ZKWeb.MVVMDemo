﻿import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { BaseModule } from '../base_module/base.module';
import { AdminBaseModule } from '../admin_base_module/admin_base.module';

import { AuthGuard } from '../auth_module/auth/auth-guard';
import { UserTypes } from '../generated_module/privileges/user-types';
import { Privileges } from '../generated_module/privileges/privileges';

import { AdminRoleListComponent } from './components/admin-role-list.component';
import { InputTextModule } from "primeng/components/inputtext/inputtext";
import { PanelModule } from "primeng/components/panel/panel";
import { ButtonModule } from "primeng/components/button/button";
import { MessagesModule } from "primeng/components/messages/messages";
import { BlockUIModule } from "primeng/components/blockui/blockui";
import { DropdownModule } from "primeng/components/dropdown/dropdown";
import { MultiSelectModule } from "primeng/components/multiselect/multiselect";
import { DataTableModule } from "primeng/components/datatable/datatable";
import { ConfirmDialogModule } from "primeng/components/confirmdialog/confirmdialog";
import { DialogModule } from "primeng/components/dialog/dialog";

const routes: Routes = [
    {
        path: '',
        component: AdminRoleListComponent,
        pathMatch: 'full',
        canActivate: [AuthGuard],
        data: {
            auth: {
                requireUserType: UserTypes.IAmAdmin,
                requirePrivileges: [Privileges.Role_View]
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
        AdminRoleListComponent,
    ]
})
export class AdminRolesModule { }
