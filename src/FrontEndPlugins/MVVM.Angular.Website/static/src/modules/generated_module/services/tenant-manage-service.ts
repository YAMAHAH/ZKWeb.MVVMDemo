import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { ApiCallExtra } from '@global_module/models/api-call-extra';
import { GridSearchResponseDto } from '@generated_module/dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '@generated_module/dtos/grid-search-request-dto';
import { ActionResponseDto } from '@generated_module/dtos/action-response-dto';
import { TenantInputDto } from '@generated_module/dtos/tenant-input-dto';

@Injectable()
/** 租户管理服务 */
export class TenantManageService {
    constructor(private appApiService: AppApiService) { }

    /** 搜索租户 */
    Search(request: GridSearchRequestDto, extra?: ApiCallExtra): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/TenantManageService/Search",
            {
                method: "POST",
                body: { request }
            }, extra);
    }

    /** 编辑租户 */
    Edit(dto: TenantInputDto, extra?: ApiCallExtra): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/TenantManageService/Edit",
            {
                method: "POST",
                body: { dto }
            }, extra);
    }

    /** 删除租户 */
    Remove(id: string, extra?: ApiCallExtra): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/TenantManageService/Remove",
            {
                method: "POST",
                body: { id }
            }, extra);
    }
}
