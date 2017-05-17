
var cryptoJS = require('crypto-js');
export class AESUtils {
    static pin = "99b3ad6e8dfjpe4c";
    static Encrypt(privateKey, pin, data) {

        let keyHash = cryptoJS.SHA256(cryptoJS.enc.Utf8.parse(privateKey));
        let key = cryptoJS.lib.WordArray.create(keyHash.words.slice(0, 32), 32);
        let pinHash = cryptoJS.SHA256(cryptoJS.enc.Utf8.parse(pin));
        let iv = cryptoJS.lib.WordArray.create(pinHash.words.slice(0, 16), 16);
        let cfg = { iv: iv, mode: cryptoJS.mode.CBC, padding: cryptoJS.pad.Pkcs7 };
        let encrypted = cryptoJS.AES.encrypt(data, key, cfg);
        return encrypted.ciphertext;
    }

    static decrypt(privateKey, pin, data) {
        let cipherBuffer = cryptoJS.enc.Base64.parse(data);

        let keyHash = cryptoJS.SHA256(cryptoJS.enc.Utf8.parse(privateKey));
        let key = cryptoJS.lib.WordArray.create(keyHash.words.slice(0, 32), 32);

        let pinHash = cryptoJS.SHA256(cryptoJS.enc.Utf8.parse(pin));
        let iv = cryptoJS.lib.WordArray.create(pinHash.words.slice(0, 16), 16);

        let cfg = { iv: iv, mode: cryptoJS.mode.CBC, padding: cryptoJS.pad.Pkcs7 };

        let paramsData = {
            ciphertext: cipherBuffer
        };

        return cryptoJS.AES.decrypt(paramsData, key, cfg);
    }

    static decryptToUtf8String(privateKey: string, data: string, pin?: string) {
        return this.decrypt(privateKey, pin ? pin : this.pin, data).toString(cryptoJS.enc.Utf8);
    }
    static EncryptToBase64String(privateKey: string, data: string, pin?: string) {
        return this.Encrypt(privateKey, pin ? pin : this.pin, data).toString(cryptoJS.enc.Base64);
    }
}