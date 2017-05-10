import { NgModule, ModuleWithProviders } from '@angular/core';
import { AppTranslationService } from './services/app-translation-service';
import { AppConfigService } from './services/app-config-service';
import { AppApiService } from './services/app-api-service';
import { AuthModule } from '../auth_module/auth.module';
import { TransPipe } from './pipes/trans-pipe';

@NgModule({
    imports: [
        AuthModule
    ],
    declarations: [
        TransPipe
    ],
    exports: [
        TransPipe
    ]
})
export class GlobalModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: GlobalModule,
            providers: [
                AppApiService,
                AppConfigService,
                AppTranslationService
            ]
        };
    }
}
