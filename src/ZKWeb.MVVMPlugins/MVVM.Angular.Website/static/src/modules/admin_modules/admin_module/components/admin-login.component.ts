import { Component, OnInit, EventEmitter } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Message } from 'primeng/primeng';
import { UserLoginService } from "@generated_module/services/user-login-service";

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
        this.userLoginService.LoginAdmin(this.adminLoginForm.value).subscribe(
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
