using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEasy.Core.lib.Utils
{
    public class AESUtils
    {
        /// <summary>
        /// ASE加密
        /// </summary>
        /// <param name="privateKey">密钥</param>
        /// <param name="pin">密钥向量</param>
        /// <param name="data">加密数据</param>
        /// <returns>加密字节数组,转BASE64字符串</returns>
        public async static Task<byte[]> Encrypt(string privateKey, string pin, byte[] data)
        {

            using (var sha = SHA256.Create())
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] keyHash = sha.ComputeHash(Encoding.UTF8.GetBytes($"{privateKey}"));
                    byte[] key = new byte[aes.Key.Length];
                    Buffer.BlockCopy(keyHash, 0, key, 0, aes.Key.Length);

                    byte[] pinHash = sha.ComputeHash(Encoding.UTF8.GetBytes($"{pin}"));
                    byte[] iv = new byte[aes.IV.Length];
                    Buffer.BlockCopy(pinHash, 0, iv, 0, aes.IV.Length);

                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var stream = new MemoryStream())
                    using (ICryptoTransform transform = aes.CreateEncryptor(key, iv))
                    {
                        using (var cryptStream = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                        {
                            await cryptStream.WriteAsync(data, 0, data.Length);
                        }
                        return stream.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// ASE加密
        /// </summary>
        /// <param name="privateKey">密钥</param>
        /// <param name="pin">密钥向量</param>
        /// <param name="data">加密数据字符串</param>
        /// <returns>BASE64字符串</returns>
        public static string EncryptToBase64String(string privateKey, string pin, string data)
        {
            return Convert.ToBase64String(Encrypt(privateKey, pin, Encoding.UTF8.GetBytes(data)).Result);
        }
        /// <summary>
        /// ASE加密
        /// </summary>
        /// <param name="privateKey">密钥</param>
        /// <param name="pin">密钥向量</param>
        /// <param name="data">加密数据对象</param>
        /// <returns>BASE64字符串</returns>
        public static string EncryptToBase64String(string privateKey, string pin, object data)
        {
            return Convert.ToBase64String(Encrypt(privateKey, pin, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data))).Result);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="privateKey">密钥</param>
        /// <param name="pin">密钥向量</param>
        /// <param name="data">加密BASE64字符串</param>
        /// <returns>解密字节数组，转UTF8字符串</returns>
        public async static Task<byte[]> Decrypt(string privateKey, string pin, string data)
        {
            var cipherBuffer = Convert.FromBase64String(data);
            byte[] decryptBytes = new byte[cipherBuffer.Length];
            using (var sha = SHA256.Create())
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] keyHash = sha.ComputeHash(Encoding.UTF8.GetBytes($"{privateKey}"));
                    byte[] key = new byte[32];
                    Buffer.BlockCopy(keyHash, 0, key, 0, 32);

                    byte[] pinHash = sha.ComputeHash(Encoding.UTF8.GetBytes($"{pin}"));
                    byte[] iv = new byte[16];
                    Buffer.BlockCopy(pinHash, 0, iv, 0, 16);

                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    // Trace.WriteLine($"Key length: { key.Length }, iv length: { iv.Length }, block mode: { aes.Mode }, padding: { aes.Padding }");
                    using (var stream = new MemoryStream(cipherBuffer))
                    using (ICryptoTransform transform = aes.CreateDecryptor(key, iv))
                    {
                        using (var cryptStream = new CryptoStream(stream, transform, CryptoStreamMode.Read))
                        {
                            await cryptStream.ReadAsync(decryptBytes, 0, decryptBytes.Length);
                        }
                        return decryptBytes;
                    }
                }
            }
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="privateKey">密钥</param>
        /// <param name="pin">密钥向量</param>
        /// <param name="data"加密BASE64字符串></param>
        /// <returns>UTF8字符串</returns>
        public async static Task<string> DecryptToUtf8String(string privateKey, string pin, string data)
        {
            return await Task.FromResult<string>(Encoding.UTF8.GetString(Decrypt(privateKey, pin, data).Result).TrimEnd('\0')); //Replace("\0", "")
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="privateKey">密钥</param>
        /// <param name="pin">密钥向量</param>
        /// <param name="data"加密BASE64字符串></param>
        /// <returns>反序列化对象</returns>
        public async static Task<T> DecryptToModel<T>(string privateKey, string pin, string data)
        {
            return await Task.FromResult<T>(JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(Decrypt(privateKey, pin, data).Result).TrimEnd('\0'))); //Replace("\0", "")
        }
    }
}
