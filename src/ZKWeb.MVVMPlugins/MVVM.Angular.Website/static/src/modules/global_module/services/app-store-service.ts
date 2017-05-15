import { Injectable } from '@angular/core';

/** 获取会话信息的服务 */
@Injectable()
export class AppStoreService {
    store: Map<string, any>;
    constructor() {
        this.store = new Map<string, any>();
    }
    getData(key: string) {
        return this.store.has(key) && this.store.get(key);
    }

    setData(key: string, value: any) {
        this.store.set(key, value);
    }

    has(key: string) {
        return this.store.has(key);
    }
}
