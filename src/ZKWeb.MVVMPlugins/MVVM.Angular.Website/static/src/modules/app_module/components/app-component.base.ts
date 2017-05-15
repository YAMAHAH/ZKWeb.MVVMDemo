import { Component, Injector } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Http } from '@angular/http';
import { AppConfigService } from '@global_module/services/app-config-service';
import 'rxjs/add/operator/toPromise';
import { AppStoreService } from '../../global_module/services/app-store-service';
export abstract class AppComponentBase {
    private http: Http;
    private router: Router;
    private appConfigService: AppConfigService;
    private store: AppStoreService;
    routerActivated: boolean;
    constructor(protected injector: Injector) {
        this.store = injector.get(AppStoreService);
        this.appConfigService = injector.get(AppConfigService);
        this.http = injector.get(Http);
        this.router = injector.get(Router);
        this.router.events.subscribe(e => {
            this.routerActivated = (e instanceof NavigationEnd);
        });
        this.initAppConfig();
    }

    async initAppConfig() {
        return new Promise(resolve => {
            this.http.get("app-config.json")
                .toPromise()
                .then(config => {
                    this.store.setData('appConfig', config.json());
                });
        });
    }
}
