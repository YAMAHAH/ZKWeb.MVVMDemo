import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { ApiCallExtra } from '@global_module/models/api-call-extra';
import { ActionResponseDto } from '@generated_module/dtos/action-response-dto';
import { UserChangePasswordInputDto } from '@generated_module/dtos/user-change-password-input-dto';
import { UserUploadAvatarInputDto } from '@generated_module/dtos/user-upload-avatar-input-dto';

@Injectable()
/** 用户资料服务 */
export class UserProfileService {
    constructor(private appApiService: AppApiService) { }

    /** 修改密码 */
    ChangePassword(dto: UserChangePasswordInputDto, extra?: ApiCallExtra): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/UserProfileService/ChangePassword",
            {
                method: "POST",
                body: { dto }
            }, extra);
    }

    /** 上传头像 */
    UploadAvatar(dto: UserUploadAvatarInputDto, extra?: ApiCallExtra): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/UserProfileService/UploadAvatar",
            {
                method: "POST",
                body: { dto }
            }, extra);
    }
}
