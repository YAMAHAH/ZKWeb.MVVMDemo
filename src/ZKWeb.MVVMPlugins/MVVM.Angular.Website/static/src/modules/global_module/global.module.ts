import { NgModule, ModuleWithProviders } from '@angular/core';
import { AppTranslationService } from './services/app-translation-service';
import { AppConfigService } from './services/app-config-service';
import { AppApiService } from './services/app-api-service';
import { TransPipe } from './pipes/trans-pipe';
import { AuthGuard } from '../auth_module/auth/auth-guard';
import { AppPrivilegeService } from '../auth_module/services/app-privilege-service';
import { AppSessionService } from '../auth_module/services/app-session-service';

@NgModule({
    imports: [

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
                AppTranslationService,
            ]
        };
    }
}
