import { NgModule, ModuleWithProviders } from '@angular/core';
import { AppTranslationService } from './services/app-translation-service';
import { AppConfigService } from './services/app-config-service';
import { AppApiService } from './services/app-api-service';
import { GTransPipe } from './pipes/trans-pipe';
import { HttpModule } from '@angular/http';

@NgModule({
    imports: [
        HttpModule
    ],
    declarations: [
        GTransPipe
    ],
    exports: [
        GTransPipe
    ]
})
export class GlobalModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: GlobalModule,
            providers: [
                AppApiService,
                AppConfigService,
                AppTranslationService,
            ]
        };
    }
}
