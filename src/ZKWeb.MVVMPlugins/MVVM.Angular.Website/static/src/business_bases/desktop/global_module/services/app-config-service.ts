import { Injectable } from '@angular/core';
import { AppStoreService } from './app-store-service';

/** 保存全局配置的服务 */
@Injectable()
export class AppConfigService {
    /** Api的基础地址 */
    private _apiUrlBase: string;
    /** 当前使用的语言，例如"en-US" */
    private _language: string;
    /** 默认使用的语言 */
    private _defaultLanguage: string;
    /** 客户端传给服务端使用的语言头 */
    private _languageHeader: string;
    /** 当前使用的语言在本地储存的key */
    private _languageKey: string;
    /** 当前使用的时区，例如"America/Los_Angeles" */
    private _timezone: string;
    /** 默认使用的时区 */
    private _defaultTimezone: string;
    /** 客户端传给服务端使用的语言头 */
    private _timezoneHeader: string;
    /** 当前使用的时区在本地储存的key */
    private _timezoneKey: string;
    /** 登录地址 */
    private _loginUrl: string[];
    /** 当前会话Id */
    private _sessionId: string;
    /** 客户端传给服务端使用的会话Id头 */
    private _sessionIdHeader: string;
    /** 服务端传给客户端使用的会话Id头 */
    private _sessionIdSetHeader: string;
    /** 会话Id储存在本地储存的key */
    private _sessionIdKey: string;

    private _enableEncrypt: boolean;
    private _enableSignature:boolean;

    constructor(private store: AppStoreService) {
        let localConfig = localStorage.getItem('appConfig');
        let conf = localConfig ? JSON.parse(localConfig) : null || store.getData('appConfig') || window['appConfig'];
        this.initConfig(conf);
    }

    /** 初始应用配置 */
    initConfig(config) {
        let conf = config || {};
        this._apiUrlBase = conf.apiUrlBase || (location.protocol + "//" + location.host);
        this._language = conf.language || null;
        this._defaultLanguage = conf.defaultLnaguage || "zh-CN";
        this._languageHeader = conf.languageHeader || "X-ZKWeb-Language";
        this._languageKey = conf.languageKey || "ZKWeb-Language";
        this._timezone = conf.timezone || null;
        this._defaultTimezone = conf.defaultTimezone || "Asia/Shanghai";
        this._timezoneHeader = conf.timezoneHeader || "X-ZKWeb-Timezone";
        this._timezoneKey = conf.timezoneKey || "ZKWeb-Timezone";
        this._loginUrl = conf.loginUrl || ["/admin", "login"];
        this._sessionIdHeader = conf.sessionIdHeader || "X-ZKWeb-SessionId";
        this._sessionIdSetHeader = conf.sessionIdSetHeader || "X-Set-ZKWeb-SessionId";
        this._sessionIdKey = conf.sessionIdKey || "ZKWeb-SessionId";
        this._enableEncrypt = conf.enableEncrypt || false;
        this._enableSignature = conf.enableSignature || false;
    }

    /** 获取Api的基础地址 */
    get apiUrlBase(): string {
        return this._apiUrlBase;
    }

    /** 获取当前使用的语言 */
    get language(): string {
        if (!this._language) {
            this._language = localStorage.getItem(this._languageKey) || this._defaultLanguage;
        }
        return this._language;
    }

    /** 设置当前使用的语言 */
    setLanguage(language: string) {
        this._language = language;
        localStorage.setItem(this._languageKey, language);
    }

    // 获取客户端传给服务端使用的语言头
    get languageHeader(): string {
        return this._languageHeader;
    }

    // 获取当前使用的时区
    get timezone(): string {
        if (!this._timezone) {
            this._timezone = localStorage.getItem(this._timezone) || this._defaultTimezone;
        }
        return this._timezone;
    }

    /** 设置当前使用的时区 */
    setTimezone(timezone: string) {
        this._timezone = timezone;
        localStorage.setItem(this._timezoneKey, timezone);
    }

    // 获取客户端传给服务端使用的语言头
    get timezoneHeader(): string {
        return this._timezoneHeader;
    }

    // 获取登录地址
    get loginUrl() {
        return this._loginUrl;
    }

    get enableEncrypt(): boolean {
        return this._enableEncrypt;
    }

    get enableSignature():boolean{
        return this._enableSignature;
    }
    // 获取当前会话Id
    get sessionId() {
        if (!this._sessionId) {
            this._sessionId = localStorage.getItem(this._sessionIdKey);
        }
        return this._sessionId;
    }

    // 设置当前会话Id
    setSessionId(sessionId: string): void {
        this._sessionId = sessionId;
        localStorage.setItem(this._sessionIdKey, sessionId);
    }

    // 获取客户端传给服务端使用的会话Id头
    get sessionIdHeader(): string {
        return this._sessionIdHeader;
    }

    // 获取服务端传给客户端使用的会话Id头
    get sessionIdSetHeader(): string {
        return this._sessionIdSetHeader;
    }
}
