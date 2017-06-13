import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GlobalModule } from '@global_module/global.module';
import { RouterModule } from '@angular/router';
import { appRootRoutesConfig } from './appRootRoutesConfig';
import { AppHMRComponent } from './components/app-hmr.component';

@NgModule({
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        GlobalModule.forRoot(),
        RouterModule.forRoot(appRootRoutesConfig, { useHash: false })
    ],
    declarations: [
        AppHMRComponent
    ]
})
export class AppHmrModule { }
