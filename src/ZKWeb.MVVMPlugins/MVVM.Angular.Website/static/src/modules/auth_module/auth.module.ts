import { NgModule } from '@angular/core';
import { AuthGuard } from './auth/auth-guard';
import { AppPrivilegeService } from './services/app-privilege-service';
import { AppSessionService } from './services/app-session-service';

@NgModule({
    providers: [
        AuthGuard,
        AppPrivilegeService,
        AppSessionService,
    ]
})
export class AuthModule { }
