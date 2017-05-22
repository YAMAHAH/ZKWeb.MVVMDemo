using System;
using System.Security.Cryptography;
using System.Text;

namespace SimpleEasy.Core.lib.Utils
{
    public static class HMACUtils
    {
        public static Object Hash_HMAC(string signatureString, string secretKey, bool raw_output = false)
        {
            var enc = Encoding.UTF8;
            HMACSHA1 hmac = new HMACSHA1(enc.GetBytes(secretKey));

            hmac.Initialize();

            byte[] buffer = enc.GetBytes(signatureString);
            if (raw_output)
            {
                // return Convert.ToBase64String(hmac.ComputeHash(buffer)).Trim();
                return hmac.ComputeHash(buffer);
            }
            else
            {
                return Convert.ToBase64String(hmac.ComputeHash(buffer)).Trim();
                // return BitConverter.ToString(hmac.ComputeHash(buffer)).Replace("-", "").ToLower().Trim();
            }
        }

        public static string HMACSha256ToBase64(string signatureString, string secretKey)
        {
            var enc = Encoding.UTF8;
            HMACSHA256 hmacSha256 = new HMACSHA256(enc.GetBytes(secretKey));
            hmacSha256.Initialize();
            byte[] buffer = enc.GetBytes(signatureString);
            return Convert.ToBase64String(hmacSha256.ComputeHash(buffer)).Trim();
        }
        public static byte[] HMACSha256ToByte(string signatureString, string secretKey)
        {
            var enc = Encoding.UTF8;
            HMACSHA256 hmacSha256 = new HMACSHA256(enc.GetBytes(secretKey));
            hmacSha256.Initialize();
            byte[] buffer = enc.GetBytes(signatureString);
            return hmacSha256.ComputeHash(buffer);
        }

        public static string HMACSha1ToBase64(string signatureString, string secretKey)
        {
            var enc = Encoding.UTF8;
            HMACSHA1 hmacSha1 = new HMACSHA1(enc.GetBytes(secretKey));
            hmacSha1.Initialize();
            byte[] buffer = enc.GetBytes(signatureString);
            return Convert.ToBase64String(hmacSha1.ComputeHash(buffer)).Trim();
        }

        public static byte[] HMACSha1ToByte(string signatureString, string secretKey)
        {
            var enc = Encoding.UTF8;
            HMACSHA1 hmacSha1 = new HMACSHA1(enc.GetBytes(secretKey));
            hmacSha1.Initialize();
            byte[] buffer = enc.GetBytes(signatureString);
            return hmacSha1.ComputeHash(buffer);
        }
    }
}
