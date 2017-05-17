import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { GridSearchResponseDto } from '../dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '../dtos/grid-search-request-dto';
import { TestInput } from '../dtos/test-input';
import { ActionResponseDto } from '../dtos/action-response-dto';
import { UserInputDto } from '../dtos/user-input-dto';
import { UserTypeOutputDto } from '../dtos/user-type-output-dto';

@Injectable()
/** 用户管理服务 */
export class UserManageService {
    constructor(private appApiService: AppApiService) { }

    /** 搜索用户 */
    Search(request: GridSearchRequestDto): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/UserManageService/Search",
            {
                method: "POST",
                body: { request }
            });
    }

    /** 测试基本数据类型 */
    Test(testid: string): Observable<GridSearchResponseDto> {
        let urlParams = this.appApiService.getRequestQueryString([{ testid }]);
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/UserManageService/test",
            {
                method: "GET",
                params: urlParams
            });
    }

    /** 测试空参数 */
    TestGet(): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/UserManageService/TestGet",
            {
                method: "GET"
            });
    }

    /** 测试复杂对象 */
    TestObject(name: string, testInputDto: TestInput): Observable<GridSearchResponseDto> {
        let urlParams = this.appApiService.getRequestQueryString([{ name }, { testInputDto }]);
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/UserManageService/TestObject",
            {
                method: "GET",
                params: urlParams
            });
    }

    /** 编辑用户 */
    Edit(dto: UserInputDto): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/UserManageService/customEdit",
            {
                method: "POST",
                body: { dto }
            });
    }

    /** 删除用户 */
    Remove(id: string): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/UserManageService/Remove",
            {
                method: "POST",
                body: { id }
            });
    }

    /** 获取所有用户类型 */
    GetAllUserTypes(): Observable<UserTypeOutputDto[]> {
        return this.appApiService.call<UserTypeOutputDto[]>(
            "/api/UserManageService/GetAllUserTypes",
            {
                method: "GET"
            });
    }
}
