import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { rxResultConverter, rxErrorConverter } from '@core/utils/type-utils';

@Injectable()
/** 验证码服务 */
export class CaptchaService {
    constructor(private appApiService: AppApiService) { }

    /** 获取验证码图片的Base64 */
    GetCaptchaImageBase64(key: string, resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter): Observable<string> {
        return this.appApiService.call<string>(
            "/api/CaptchaService/GetCaptchaImageBase64",
            {
                method: "POST",
                body: { key }
            }, resultConverter, errorConverter);
    }
}
