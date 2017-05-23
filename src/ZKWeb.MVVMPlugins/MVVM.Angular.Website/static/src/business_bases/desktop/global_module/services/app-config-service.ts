import { Injectable } from '@angular/core';
import { AppStoreService } from './app-store-service';
import { AppConsts } from '@global_module/app-consts';
import { AppConfig } from '../models/app-config';

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
    /** 启用加密 */
    private _enableEncrypt: boolean;
    /**启用签名 */
    private _enableSignature: boolean;
    /**保存到本地 */
    private _saveToLocal: boolean;

    constructor(private store: AppStoreService) {
        let conf = store.getData<AppConfig>(AppConsts.AppConfigKey) || new AppConfig();
        this.initConfig(conf);
    }

    /** 初始应用配置 */
    initConfig(confifg: AppConfig) {
        this._apiUrlBase = confifg.apiUrlBase || (location.protocol + "//" + location.host);
        this._language = confifg.language || null;
        this._defaultLanguage = confifg.defaultLanguage || "zh-CN";
        this._languageHeader = confifg.languageHeader || "X-ZKWeb-Language";
        this._languageKey = confifg.languageKey || "ZKWeb-Language";
        this._timezone = confifg.timezone || null;
        this._defaultTimezone = confifg.defaultTimezone || "Asia/Shanghai";
        this._timezoneHeader = confifg.timezoneHeader || "X-ZKWeb-Timezone";
        this._timezoneKey = confifg.timezoneKey || "ZKWeb-Timezone";
        this._loginUrl = confifg.loginUrl || ["/admin", "login"];
        this._sessionIdHeader = confifg.sessionIdHeader || "X-ZKWeb-SessionId";
        this._sessionIdSetHeader = confifg.sessionIdSetHeader || "X-Set-ZKWeb-SessionId";
        this._sessionIdKey = confifg.sessionIdKey || "ZKWeb-SessionId";
        this._enableEncrypt = confifg.enableEncrypt || false;
        this._enableSignature = confifg.enableSignature || false;
        this._saveToLocal = confifg.saveToLocal || false;
    }

    /** 获取Api的基础地址 */
    get apiUrlBase(): string {
        return this._apiUrlBase;
    }

    /** 获取当前使用的语言 */
    get language(): string {
        if (!this._language) {
            this._language = this.store.getData<string>(this._languageKey) || this._defaultLanguage;
        }
        return this._language;
    }

    /** 设置当前使用的语言 */
    setLanguage(language: string) {
        this._language = language;
        this.store.saveData(this._languageKey, language);
    }

    // 获取客户端传给服务端使用的语言头
    get languageHeader(): string {
        return this._languageHeader;
    }

    // 获取当前使用的时区
    get timezone(): string {
        if (!this._timezone) {
            this._timezone = this.store.getData<string>(this._timezoneKey) || this._defaultTimezone;
        }
        return this._timezone;
    }

    /** 设置当前使用的时区 */
    setTimezone(timezone: string) {
        this._timezone = timezone;
        this.store.saveData(this._timezoneKey, timezone);
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

    get enableSignature(): boolean {
        return this._enableSignature;
    }
    // 获取当前会话Id
    get sessionId() {
        if (!this._sessionId) {
            this._sessionId = this.store.getData<string>(AppConsts.AccessToken);
            //localStorage.getItem(this._sessionIdKey);
        }
        return this._sessionId;
    }

    // 设置当前会话Id
    setSessionId(sessionId: string): void {
        this._sessionId = sessionId;
        //localStorage.setItem(this._sessionIdKey, sessionId);
        this.store.saveData(AppConsts.AccessToken, sessionId);
    }

    // 获取客户端传给服务端使用的会话Id头
    get sessionIdHeader(): string {
        return this._sessionIdHeader;
    }

    // 获取服务端传给客户端使用的会话Id头
    get sessionIdSetHeader(): string {
        return this._sessionIdSetHeader;
    }
    get saveToLocal(): boolean {
        return this._saveToLocal;
    }
}
