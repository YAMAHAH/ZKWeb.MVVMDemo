import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { rxResultConverter, rxErrorConverter } from '@core/utils/type-utils';
import { SessionInfoDto } from '@generated_module/dtos/session-info-dto';

@Injectable()
/** 会话服务 */
export class SessionService {
    constructor(private appApiService: AppApiService) { }

    /** 获取当前的会话信息 */
    GetSessionInfo(resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<SessionInfoDto> {
        return this.appApiService.call<SessionInfoDto>(
            "/api/SessionService/GetSessionInfo",
            {
                method: "POST"
            }, resultConverter, errorConverter);
    }
}
