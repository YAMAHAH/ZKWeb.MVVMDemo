import { Injectable } from '@angular/core';
import { AppConfigService } from './app-config-service';
import { TranslationIndex } from '@generated_module/translations/index';

// 提供文本翻译的服务
@Injectable()
export class AppTranslationService {
    protected translation: any;

    constructor(protected appConfigService: AppConfigService) {
        let language = appConfigService.language;
        TranslationIndex.translationModules.forEach(translation => {
            if (translation.language === language) {
                this.translation = translation;
            }
        });
    }

    // 翻译指定文本，翻译不存在时返回原文本,异步坑爹
    translate(text: string) {
        if (!this.translation || !text) {
            return text;
        }
        return this.translation.translations[text] || text;
    }
}
