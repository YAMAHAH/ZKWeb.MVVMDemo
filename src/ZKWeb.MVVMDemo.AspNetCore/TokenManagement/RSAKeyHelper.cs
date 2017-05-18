using System.Security.Cryptography;

namespace ZKWeb.MVVMDemo.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class RSAKeyHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static RSAParameters GenerateKey()
        {
            using (var key = new RSACryptoServiceProvider(2048))
            {
                return key.ExportParameters(true);
            }
        }
    }

}
