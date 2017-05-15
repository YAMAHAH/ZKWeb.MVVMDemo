import { Component, Injector } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

export abstract class AppComponentBase {

    private router: Router;
    routerActivated: boolean;
    constructor(protected injector: Injector) {
        this.router = injector.get(Router);
        this.router.events.subscribe(e => {
            this.routerActivated = (e instanceof NavigationEnd);
        });
    }
}
