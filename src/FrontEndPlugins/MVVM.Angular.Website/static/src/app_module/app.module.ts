import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GlobalModule } from "@global_module/global.module";
import { RouterModule } from '@angular/router';
import { appRootRoutesConfig } from './appRootRoutesConfig';
import { AppComponent } from './components/app.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        GlobalModule.forRoot(),
        RouterModule.forRoot(appRootRoutesConfig)
    ],
    declarations: [
        AppComponent,
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
