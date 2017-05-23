import { Injectable, Optional } from '@angular/core';
import { AppConsts } from '@global_module/app-consts';
import { isJson } from '@core/utils/type-utils';
import { isString } from '@core/utils/type-utils';

/** 获取会话信息的服务 */
@Injectable()
export class AppStoreService {
    protected store: Map<string, any> = new Map<string, any>();
    constructor() {
    }
    getData(key: string) {
        let value = this.store.has(key) ? this.store.get(key) : null;
        if (!!!value) {
            value = localStorage.getItem(key);
            if (value && isString(value) && isJson(value)) {
                value = JSON.parse(value);
            }
        }
        if (!!!value) {
            value = window[key];
        }
        return value;
    }

    setData(key: string, value: any, saveToLocal: boolean = false) {
        this.store.set(key, value);
        if (saveToLocal) {
            if (typeof value == "object") {
                localStorage.setItem(key, JSON.stringify(value));
            } else {
                localStorage.setItem(key, value)
            }
        }
    }
    saveData(key: string, value: any) {
        let conf = this.getData(AppConsts.AppConfigKey);
        console.log(conf);
        this.store.set(key, value);
        if (conf && conf.saveToLocal) {
            if (typeof value == "object") {
                localStorage.setItem(key, JSON.stringify(value));
            } else {
                localStorage.setItem(key, value)
            }
        }
    }
    has(key: string) {
        return this.store.has(key);
    }
}
