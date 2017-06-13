import { Component, OnInit, EventEmitter } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Message } from 'primeng/primeng';
import { UserLoginService } from "@generated_module/services/user-login-service";
import { UserLoginRequestDto } from '@generated_module/dtos/user-login-request-dto';
import { AppConsts } from '@business_bases/desktop/global_module/app-consts';
import { RSAUtils } from '@core/utils/rsa-utils';
import { AESUtils } from '@core/utils/aes-utils';

@Component({
    moduleId: module.id,
    selector: 'admin-login',
    templateUrl: '../views/admin-login.html',
    styleUrls: ['../styles/admin-login.scss']
})
export class AdminLoginComponent implements OnInit {
    adminLoginForm = new FormGroup({
        Tenant: new FormControl('', Validators.required),
        Username: new FormControl('', Validators.required),
        Password: new FormControl('', Validators.compose([Validators.required, Validators.minLength(6)])),
        Captcha: new FormControl('', Validators.compose([Validators.required, Validators.minLength(4), Validators.maxLength(4)]))
    });
    logoUrl = require("@vendor/images/logo.png");
    msgs: Message[] = [];
    captchaRefreshEvent = new EventEmitter();
    isSubmitting = false;

    targetUrl = "";

    constructor(
        private router: Router,
        private activeRoute: ActivatedRoute,
        private userLoginService: UserLoginService) {
        this.activeRoute
            .params
            .map(p => p['targetUrl'])
            .subscribe(url => {
                if (url) this.targetUrl = decodeURIComponent(url);
            });
    }

    gotoUrl(target: string, defaultPage: string) {
        this.router.navigateByUrl(target ? target : defaultPage);
    }

    onSubmit() {
        this.isSubmitting = true;
        let requestDto: UserLoginRequestDto = this.adminLoginForm.value;
        //获取加密密钥
        //使用RSA公钥加密密钥
        requestDto.SecretKey = RSAUtils.RSAEncrypt(localStorage.getItem(AppConsts.SrvRsaPublicKey), localStorage.getItem(AppConsts.SecretKey));
        //使用AES加密公钥
        requestDto.PublicKey = AESUtils.EncryptToBase64String(localStorage.getItem(AppConsts.SecretKey), localStorage.getItem(AppConsts.RsaPublicKey));

        this.userLoginService.LoginAdmin(requestDto).subscribe(
            result => {
                this.isSubmitting = false;
                this.gotoUrl("/admin", "/");
            },
            error => {
                this.isSubmitting = false;
                this.msgs = [{ severity: 'error', detail: error }];
                this.captchaRefreshEvent.emit();
            });
    }

    ngOnInit() {
        this.adminLoginForm.patchValue({ Tenant: "Master" });
    }
}
