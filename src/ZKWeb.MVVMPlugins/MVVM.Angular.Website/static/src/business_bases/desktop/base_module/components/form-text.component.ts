﻿import { Component } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';
import { AppTranslationService } from '@global_module/services/app-translation-service';

@Component({
    moduleId: module.id,
    selector: 'z-form-text',
    templateUrl: '../views/form-text.html',
    host: { 'class': 'ui-grid-row' }
})
export class FormTextComponent extends FormFieldBaseComponent {
    constructor(protected appTranslationService: AppTranslationService) {
        super(appTranslationService);
    }

}