import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { AdminBaseModule } from '../admin_base_module/admin_base.module';

import { AdminTenantListComponent } from './components/admin-tenant-list.component';
import { InputTextModule } from 'primeng/components/inputtext/inputtext';
import { PanelModule } from 'primeng/components/panel/panel';
import { ButtonModule } from 'primeng/components/button/button';
import { MessagesModule } from 'primeng/components/messages/messages';
import { BlockUIModule } from 'primeng/components/blockui/blockui';
import { DataTableModule } from 'primeng/components/datatable/datatable';
import { DropdownModule } from 'primeng/components/dropdown/dropdown';
import { MultiSelectModule } from 'primeng/components/multiselect/multiselect';
import { DialogModule } from 'primeng/components/dialog/dialog';
import { ConfirmDialogModule } from 'primeng/components/confirmdialog/confirmdialog';
import { UserTypes } from '@generated_module/privileges/user-types';
import { AuthGuard } from '@auth_module/auth/auth-guard';
import { Privileges } from '@generated_module/privileges/privileges';
import { BaseModule } from '@business_bases/desktop/base_module/base.module';

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
export class AdminTenantsModule {

    u = {
        RItems: [
            {
                Rid: "111",
                RName: "R对象名称",
                Citems: [
                    { cid: 123, cname: "C对象名称" },
                    { cid: 126, cname: "C对象名称" },
                ]
            },
            {
                Rid: "112",
                RName: "R对象名称",
                Citems: [
                    { cid: 127, cname: "C对象名称" },
                    { cid: 128, cname: "C对象名称" },
                ]
            }
        ],
        p: {
            pv: {
                prop1: "pv对象属性1"
            },
            prop2: "p对象属性2",
        },
        prop3: "U对象属性3"

    }
}
