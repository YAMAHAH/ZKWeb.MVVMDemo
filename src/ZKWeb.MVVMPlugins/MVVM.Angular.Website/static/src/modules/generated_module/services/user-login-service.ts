import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { rxResultConverter, rxErrorConverter } from '@core/utils/type-utils';
import { ActionResponseDto } from '@generated_module/dtos/action-response-dto';
import { UserLoginRequestDto } from '@generated_module/dtos/user-login-request-dto';

@Injectable()
/** 用户登录服务 */
export class UserLoginService {
    constructor(private appApiService: AppApiService) { }

    /** 登录用户 */
    LoginUser(request: UserLoginRequestDto, resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/UserLoginService/LoginUser",
            {
                method: "POST",
                body: { request }
            }, resultConverter, errorConverter);
    }

    /** 登录管理员 */
    LoginAdmin(request: UserLoginRequestDto, resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/UserLoginService/LoginAdmin",
            {
                method: "POST",
                body: { request }
            }, resultConverter, errorConverter);
    }
}
