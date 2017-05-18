using Microsoft.IdentityModel.Tokens;
using System;

namespace ZKWeb.MVVMDemo.AspNetCore.TokenManagement
{
    public class TokenAuthOption
    {
        public static string Audience { get; } = "ExampleAudience";
        public static string Issuer { get; } = "ExampleIssuer";
        public static RsaSecurityKey Key { get; } = new RsaSecurityKey(RSAKeyHelper.GenerateKey());
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);

        public static string TokenType { get; } = "Bearer";
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(5);

        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromMinutes(20);
    }
}
