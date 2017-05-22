import { Injectable, Optional } from '@angular/core';
import { AppStore } from '../../../../dev/app-store';
import { AppConsts } from "@global_module/app-consts";

/** 获取会话信息的服务 */
@Injectable()
export class AppStoreService {
    protected store: Map<string, any> = new Map<string, any>();
    constructor( @Optional() private hmrStore: AppStore) {
    }
    getData(key: string) {
        return this.store.has(key) ?
            this.store.get(key) :
            localStorage.getItem(key);
    }

    setData(key: string, value: any) {
        this.store.set(key, value);
        localStorage.setItem(key, JSON.stringify(value));
    }

    has(key: string) {
        return this.store.has(key);
    }
}
