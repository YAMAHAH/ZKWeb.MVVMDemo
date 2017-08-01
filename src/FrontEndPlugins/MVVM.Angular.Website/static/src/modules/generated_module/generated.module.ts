import { NgModule } from '@angular/core';
import { CaptchaService } from './services/captcha-service';
import { GlobalManageService } from './services/global-manage-service';
import { RoleManageService } from './services/role-manage-service';
import { SessionService } from './services/session-service';
import { TenantManageService } from './services/tenant-manage-service';
import { UserLoginService } from './services/user-login-service';
import { UserManageService } from './services/user-manage-service';
import { UserProfileService } from './services/user-profile-service';
import { WebsiteManageService } from './services/website-manage-service';
import { ExampleDataManageService } from './services/example-data-manage-service';
import { SaleOrderManageService } from './services/sale-order-manage-service';

@NgModule({
    providers: [
        CaptchaService,
        GlobalManageService,
        RoleManageService,
        SessionService,
        TenantManageService,
        UserLoginService,
        UserManageService,
        UserProfileService,
        WebsiteManageService,
        ExampleDataManageService,
        SaleOrderManageService
    ]
})
export class GeneratedModule { }
