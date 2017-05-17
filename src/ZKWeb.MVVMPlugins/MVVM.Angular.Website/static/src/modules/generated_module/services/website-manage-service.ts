import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { ActionResponseDto } from '../dtos/action-response-dto';
import { WebsiteInfoOutputDto } from '../dtos/website-info-output-dto';
import { WebsiteSettingsDto } from '../dtos/website-settings-dto';
import { GridSearchResponseDto } from '../dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '../dtos/grid-search-request-dto';

@Injectable()
/** 网站管理服务 */
export class WebsiteManageService {
    constructor(private appApiService: AppApiService) { }

    /** 清理缓存 */
    ClearCache(): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/WebsiteManageService/ClearCache",
            {
                method: "POST"
            });
    }

    /** 获取网站信息 */
    GetWebsiteInfo(): Observable<WebsiteInfoOutputDto> {
        return this.appApiService.call<WebsiteInfoOutputDto>(
            "/api/WebsiteManageService/GetWebsiteInfo",
            {
                method: "POST"
            });
    }

    /** 获取网站信息 */
    GetWebsiteSettings(): Observable<WebsiteSettingsDto> {
        return this.appApiService.call<WebsiteSettingsDto>(
            "/api/WebsiteManageService/GetWebsiteSettings",
            {
                method: "POST"
            });
    }

    /** 保存网站信息 */
    SaveWebsiteSettings(dto: WebsiteSettingsDto): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/WebsiteManageService/SaveWebsiteSettings",
            {
                method: "POST",
                body: { dto }
            });
    }

    /** 搜索定时任务 */
    SearchScheduledTasks(request: GridSearchRequestDto): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/WebsiteManageService/SearchScheduledTasks",
            {
                method: "POST",
                body: { request }
            });
    }

    /** 搜索定时任务记录 */
    SearchScheduledTaskLogs(request: GridSearchRequestDto): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/WebsiteManageService/SearchScheduledTaskLogs",
            {
                method: "POST",
                body: { request }
            });
    }
}
