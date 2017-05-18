import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';
import { SessionInfoDto } from "@generated_module/dtos/session-info-dto";
import { AppConfigService } from '@global_module/services/app-config-service';
import { SessionService } from "@generated_module/services/session-service";

/** 获取会话信息的服务 */
@Injectable()
export class AppSessionService {
    private domSessionIdKey = "appSessionId";
    private domSessionInfoKey = "appSessionInfo";
    private sessionId: string;
    private sessionInfo: SessionInfoDto;

    constructor(
        // private store: AppStoreService,
        private appConfigService: AppConfigService,
        private sessionService: SessionService) {
        console.log("create app session service.");
    }

    /** 获取当前的会话信息 */
    getSessionInfo(): Observable<SessionInfoDto> {
        // 如果本地已有会话信息则直接返回

        let newSessionId = this.appConfigService.sessionId;
        if (newSessionId === this.sessionId && this.sessionInfo) {
            return new Observable<SessionInfoDto>(o => {
                o.next(this.sessionInfo);
                o.complete();
            });
        }
        // 调用api重新获取
        return this.sessionService.GetSessionInfo().do(result => {
            this.sessionId = newSessionId;
            this.sessionInfo = result;
        });
    }
}
