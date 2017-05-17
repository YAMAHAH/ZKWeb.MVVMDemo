
import { JSEncrypt } from 'jsencrypt';

export class RSAUtils {
    /**
     * @param  {string} publickey 公钥
     * @param  {string} inputString 加密字符串
     * @return {string}  返回字符串
     */
    static RsaEncrypt(publickey: string, inputString: string): string {
        var encrypt = new JSEncrypt();
        encrypt.setPublicKey(publickey);
        var encrypted = encrypt.encrypt(inputString);
        return encrypted;
    }

    /**
     * 
     * @param {string} privateKey  私钥
     * @param {string} inputString 解密字符串
     * @return {string} 字符串
     */
    static RsaDecrypt(privateKey: string, inputString: string): string {
        // Decrypt with the private key...
        let decrypt = new JSEncrypt();
        decrypt.setPrivateKey(privateKey);
        let uncrypted = decrypt.decrypt(inputString);
        return uncrypted;
    }
}