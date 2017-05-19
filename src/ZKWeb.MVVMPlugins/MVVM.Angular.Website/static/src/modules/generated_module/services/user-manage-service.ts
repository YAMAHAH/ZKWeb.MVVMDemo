import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { rxResultConverter, rxErrorConverter } from '@core/utils/type-utils';
import { GridSearchResponseDto } from '@generated_module/dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '@generated_module/dtos/grid-search-request-dto';
import { TestInput } from '@generated_module/dtos/test-input';
import { ActionResponseDto } from '@generated_module/dtos/action-response-dto';
import { UserInputDto } from '@generated_module/dtos/user-input-dto';
import { UserTypeOutputDto } from '@generated_module/dtos/user-type-output-dto';

@Injectable()
/** 用户管理服务 */
export class UserManageService {
    constructor(private appApiService: AppApiService) { }

    /** 搜索用户 */
    Search(request: GridSearchRequestDto, resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/UserManageService/Search",
            {
                method: "POST",
                body: { request }
            }, resultConverter, errorConverter);
    }

    /** 测试基本数据类型 */
    Test(testid: string, resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<GridSearchResponseDto> {
        let urlParams = this.appApiService.getRequestQueryString([{ testid }]);
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/UserManageService/test",
            {
                method: "GET",
                params: urlParams
            }, resultConverter, errorConverter);
    }

    /** 测试空参数 */
    TestGet(resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/UserManageService/TestGet",
            {
                method: "GET"
            }, resultConverter, errorConverter);
    }

    /** 测试复杂对象 */
    TestObject(name: string, testInputDto: TestInput, resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<GridSearchResponseDto> {
        let urlParams = this.appApiService.getRequestQueryString([{ name }, { testInputDto }]);
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/UserManageService/TestObject",
            {
                method: "GET",
                params: urlParams
            }, resultConverter, errorConverter);
    }

    /** 编辑用户 */
    Edit(dto: UserInputDto, resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/UserManageService/customEdit",
            {
                method: "POST",
                body: { dto }
            }, resultConverter, errorConverter);
    }

    /** 删除用户 */
    Remove(id: string, resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/UserManageService/Remove",
            {
                method: "POST",
                body: { id }
            }, resultConverter, errorConverter);
    }

    /** 获取所有用户类型 */
    GetAllUserTypes(resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<UserTypeOutputDto[]> {
        return this.appApiService.call<UserTypeOutputDto[]>(
            "/api/UserManageService/GetAllUserTypes",
            {
                method: "GET"
            }, resultConverter, errorConverter);
    }
}
