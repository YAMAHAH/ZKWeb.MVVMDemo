using Microsoft.IdentityModel.Tokens;
using System;

namespace ZKWeb.MVVMDemo.AspNetCore.TokenManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenAuthOption
    {
        /// <summary>
        /// 
        /// </summary>
        public static string Audience { get; } = "ExampleAudience";
        /// <summary>
        /// 
        /// </summary>
        public static string Issuer { get; } = "ExampleIssuer";
        /// <summary>
        /// 
        /// </summary>
        public static RsaSecurityKey Key { get; } = new RsaSecurityKey(RSAKeyHelper.GenerateKey());
        /// <summary>
        /// 
        /// </summary>
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);

        /// <summary>
        /// 
        /// </summary>
        public static string TokenType { get; } = "Bearer";
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// 
        /// </summary>
        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromMinutes(20);
    }
}
