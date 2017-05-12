import { Pipe, PipeTransform } from '@angular/core';
import { AppTranslationService } from '../../global_module/services/app-translation-service';

// 翻译文本的管道
// 例: {{ "text" | trans }}
@Pipe({ name: 'gtrans' })
export class GTransPipe implements PipeTransform {
    constructor(private appTranslationService: AppTranslationService) { }
    transform(value: string): string {
        return this.appTranslationService.translate(value);
    }
}
