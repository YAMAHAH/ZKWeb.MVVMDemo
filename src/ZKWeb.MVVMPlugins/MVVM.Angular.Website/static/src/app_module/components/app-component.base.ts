import { Component, Injector } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Http } from '@angular/http';
import { AppConfigService } from '@global_module/services/app-config-service';
import 'rxjs/add/operator/toPromise';
import { AppStoreService } from "@business_bases/desktop/global_module/services/app-store-service";
import { AppConsts } from '@business_bases/desktop/global_module/app-consts';
import { GuidUtils } from '@core/utils/guid-utils';
import { RSAUtils } from '@core/utils/rsa-utils';

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
        this.appInit();
        this.getAppConfig();

    }

    appInit() {
        //随机生成密钥
        let randomKey ="99b3ad6e"; // GuidUtils.uuid(16, 16);
        this.store.setData(AppConsts.SecretKey, randomKey);
        localStorage.setItem(AppConsts.SecretKey, randomKey);
        //RSA公钥密钥
        let rsaKey = RSAUtils.genRSAKey();
        this.store.setData(AppConsts.RsaPrivateKey, rsaKey.privateKey);
        localStorage.setItem(AppConsts.RsaPrivateKey, rsaKey.privateKey);
        this.store.setData(AppConsts.RsaPublicKey, rsaKey.publicKey);
        localStorage.setItem(AppConsts.RsaPublicKey, rsaKey.publicKey);
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
