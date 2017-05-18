using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace ZKWeb.MVVMDemo.AspNetCore.TokenManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class PrivateManager
    {
        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<string, PrivateData> PrivateKeys = new Dictionary<string, PrivateData>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static PrivateData GetData(string key)
        {
            if (PrivateKeys.ContainsKey(key))
            {
                return PrivateKeys[key];
            }
            return new PrivateData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="privateData"></param>
        public static void PushData(string key, PrivateData privateData)
        {
            if (key == null || privateData == null)
            {
                return;
            }
            PrivateKeys[key] = privateData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetJwtTokenPlain(string key)
        {
            if (PrivateKeys.ContainsKey(key))
            {
                return PrivateKeys[key].JwtToken;
            }
            return string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetPublicKey(string key)
        {
            if (PrivateKeys.ContainsKey(key))
            {
                return PrivateKeys[key].PublicKey;
            }
            return string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetPublicKey(Func<PrivateData,bool> key)
        {
            var aa = PrivateKeys.Values.Where(key);
            return PrivateKeys.Values.Where(key).SingleOrDefault()?.PublicKey;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PrivateData
    {
        /// <summary>
        /// 
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string JwtToken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PrivateKey { get; set; }
    }
}
