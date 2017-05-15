import { Component, Injector } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { AppComponentBase } from './app-component.base';

@Component({
    moduleId: module.id,
    selector: 'my-app',
    templateUrl: '../views/app.html'
})
export class AppHMRComponent extends AppComponentBase {
    constructor(protected injector: Injector) {
        super(injector);
    }
}
