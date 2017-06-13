import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { ApiCallExtra } from '@global_module/models/api-call-extra';

@Injectable()
/** 销售订单管理服务 */
export class SaleOrderManageService {
    constructor(private appApiService: AppApiService) { }

}
