﻿import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Message } from 'primeng/primeng';
import { UserProfileService } from "@generated_module/services/user-profile-service";
import { AppTranslationService } from "@global_module/services/app-translation-service";
import { AppSessionService } from "@auth_module/services/app-session-service";
import { AppPrivilegeService } from "@auth_module/services/app-privilege-service";

@Component({
    moduleId: module.id,
    selector: 'admin-about-me',
    templateUrl: '../views/admin-about-me.html'
})
export class AdminAboutMeComponent implements OnInit {
    tenant: string;
    username: string;
    userType: string;
    roles: string[] = [];
    privileges: string[] = [];
    msgs: Message[] = [];
    isSubmitting = false;
    changePasswordForm = new FormGroup({
        OldPassword: new FormControl('', Validators.compose([Validators.required, Validators.minLength(6)])),
        NewPassword: new FormControl('', Validators.compose([Validators.required, Validators.minLength(6)])),
        ConfirmNewPassword: new FormControl('', Validators.compose([Validators.required, Validators.minLength(6)]))
    });
    avatarUploadForm = new FormGroup({
        Avatar: new FormControl('', Validators.required),
    });

    constructor(
        private appTranslationService: AppTranslationService,
        private appSessionService: AppSessionService,
        private appPrivilegeService: AppPrivilegeService,
        private userProfileService: UserProfileService) {
    }

    ngOnInit() {
        console.debug("5555");
        this.appSessionService.getSessionInfo().subscribe(
            s => {
                this.tenant = s.User.OwnerTenantName;
                this.username = s.User.Username;
                this.userType = this.appTranslationService.translate(s.User.Type);
                this.roles = s.User.Roles.map(r => r.Name);
                this.privileges = s.User.Privileges.map(p => this.appPrivilegeService.translatePrivilege(p));
            },
            e => this.msgs = [{ severity: "error", detail: e }]);
    }

    onChangePassword() {
        this.isSubmitting = true;
        this.userProfileService.ChangePassword(this.changePasswordForm.value).subscribe(
            s => {
                this.msgs = [{ severity: "info", detail: s.Message }];
                this.isSubmitting = false;
            },
            e => {
                this.msgs = [{ severity: "error", detail: e }];
                this.isSubmitting = false;
            });
    }

    onUploadAvatar() {
        this.isSubmitting = true;
        this.userProfileService.UploadAvatar(this.avatarUploadForm.value).subscribe(
            s => {
                this.msgs = [{ severity: "info", detail: s.Message }];
                this.isSubmitting = false;
            },
            e => {
                this.msgs = [{ severity: "error", detail: e }];
                this.isSubmitting = false;
            });
    }
}
