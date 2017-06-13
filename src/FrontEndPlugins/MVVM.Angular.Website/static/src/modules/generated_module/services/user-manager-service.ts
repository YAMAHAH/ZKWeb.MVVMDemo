import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { ApiCallExtra } from '@global_module/models/api-call-extra';
import { GridSearchResponseDto } from '@generated_module/dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '@generated_module/dtos/grid-search-request-dto';
import { TestInput } from '@generated_module/dtos/test-input';
import { ActionResponseDto } from '@generated_module/dtos/action-response-dto';
import { UserInputDto } from '@generated_module/dtos/user-input-dto';
import { UserTypeOutputDto } from '@generated_module/dtos/user-type-output-dto';

@Injectable()
/** 用户管理服务 */
export class UserManagerService {
    constructor(private appApiService: AppApiService) { }

    /** 搜索用户 */
    Search(request: GridSearchRequestDto, extra?: ApiCallExtra): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/UserManagerService/Search",
            {
                method: "POST",
                body: { request }
            }, extra);
    }

    /** 测试基本数据类型 */
    Test(testid: string, extra?: ApiCallExtra): Observable<GridSearchResponseDto> {
        let urlParams = this.appApiService.getRequestQueryString([{ testid }]);
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/UserManagerService/test",
            {
                method: "GET",
                params: urlParams
            }, extra);
    }

    /** 测试空参数 */
    TestGet(extra?: ApiCallExtra): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/UserManagerService/TestGet",
            {
                method: "GET"
            }, extra);
    }

    /** 测试复杂对象 */
    TestObject(name: string, testInputDto: TestInput, extra?: ApiCallExtra): Observable<GridSearchResponseDto> {
        let urlParams = this.appApiService.getRequestQueryString([{ name }, { testInputDto }]);
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/UserManagerService/TestObject",
            {
                method: "GET",
                params: urlParams
            }, extra);
    }

    /** 编辑用户 */
    Edit(dto: UserInputDto, extra?: ApiCallExtra): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/UserManagerService/customEdit",
            {
                method: "POST",
                body: { dto }
            }, extra);
    }

    /** 删除用户 */
    Remove(id: string, extra?: ApiCallExtra): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/UserManagerService/Remove",
            {
                method: "POST",
                body: { id }
            }, extra);
    }

    /** 获取所有用户类型 */
    GetAllUserTypes(extra?: ApiCallExtra): Observable<UserTypeOutputDto[]> {
        return this.appApiService.call<UserTypeOutputDto[]>(
            "/api/UserManagerService/GetAllUserTypes",
            {
                method: "GET"
            }, extra);
    }
}
