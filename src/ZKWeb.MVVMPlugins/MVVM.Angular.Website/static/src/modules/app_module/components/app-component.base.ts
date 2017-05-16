import { Component, Injector } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Http } from '@angular/http';
import { AppConfigService } from '@global_module/services/app-config-service';
import 'rxjs/add/operator/toPromise';
import { AppStoreService } from '../../global_module/services/app-store-service';
import { AppStore } from '../../../dev/app-store';
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
        this.getAppConfig();
    }
    async getAppConfig() {
        return new Promise(resolve => {
            this.http.get("app-config.json")
                .toPromise()
                .then(res => {
                    let conf = res.json();
                    window['appConfig'] = conf;
                    this.store.setData('appConfig', conf);
                    localStorage.setItem('appConfig', JSON.stringify(conf));
                    this.appConfigService.initConfig(conf);
                });
        });
    }
}
