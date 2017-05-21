import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '@global_module/services/app-api-service';
import { ApiCallExtra } from '@global_module/models/api-call-extra';
import { HandshakeRequestOutput } from '@generated_module/dtos/handshake-request-output';
import { HandshakeRequestInput } from '@generated_module/dtos/handshake-request-input';

@Injectable()
/** 验证码服务 */
export class CaptchaService {
    constructor(private appApiService: AppApiService) { }

    /** 获取验证码图片的Base64 */
    GetCaptchaImageBase64(key: string, extra?: ApiCallExtra): Observable<string> {
        return this.appApiService.call<string>(
            "/api/CaptchaService/GetCaptchaImageBase64",
            {
                method: "POST",
                body: { key }
            }, extra);
    }

    /** 客户端与服务端第一次连接握手请求 */
    HandshakeRequest(handshakeRequest: HandshakeRequestInput, extra?: ApiCallExtra): Observable<HandshakeRequestOutput> {
        return this.appApiService.call<HandshakeRequestOutput>(
            "/api/CaptchaService/HandshakeRequest",
            {
                method: "POST",
                body: { handshakeRequest }
            }, extra);
    }
}
