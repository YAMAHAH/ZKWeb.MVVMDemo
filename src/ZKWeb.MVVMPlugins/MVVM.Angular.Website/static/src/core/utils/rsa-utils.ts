
import { JSEncrypt } from 'jsencrypt';
//var CryptoJS : = require('crypto-js');
export class RSAUtils {
     //CryptoJS = CryptoJS.CryptoJSStatic = require("@vendor/scripts/crypto-js.js");

    /**
     * @param  {string} publickey 公钥
     * @param  {string} inputString 加密字符串
     * @return {string}  返回字符串
     */
    static RSAEncrypt(publickey: string, inputString: string): string {
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
    static RSADecrypt(privateKey: string, inputString: string): string {
        // Decrypt with the private key...
        let decrypt = new JSEncrypt();
        decrypt.setPrivateKey(privateKey);
        let uncrypted = decrypt.decrypt(inputString);
        return uncrypted;
    }

    /**
     * 
     * @param keySize 生成的位数（512，1024，2048，4096），位数越大，加密时间越长
     */
    static genRSAKey(keySize: number = 1024) {
        while (true) {
            var crypt = new JSEncrypt({ default_key_size: keySize || 1024 });
            crypt.getKey();
            var publicKey = crypt.getPublicKey()
                .replace("-----BEGIN PUBLIC KEY-----", "")
                .replace("-----END PUBLIC KEY-----", "")
                .replace("\n", "")
                .replace("\r", "");
            var wordArray = CryptoJS.enc.Base64.parse(publicKey);

            if (wordArray.sigBytes == 165) { //解析长度165，服务端API才能接受
                return {
                    privateKey: crypt.getPrivateKey(),
                    publicKey: crypt.getPublicKey()
                }
            }
        }
    }
}