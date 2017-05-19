import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { rxResultConverter, rxErrorConverter } from '@core/utils/type-utils';
import { GridSearchResponseDto } from '@generated_module/dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '@generated_module/dtos/grid-search-request-dto';
import { ActionResponseDto } from '@generated_module/dtos/action-response-dto';
import { TenantInputDto } from '@generated_module/dtos/tenant-input-dto';

@Injectable()
/** 租户管理服务 */
export class TenantManageService {
    constructor(private appApiService: AppApiService) { }

    /** 搜索租户 */
    Search(request: GridSearchRequestDto, resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/TenantManageService/Search",
            {
                method: "POST",
                body: { request }
            }, resultConverter, errorConverter);
    }

    /** 编辑租户 */
    Edit(dto: TenantInputDto, resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/TenantManageService/Edit",
            {
                method: "POST",
                body: { dto }
            }, resultConverter, errorConverter);
    }

    /** 删除租户 */
    Remove(id: string, resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/TenantManageService/Remove",
            {
                method: "POST",
                body: { id }
            }, resultConverter, errorConverter);
    }
}
