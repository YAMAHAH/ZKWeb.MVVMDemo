import { Component, Input } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';
import { AppTranslationService } from '@global_module/services/app-translation-service';

@Component({
    moduleId: module.id,
    selector: 'z-form-textarea',
    templateUrl: '../views/form-textarea.html',
    host: { 'class': 'ui-grid-row' }
})
export class FormTextAreaComponent extends FormFieldBaseComponent {
    constructor(protected appTranslationService: AppTranslationService) {
        super(appTranslationService);
    }
    @Input() rows: number = 5;
}
