import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { ApiCallExtra } from '@global_module/models/api-call-extra';
import { ActionResponseDto } from '@generated_module/dtos/action-response-dto';
import { WebsiteInfoOutputDto } from '@generated_module/dtos/website-info-output-dto';
import { WebsiteSettingsDto } from '@generated_module/dtos/website-settings-dto';
import { GridSearchResponseDto } from '@generated_module/dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '@generated_module/dtos/grid-search-request-dto';

@Injectable()
/** 网站管理服务 */
export class WebsiteManageService {
    constructor(private appApiService: AppApiService) { }

    /** 清理缓存 */
    ClearCache(extra?: ApiCallExtra): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/WebsiteManageService/ClearCache",
            {
                method: "POST"
            }, extra);
    }

    /** 获取网站信息 */
    GetWebsiteInfo(extra?: ApiCallExtra): Observable<WebsiteInfoOutputDto> {
        return this.appApiService.call<WebsiteInfoOutputDto>(
            "/api/WebsiteManageService/GetWebsiteInfo",
            {
                method: "POST"
            }, extra);
    }

    /** 获取网站信息 */
    GetWebsiteSettings(extra?: ApiCallExtra): Observable<WebsiteSettingsDto> {
        return this.appApiService.call<WebsiteSettingsDto>(
            "/api/WebsiteManageService/GetWebsiteSettings",
            {
                method: "POST"
            }, extra);
    }

    /** 保存网站信息 */
    SaveWebsiteSettings(dto: WebsiteSettingsDto, extra?: ApiCallExtra): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/WebsiteManageService/SaveWebsiteSettings",
            {
                method: "POST",
                body: { dto }
            }, extra);
    }

    /** 搜索定时任务 */
    SearchScheduledTasks(request: GridSearchRequestDto, extra?: ApiCallExtra): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/WebsiteManageService/SearchScheduledTasks",
            {
                method: "POST",
                body: { request }
            }, extra);
    }

    /** 搜索定时任务记录 */
    SearchScheduledTaskLogs(request: GridSearchRequestDto, extra?: ApiCallExtra): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/WebsiteManageService/SearchScheduledTaskLogs",
            {
                method: "POST",
                body: { request }
            }, extra);
    }
}
