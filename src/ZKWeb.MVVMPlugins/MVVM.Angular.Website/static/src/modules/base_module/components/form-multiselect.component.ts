import { Component, Input } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';
import { SelectItem } from 'primeng/primeng';
import { AppTranslationService } from "@global_module/services/app-translation-service";

@Component({
    moduleId: module.id,
    selector: 'z-form-multiselect',
    templateUrl: '../views/form-multiselect.html',
    host: { 'class': 'ui-grid-row' }
})
export class FormMultiSelectComponent extends FormFieldBaseComponent {
    constructor(protected appTranslationService: AppTranslationService) {
        super(appTranslationService);
    }
    @Input() options: SelectItem[];
    @Input() defaultLabel: null;
}
