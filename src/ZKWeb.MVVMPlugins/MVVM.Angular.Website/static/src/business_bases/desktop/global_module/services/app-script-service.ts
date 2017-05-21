import { Injectable } from '@angular/core';

@Injectable()
export class AppScriptService {

    constructor() { }

    private loadlibs: Map<string, string> = new Map<string, string>();
    load(src: string): Promise<any> {
        if (!!!this.loadlibs.has(src)) {
            let doc = document;
            return new Promise((resolve, reject) => {
                let script = doc.createElement('script');
                if (!src) {
                    new Error('src');
                }
                script.src = src;
                doc.querySelector('head').appendChild(script);

                script.onload = () => {
                    resolve();
                    this.loadlibs.set(src, src);
                    script.remove()
                }
                script.onerror = function () {
                    reject()
                }
            });
        }
        return Promise.resolve(true);
    }
    loadCSS(src: string): Promise<any> {
        if (!!!this.loadlibs.has(src)) {
            let doc = document;
            return new Promise((resolve, reject) => {
                let link = doc.createElement('link');
                if (!src) {
                    new Error('src');
                }
                link.href = src;
                link.rel = "stylesheet";
                doc.querySelector('head').appendChild(link);

                link.onload = () => {
                    resolve();
                    this.loadlibs.set(src, src);
                }
                link.onerror = function () {
                    reject();
                }
            });
        }
        return Promise.resolve(true);
    }

}