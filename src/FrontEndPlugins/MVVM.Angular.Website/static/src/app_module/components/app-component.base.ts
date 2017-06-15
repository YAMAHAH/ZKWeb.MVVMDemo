﻿import { Component, Injector } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Http } from '@angular/http';
import { AppConfigService } from '@global_module/services/app-config-service';
import 'rxjs/add/operator/toPromise';
import { AppStoreService } from "@business_bases/desktop/global_module/services/app-store-service";
import { AppConsts } from '@business_bases/desktop/global_module/app-consts';
import { GuidUtils } from '@core/utils/guid-utils';
import { RSAUtils } from '@core/utils/rsa-utils';
import { AppApiService } from '@global_module/services/app-api-service';
import { HandshakeRequestInput } from '@generated_module/dtos/handshake-request-input';
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
    }
    public readonly publicKey: string = `MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC0xP5HcfThSQr43bAMoopbzcCy
                                         ZWE0xfUeTA4Nx4PrXEfDvybJEIjbU\/rgANAty1yp7g20J7+wVMPCusxftl/d0rPQ
                                         iCLjeZ3HtlRKld+9htAZtHFZosV29h/hNE9JkxzGXstaSeXIUIWquMZQ8XyscIHh
                                         qoOmjXaCv58CSRAlAQIDAQAB`;

    /**
     * 程序初始化
     */
    async appInit() {
        let conf = await this.getAppConfig();
        let saveToLocal = conf && !!conf.saveToLocal;
        if (!!!saveToLocal) localStorage.clear();
        let clientDataModel = this.store.getData<ClientDataModel>(AppConsts.ClientDataKey);
        if (!!!clientDataModel) {
            //随机生成密钥
            let randomKey = GuidUtils.uuid(16, 16);
            debugger;
            //RSA公钥密钥
            let rsaKey = RSAUtils.genRSAKey();

            clientDataModel = new ClientDataModel(randomKey, rsaKey.publicKey, rsaKey.privateKey, this.publicKey);
            this.store.saveData(AppConsts.ClientDataKey, clientDataModel);
        }
        await this.handshakeRequest();
    }

    /**
     * 程序首次启动时与服务器握手交换信息
     */
    async handshakeRequest(): Promise<boolean> {
        let request = new HandshakeRequestInput();
        //获取加密密钥
        let clientData = this.store.getData<ClientDataModel>(AppConsts.ClientDataKey);
        //使用RSA公钥加密密钥
        request.SecretKey = RSAUtils.RSAEncrypt(clientData.ServerRsaPublicKey, clientData.SecretKey);
        //使用AES加密公钥
        request.PublicKey = AESUtils.EncryptToBase64String(clientData.SecretKey, clientData.RsaPublickKey);
        return new Promise<boolean>(resolve => {
            this.apiService.call<HandshakeRequestOutput>("/api/CaptchaService/HandshakeRequest",
                { method: "POST", body: request }, { enableEncrypt: false })
                .subscribe(res => {
                    let chiperText = AESUtils.EncryptToBase64String(clientData.SecretKey, res.TestData);
                    if (chiperText === res.ProcessResult) {
                        resolve(true);
                    } else {
                        resolve(false);
                    }
                });
        });
    }

    /**
     * 获取应用程序配置文件
     */
    async getAppConfig() {
        return new Promise<any>(resolve => {
            this.http.get("app-config.json")
                .toPromise()
                .then(res => {
                    let conf = res.json();
                    window[AppConsts.AppConfigKey] = conf;
                    this.store.saveData(AppConsts.AppConfigKey, conf);
                    this.appConfigService.initConfig(conf);
                    resolve(conf);
                });
        });
    }
}