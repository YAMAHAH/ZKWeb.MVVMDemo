import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { ApiCallExtra } from '@global_module/models/api-call-extra';
import { GridSearchResponseDto } from '@generated_module/dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '@generated_module/dtos/grid-search-request-dto';
import { ActionResponseDto } from '@generated_module/dtos/action-response-dto';
import { ExampleDataInputDto } from '@generated_module/dtos/example-data-input-dto';

@Injectable()
/** 示例数据管理服务 */
export class ExampleDataManageService {
    constructor(private appApiService: AppApiService) { }

    /** 搜索数据 */
    Search(request: GridSearchRequestDto, extra?: ApiCallExtra): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/ExampleDataManageService/Search",
            {
                method: "POST",
                body: { request }
            }, extra);
    }

    /** 编辑数据 */
    Edit(dto: ExampleDataInputDto, extra?: ApiCallExtra): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/ExampleDataManageService/Edit",
            {
                method: "POST",
                body: { dto }
            }, extra);
    }

    /** 删除数据 */
    Remove(id: string, extra?: ApiCallExtra): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/ExampleDataManageService/Remove",
            {
                method: "POST",
                body: { id }
            }, extra);
    }
}
