import { Component, Input } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';
import { SelectItem } from 'primeng/primeng';
import { AppTranslationService } from "@global_module/services/app-translation-service";

@Component({
    moduleId: module.id,
    selector: 'z-form-dropdown',
    templateUrl: '../views/form-dropdown.html',
    host: { 'class': 'ui-grid-row' }
})
export class FormDropdownComponent extends FormFieldBaseComponent {
    constructor(protected appTranslationService: AppTranslationService) {
        super(appTranslationService);
    }
    @Input() options: SelectItem[];
}
