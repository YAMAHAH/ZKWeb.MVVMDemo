// Angular Imports
import { NgModule } from '@angular/core';

// This Module's Components
import { RouterModule } from '@angular/router';
import { MainComponent } from './main.component';
import { LazyLoadContainer } from './lazy-load-container';
import { applicationMainRoutesConfig } from './main.router';
import { GeneratedModule } from '../../generated_module/generated.module';
import { AuthModule } from '../../auth_module/auth.module';

@NgModule({
    imports: [
        GeneratedModule,
        AuthModule,
        RouterModule.forChild(applicationMainRoutesConfig),

    ],
    declarations: [
        MainComponent, LazyLoadContainer
    ],
    exports: [
        MainComponent, LazyLoadContainer
    ]
})
export class MainModule {

}
