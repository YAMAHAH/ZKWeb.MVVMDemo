import { Component, Injector } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Http } from '@angular/http';
import { AppConfigService } from '@global_module/services/app-config-service';
import 'rxjs/add/operator/toPromise';
import { AppStoreService } from "@business_bases/desktop/global_module/services/app-store-service";
import { AppConsts } from '@business_bases/desktop/global_module/app-consts';
import { GuidUtils } from '@core/utils/guid-utils';
import { RSAUtils } from '@core/utils/rsa-utils';
import { AppApiService } from '@global_module/services/app-api-service';
import { HandshakeRequestInput } from '../../modules/generated_module/dtos/handshake-request-input';
import { AESUtils } from '@core/utils/aes-utils';
import { HandshakeRequestOutput } from "@generated_module/dtos/handshake-request-output";

export abstract class AppComponentBase {
    private http: Http;
    private router: Router;
    private appConfigService: AppConfigService;
    private store: AppStoreService;
    private apiService: AppApiService;
    routerActivated: boolean;
    constructor(protected injector: Injector) {
        this.store = injector.get(AppStoreService);
        this.appConfigService = injector.get(AppConfigService);
        this.http = injector.get(Http);
        this.apiService = injector.get(AppApiService);
        this.router = injector.get(Router);

        this.router.events.subscribe(e => {
            this.routerActivated = (e instanceof NavigationEnd);
        });
        this.appInit();
        this.getAppConfig();
        this.handshakeRequest();

    }
    public readonly publicKey: string = `MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC0xP5HcfThSQr43bAMoopbzcCy
                                                 ZWE0xfUeTA4Nx4PrXEfDvybJEIjbU\/rgANAty1yp7g20J7+wVMPCusxftl/d0rPQ
                                                 iCLjeZ3HtlRKld+9htAZtHFZosV29h/hNE9JkxzGXstaSeXIUIWquMZQ8XyscIHh
                                                 qoOmjXaCv58CSRAlAQIDAQAB`;
    appInit() {
        //随机生成密钥
        let randomKey = GuidUtils.uuid(16, 16);
        this.store.setData(AppConsts.SecretKey, randomKey);
        localStorage.setItem(AppConsts.SecretKey, randomKey);

        this.store.setData(AppConsts.SrvRsaPublicKey, this.publicKey);
        localStorage.setItem(AppConsts.SrvRsaPublicKey, this.publicKey);

        //RSA公钥密钥
        let rsaKey = RSAUtils.genRSAKey();
        this.store.setData(AppConsts.RsaPrivateKey, rsaKey.privateKey);
        localStorage.setItem(AppConsts.RsaPrivateKey, rsaKey.privateKey);
        this.store.setData(AppConsts.RsaPublicKey, rsaKey.publicKey);
        localStorage.setItem(AppConsts.RsaPublicKey, rsaKey.publicKey);
    }

    handshakeRequest() {
        let request = new HandshakeRequestInput();
        //获取加密密钥
        //使用RSA公钥加密密钥
        request.SecretKey = RSAUtils.RSAEncrypt(localStorage.getItem(AppConsts.SrvRsaPublicKey), localStorage.getItem(AppConsts.SecretKey));
        //使用AES加密公钥
        request.PublicKey = AESUtils.EncryptToBase64String(localStorage.getItem(AppConsts.SecretKey), localStorage.getItem(AppConsts.RsaPublicKey));
        this.apiService.call<HandshakeRequestOutput>("/api/CaptchaService/HandshakeRequest",
            { method: "POST", body: request }, { enableEncrypt: false })
            .subscribe(res => {
                console.debug(res);
                let chiperText = AESUtils.EncryptToBase64String(localStorage.getItem(AppConsts.SecretKey), res.TestData);
                console.debug(chiperText === res.ProcessResult);
            });
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
