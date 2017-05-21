import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { ApiCallExtra } from '@global_module/models/api-call-extra';
import { SessionInfoDto } from '@generated_module/dtos/session-info-dto';

@Injectable()
/** 会话服务 */
export class SessionService {
    constructor(private appApiService: AppApiService) { }

    /** 获取当前的会话信息 */
    GetSessionInfo(extra?: ApiCallExtra): Observable<SessionInfoDto> {
        return this.appApiService.call<SessionInfoDto>(
            "/api/SessionService/GetSessionInfo",
            {
                method: "POST"
            }, extra);
    }
}
