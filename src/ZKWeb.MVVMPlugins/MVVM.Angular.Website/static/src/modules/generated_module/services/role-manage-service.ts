import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { ApiCallExtra } from '@global_module/models/api-call-extra';
import { GridSearchResponseDto } from '@generated_module/dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '@generated_module/dtos/grid-search-request-dto';
import { ActionResponseDto } from '@generated_module/dtos/action-response-dto';
import { RoleInputDto } from '@generated_module/dtos/role-input-dto';
import { RoleOutputDto } from '@generated_module/dtos/role-output-dto';

@Injectable()
/** 角色管理服务 */
export class RoleManageService {
    constructor(private appApiService: AppApiService) { }

    /** 搜索角色 */
    Search(request: GridSearchRequestDto, extra?: ApiCallExtra): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/RoleManageService/Search",
            {
                method: "POST",
                body: { request }
            }, extra);
    }

    /** 编辑角色 */
    Edit(dto: RoleInputDto, extra?: ApiCallExtra): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/RoleManageService/Edit",
            {
                method: "POST",
                body: { dto }
            }, extra);
    }

    /** 删除角色 */
    Remove(id: string, extra?: ApiCallExtra): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/RoleManageService/Remove",
            {
                method: "POST",
                body: { id }
            }, extra);
    }

    /** 获取所有角色 */
    GetAllRoles(extra?: ApiCallExtra): Observable<RoleOutputDto[]> {
        return this.appApiService.call<RoleOutputDto[]>(
            "/api/RoleManageService/GetAllRoles",
            {
                method: "POST"
            }, extra);
    }
}
