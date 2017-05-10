﻿import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import {
    InputTextModule,
    PanelModule,
    ButtonModule,
    MessagesModule,
    BlockUIModule,
    GrowlModule,
    SlideMenuModule
} from 'primeng/primeng';

import { BaseModule } from '../base_module/base.module';
import { GeneratedModule } from '../generated_module/generated.module';
import { AdminContainerComponent } from './components/admin-container.component';
import { AdminToastService } from './services/admin-toast-service';

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
