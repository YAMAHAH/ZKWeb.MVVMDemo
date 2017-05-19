import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { rxResultConverter, rxErrorConverter } from '@core/utils/type-utils';
import { GridSearchResponseDto } from '@generated_module/dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '@generated_module/dtos/grid-search-request-dto';
import { ActionResponseDto } from '@generated_module/dtos/action-response-dto';
import { ExampleDataInputDto } from '@generated_module/dtos/example-data-input-dto';

@Injectable()
/** 示例数据管理服务 */
export class ExampleDataManageService {
    constructor(private appApiService: AppApiService) { }

    /** 搜索数据 */
    Search(request: GridSearchRequestDto, resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/ExampleDataManageService/Search",
            {
                method: "POST",
                body: { request }
            }, resultConverter, errorConverter);
    }

    /** 编辑数据 */
    Edit(dto: ExampleDataInputDto, resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/ExampleDataManageService/Edit",
            {
                method: "POST",
                body: { dto }
            }, resultConverter, errorConverter);
    }

    /** 删除数据 */
    Remove(id: string, resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/ExampleDataManageService/Remove",
            {
                method: "POST",
                body: { id }
            }, resultConverter, errorConverter);
    }
}
