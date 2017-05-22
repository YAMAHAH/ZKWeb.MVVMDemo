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
import { ClientDataModel } from "@global_module/models/client-data-model";

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
        //RSA公钥密钥
        let rsaKey = RSAUtils.genRSAKey();
        let clientDataModel = new ClientDataModel(randomKey, rsaKey.publicKey, rsaKey.privateKey, this.publicKey);
        this.store.setData(AppConsts.ClientDataKey, clientDataModel);
    }

    handshakeRequest() {
        let request = new HandshakeRequestInput();
        //获取加密密钥
        let clientData: ClientDataModel = this.store.getData(AppConsts.ClientDataKey);
        //使用RSA公钥加密密钥
        request.SecretKey = RSAUtils.RSAEncrypt(clientData.ServerRsaPublicKey, clientData.SecretKey);
        //使用AES加密公钥
        request.PublicKey = AESUtils.EncryptToBase64String(clientData.SecretKey, clientData.RsaPublickKey);
        this.apiService.call<HandshakeRequestOutput>("/api/CaptchaService/HandshakeRequest",
            { method: "POST", body: request }, { enableEncrypt: false })
            .subscribe(res => {
                let chiperText = AESUtils.EncryptToBase64String(clientData.SecretKey, res.TestData);
                if (chiperText === res.ProcessResult) console.debug("HandshakeReuest Sucessful");
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
