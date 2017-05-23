export class AppConfig {
    /** Api的基础地址 */
    apiUrlBase?: string = (location.protocol + "//" + location.host);

    /** 当前使用的语言，例如"en-US" */
    language?: string = null;

    /** 默认使用的语言 */
    defaultLanguage?: string = "zh-CN";

    /** 客户端传给服务端使用的语言头 */
    languageHeader?: string = "X-ZKWeb-Language";

    /** 当前使用的语言在本地储存的key */
    languageKey?: string = "ZKWeb-Language";

    /** 当前使用的时区，例如"America/Los_Angeles" */
    timezone?: string = null;

    /** 默认使用的时区 */
    defaultTimezone?: string = "Asia/Shanghai";

    /** 客户端传给服务端使用的语言头 */
    timezoneHeader?: string = "X-ZKWeb-Timezone";

    /** 当前使用的时区在本地储存的key */
    timezoneKey?: string = "ZKWeb-Timezone";

    /** 登录地址 */
    loginUrl = ["/admin", "login"];

    /** 客户端传给服务端使用的会话Id头 */
    sessionIdHeader?: string = "X-ZKWeb-SessionId";

    /** 服务端传给客户端使用的会话Id头 */
    sessionIdSetHeader?: string = "X-Set-ZKWeb-SessionId";

    /** 会话Id储存在本地储存的key */
    sessionIdKey?: string = "ZKWeb-SessionId";

    /**启用加密 */
    enableEncrypt?: boolean = false;

    /**启用签名 */
    enableSignature?: boolean = false;

    /**保存到本地 */
    saveToLocal?: boolean = false;
}