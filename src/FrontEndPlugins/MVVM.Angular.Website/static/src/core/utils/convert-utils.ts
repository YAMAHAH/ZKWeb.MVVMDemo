
export class ConvertUtils {

    public static toBool(obj) {
        return !!obj;
    }

    /**
     * 转换成数字
    obj:any     */
    public static toNumber(obj: any) {
        let value = parseInt(obj);
        return isNaN(value) ? 0 : + value;
    }

}