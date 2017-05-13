import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { AdminContainerComponent } from './components/admin-container.component';
import { AdminToastService } from './services/admin-toast-service';
import { InputTextModule } from 'primeng/components/inputtext/inputtext';
import { PanelModule } from 'primeng/components/panel/panel';
import { ButtonModule } from 'primeng/components/button/button';
import { MessagesModule } from 'primeng/components/messages/messages';
import { BlockUIModule } from 'primeng/components/blockui/blockui';
import { GrowlModule } from "primeng/components/growl/growl";
import { SlideMenuModule } from "primeng/components/slidemenu/slidemenu";
import { BaseModule } from "@base_module/base.module";

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
        GrowlModule,
        SlideMenuModule,
        BaseModule,
        RouterModule 
    ],
    declarations: [
        AdminContainerComponent,
    ],
    providers: [
        AdminToastService
    ],
    exports: [
        AdminContainerComponent
    ]
})
export class AdminBaseModule { }
