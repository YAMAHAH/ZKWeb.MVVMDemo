import { NgModule, ModuleWithProviders } from '@angular/core';
import { AppTranslationService } from './services/app-translation-service';
import { AppConfigService } from './services/app-config-service';
import { AppApiService } from './services/app-api-service';
import { GTransPipe } from './pipes/trans-pipe';
import { HttpModule } from '@angular/http';
import { RouterOutletComponent } from './components/router-outlet.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        CommonModule,
        HttpModule,
        RouterModule
    ],
    declarations: [
        RouterOutletComponent,
        GTransPipe
    ],
    exports: [
        GTransPipe,
        RouterOutletComponent,
        CommonModule,
        HttpModule,
        RouterModule
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
