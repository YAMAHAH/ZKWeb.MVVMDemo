import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { ApiCallExtra } from '@global_module/models/api-call-extra';
import { ActionResponseDto } from '@generated_module/dtos/action-response-dto';

@Injectable()
/** 全局管理服务 */
export class GlobalManageService {
    constructor(private appApiService: AppApiService) { }

    /** 删除数据 */
    Test(extra?: ApiCallExtra): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/GlobalManageService/Test",
            {
                method: "GET"
            }, extra);
    }
}
