import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptionsArgs, URLSearchParams } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import * as Rx from 'rxjs/rx';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/publishLast';
import { AppConfigService } from './app-config-service';
import { AppConsts } from '@business_bases/desktop/global_module/app-consts';
import { AESUtils } from '@core/utils/aes-utils';
import { GuidUtils } from '@core/utils/guid-utils';
import { ApiCallExtra } from "@business_bases/desktop/global_module/models/api-call-extra";
import { EncryptOutput } from "@business_bases/desktop/global_module/models/encrypt-output";
import { AppStoreService } from './app-store-service';
import { ClientDataModel } from '../models/client-data-model';
import { isJson } from '@core/utils/type-utils';

// 调用远程Api的服务
@Injectable()
export class AppApiService {
    // 全局Url过滤器
    private urlFilters: ((url: string) => string)[] = [];
    // 全局选项过滤器
    private optionsFilters: ((options: RequestOptionsArgs) => RequestOptionsArgs)[] = [];
    // 全局内容过滤器
    private bodyFilters: ((body: any) => any)[] = [];
    // 全局结果过滤器
    private resultFilters: ((response: Response) => Response)[] = [];
    // 全局错误过滤器
    private errorFilters: ((error: any) => any)[] = [];
    // 默认结果转换器
    private resultConverter: (response: Response) => any;
    // 默认错误转换器
    private errorConverter: (error: any) => Observable<any>;
    cryptojs: CryptoJS.CryptoJSStatic = require("@vendor/scripts/crypto-js.js");

    initBodyFilter() {
        // 如果内容包含文件对象则转换为FormData
        this.registerBodyFilter(body => {
            let formData = new FormData();
            // 设置最外层的参数
            for (let key in body) {
                if (body.hasOwnProperty(key)) {
                    formData.append(key, JSON.stringify(body[key]));
                }
            }
            // 枚举里层检测是否有文件对象
            let fileCount = 0;
            let visitor = (obj) => {
                for (let key in obj) {
                    if (obj.hasOwnProperty(key)) {
                        let value = obj[key];
                        if (value instanceof File) {
                            // 名称用原来的key，请注意重复
                            formData.append(key, value);
                            fileCount += 1;
                        } else if (value instanceof Object && value !== obj) {
                            // 检测子对象
                            visitor(value);
                        }
                    }
                }
            };
            visitor(body);
            // 有文件对象时返回FormData，否则返回原来的Body
            return (fileCount > 0) ? formData : body;
        });
    }

    constructor(
        protected http: Http,
        protected appConfigService: AppConfigService,
        protected appStoreService: AppStoreService
    ) {
        // 设置http头
        this.registerOptionsFilter(options => {
            // 让服务端把请求当作ajax请求
            options.headers.append("X-Requested-With", "XMLHttpRequest");
            // 设置当前语言
            options.headers.append(this.appConfigService.languageHeader, this.appConfigService.language);
            // 设置当前时区
            options.headers.append(this.appConfigService.timezoneHeader, this.appConfigService.timezone);
            // 附上会话Id
            options.headers.append(this.appConfigService.sessionIdHeader, this.appConfigService.sessionId);
            return options;
        });
        this.initBodyFilter();
        // 过滤回应
        this.registerResultFilter(response => {
            // 解析返回的会话Id
            let newSessionId = response.headers.get(this.appConfigService.sessionIdSetHeader);
            if (newSessionId) {
                this.appConfigService.setSessionId(newSessionId);
            }
            return response;
        });
        // 设置默认的结果转换器
        this.setResultConverter(response => {
            try {
                // 尝试使用json解析
                return response.json();
            } catch (e) {
                // 失败时返回字符串
                return response.text();
            }
        });
        // 设置默认的错误转换器
        this.setErrorConverter(error => {
            console.error("api request error:", error);
            let errorMessage: string;
            if (error instanceof Response) {
                if (error.status === 0) {
                    // 网络错误时显示特殊信息
                    errorMessage = "Network error, please check your internet connection";
                } else {
                    // 返回过滤html标签后的文本
                    errorMessage = error.text().replace(/<[^>]+>/g, "");
                }
            } else {
                // 返回错误对象的json
                errorMessage = JSON.stringify(error);
            }
            return new Observable(o => {
                o.error(errorMessage);
                o.complete();
            });
        });
    }

    // 注册全局Url过滤器
    registerUrlFilter(filter: (string) => string) {
        this.urlFilters.push(filter);
    }

    // 注册全局选项过滤器
    registerOptionsFilter(filter: (options: RequestOptionsArgs) => RequestOptionsArgs) {
        this.optionsFilters.push(filter);
    }

    // 注册全局内容过滤器
    registerBodyFilter(filter: (body: any) => any) {
        this.bodyFilters.push(filter);
    }

    // 注册全局结果过滤器
    registerResultFilter(filter: (response: Response) => Response) {
        this.resultFilters.push(filter);
    }

    // 注册全局错误过滤器
    registerErrorFilter(filter: (error: any) => any) {
        this.errorFilters.push(filter);
    }

    // 设置默认结果转换器
    setResultConverter(converter: (response: Response) => any) {
        this.resultConverter = converter;
    }

    // 获取默认结果转换器
    getResultConverter(): (response: Response) => any {
        return this.resultConverter;
    }

    // 设置默认错误转换器
    setErrorConverter(converter: (error: any) => any) {
        this.errorConverter = converter;
    }

    // 获取默认错误转换器
    getErrorConverter(): (error: any) => any {
        return this.errorConverter;
    }

    //对象转换成querystring

    getRequestQueryString(requestObjects: any[]) {
        let urlSearchParams = new URLSearchParams();
        requestObjects.forEach(requestObject => {
            for (var key in requestObject) {
                if (requestObject.hasOwnProperty(key)) {
                    let value = requestObject[key];
                    if (Array.isArray(value)) {
                        continue;
                    }
                    if (value instanceof Object) {
                        urlSearchParams.appendAll(this.getRequestQueryString([value]));
                    } else {
                        urlSearchParams.set(key, value);
                    }
                }
            }
        });
        return urlSearchParams;
    }
    clientData: ClientDataModel;
    // 调用Api函数
    call<T>(url: string, options?: RequestOptionsArgs, extra?: ApiCallExtra): Observable<T> {
        // 构建完整url，可能不在同一个host
        let fullUrl = this.appConfigService.apiUrlBase + url;
        this.urlFilters.forEach(h => { fullUrl = h(fullUrl); });
        // 构建选项，包括http头等
        options = options || { method: "POST" };
        options.headers = options.headers || new Headers();
        this.optionsFilters.forEach(h => { options = h(options); });
        let body = options.body;
        // 构建提交内容
        this.bodyFilters.forEach(h => { body = h(body); });
        if (!!!this.clientData) this.clientData = this.appStoreService.getData<ClientDataModel>(AppConsts.ClientDataKey);
        let enableEncrypt = this.appConfigService.enableEncrypt;
        //typeof 返回的是字符串,有六种可能:"number" "String" "boolean" "object" "function" "undefined"
        if (extra && typeof (extra.enableEncrypt) != 'undefined') enableEncrypt = extra.enableEncrypt;
        if (enableEncrypt) { // 分析请求和服务端IP地址，如果是同一网端则不加密数据,否则加密
            let chiperText = AESUtils.EncryptToBase64String(this.clientData.SecretKey, JSON.stringify(options.body));
            body = { requestId: GuidUtils.uuid(16, 10), data: chiperText, encrypt: true, signature: "" };
        }
        //默认全局选项决定是否启用签名，如果有传递enableSignature选项，则以传递的选项为准
        let enableSign = this.appConfigService.enableSignature;
        if (extra && typeof (extra.enableSignature) != 'undefined') enableSign = extra.enableSignature;
        if (enableSign) {
            let signStr = enableEncrypt ? body.data + "\n" : JSON.stringify(options.body);
            let sign = this.cryptojs.HmacSHA256(signStr, this.clientData.SecretKey).toString(this.cryptojs.enc.Base64);
            body.signature = sign;
        }
        options.body = body;
        return this.http
            .request(fullUrl, options) // 提交api
            .publishLast().refCount() // 防止多次subscribe导致多次提交
            .map(response => {
                //解密返回的数据
                //转换成对象，如果是加密接口，则解密
                let res = response;
                let resData = res[AppConsts.responseBodyKey] as string;
                if (isJson(resData)) {
                    let bodyObj = res.json() as EncryptOutput;
                    //计算签名
                    if (bodyObj && bodyObj.hasOwnProperty("signature") && bodyObj['signature']) {
                        let srvSign = bodyObj.signature;
                        let clientSign = this.cryptojs.HmacSHA256(bodyObj.data, this.clientData.SecretKey).toString(this.cryptojs.enc.Base64);
                        if (srvSign === clientSign) {
                            //签名验证通过
                        } else {
                            //抛出错误
                            return Observable.throw("客户端与服务端的签名不一致");
                        }
                    }
                    //解密
                    if (bodyObj && bodyObj.hasOwnProperty("data") && bodyObj['data']) {
                        response[AppConsts.responseBodyKey] = AESUtils.decryptToUtf8String(this.clientData.SecretKey, bodyObj.data);
                    }
                }
                // 过滤返回的结果
                this.resultFilters.forEach(f => { response = f(response); });
                // 转换返回的结果
                return (extra && extra.resultConverter || this.resultConverter)(response);
            })
            .catch(error => {
                // 过滤返回的错误
                this.errorFilters.forEach(f => { error = f(error); });
                // 转换返回的错误
                return (extra && extra.errorConverter || this.errorConverter)(error);
            });
    }
}
