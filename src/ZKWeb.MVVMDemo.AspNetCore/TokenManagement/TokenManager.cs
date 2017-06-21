using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace ZKWeb.MVVMDemo.AspNetCore.TokenManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenManager
    {
        
        //public static string CreateToken(User user, DateTime expires)
        //{
        //    var handler = new JwtSecurityTokenHandler();
        //    //生成会话ID
        //    ClaimsIdentity identity = new ClaimsIdentity(
        //        new GenericIdentity(user.Username, "TokenAuth"),
        //        new[] {
        //            new Claim("ID", user.ID.ToString()),
        //            new Claim("SessionId",Guid.NewGuid().ToString()),
        //            new Claim("tenantId",Guid.NewGuid().ToString())
        //        }
        //    );

        //    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
        //    {
        //        Issuer = TokenAuthOption.Issuer,
        //        Audience = TokenAuthOption.Audience,
        //        SigningCredentials = TokenAuthOption.SigningCredentials,
        //        Subject = identity,
        //        Expires = expires
        //    });
        //    return handler.WriteToken(securityToken);
        //}

        /// <summary>
        /// Validate Token
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        public static ClaimsPrincipal ValidateToken(string tokenString)
        {

            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = TokenAuthOption.Key,
                ValidateAudience = true,
                ValidAudience = TokenAuthOption.Audience,
                ValidateIssuer = true,
                ValidIssuer = TokenAuthOption.Issuer,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(0)
            };

            SecurityToken token = new JwtSecurityToken();
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(tokenString, validationParameters, out token);

                return principal;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
            }

            return null;

        }

        /// <summary>
        /// Create Claim
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value);
        }

        /// <summary>
        /// Get Bytes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;

        }

        /// <summary>
        /// Get UserID
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        public static string GetUserID(string tokenString)
        {
            ClaimsPrincipal principal = TokenManager.ValidateToken(tokenString);
            var userId = principal.Claims.FirstOrDefault(c => c.Type == "ID")?.Value;
            return userId;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        public static string GetSessionID(string tokenString)
        {
            ClaimsPrincipal principal = TokenManager.ValidateToken(tokenString);
            var sessionId = principal.Claims.FirstOrDefault(c => c.Type == "SessionId")?.Value ;
            return sessionId;
        }
    }
}
