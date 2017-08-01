import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { ApiCallExtra } from '@global_module/models/api-call-extra';

@Injectable()
/** 全局管理服务 */
export class GlobalManageService {
    constructor(private appApiService: AppApiService) { }

}
