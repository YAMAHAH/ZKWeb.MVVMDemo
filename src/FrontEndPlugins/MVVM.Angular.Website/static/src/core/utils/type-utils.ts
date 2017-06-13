
export const Type = Function;
export function isType(v: any): v is Type<any> {
    return typeof v === 'function';
}
export interface Type<T> extends Function { new (...args: any[]): T; }

/**
 * Check and return true if an object is type of string
 */
export function isString(obj: any) {
    return typeof obj === "string";
}

/**
 * Check and return true if an object is type of number
 */
export function isNumber(obj: any) {
    return typeof obj === "number";
}

export function isBool(obj: any) {
    return typeof obj === "boolean";
}

export function isBlank(obj: any) {
    return typeof obj === "undefined" || obj === null;
}

/**参数对象为数组返回TRUE,否则返回FALSE */
export function isArray(obj: any) {
    return Array.isArray(obj);
}

/**
 * Check and return true if an object is type of Function
 */
export function isFunction(obj: any) {
    //alternative for $.isFunction
    return typeof obj === "function" || !!(obj && obj.constructor && obj.call && obj.apply);
}


export function isObject(obj: any) {
    return typeof obj === "object";
}

export function isJson(jsonStr: string) {
    return /^[\],:{}\s]*$/.test(jsonStr.replace(/\\["\\\/bfnrtu]/g, '@')
        .replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']')
        .replace(/(?:^|:|,)(?:\s*\[)+/g, ''));
}