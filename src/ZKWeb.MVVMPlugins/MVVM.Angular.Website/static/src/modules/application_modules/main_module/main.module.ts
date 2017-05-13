// Angular Imports
import { NgModule } from '@angular/core';

// This Module's Components
import { RouterModule } from '@angular/router';
import { MainComponent } from './main.component';
import { LazyLoadContainer } from './lazy-load-container';
import { applicationMainRoutesConfig } from './main.router';

@NgModule({
    imports: [
        RouterModule.forChild(applicationMainRoutesConfig)
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
