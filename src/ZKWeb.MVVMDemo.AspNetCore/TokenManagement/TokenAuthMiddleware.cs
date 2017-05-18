using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ZKWeb.MVVMDemo.AspNetCore.TokenManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public TokenAuthMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<TokenAuthMiddleware>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)

        {
            //1.获取前台传递过来的密文和AES密钥密文
            //2.使用服务器的RSA私钥解密AES密钥
            //3.使用AES密钥解密密文取得明文和数字签名获得HASH
            //4.

            var authorization = context.Request.Headers.Where(h => h.Key == "Authorization").FirstOrDefault();
            var sign = context.Request.Headers.Where(h => h.Key == "X-Signature").FirstOrDefault();
            if (context.Request.Path.Equals("/login/index", StringComparison.Ordinal) ||
                context.Request.Path.Equals("/api/TokenAuth", StringComparison.Ordinal))
            {
                //if (context.Request.ContentLength.Value > 0)
                //{
                //    Stream stream = context.Request.Body;
                //    byte[] buffer = new byte[context.Request.ContentLength.Value];
                //    stream.Read(buffer, 0, buffer.Length);
                //    string content = Encoding.UTF8.GetString(buffer);
                //    Console.WriteLine(content);
                //}

                await _next(context);
            }
            else if (authorization.Key != null)
            {
                //using (StreamWriter writer = new Process().StandardInput)
                //{
                //    using (var buffer = new MemoryStream())
                //    {
                //        context.Request.Body.CopyTo(buffer);
                //        buffer.Position = 0;
                //        buffer.CopyTo(writer.BaseStream);
                //        Console.WriteLine("Request.Body:");
                //        buffer.Position = 0;
                //        buffer.CopyTo(Console.OpenStandardOutput());
                //    }
                //}
                var requestTime = DateTime.UtcNow.AddMinutes(-10);
                var curTime = DateTime.UtcNow;
                //var sign2 = context.Request.Form["signCipher"];
                //Console.WriteLine(sign2);
                //检查过期时间,根据流水号来控制防止重复请求
                if (requestTime.AddMinutes(15) > curTime)
                {
                    context.Request.Headers[authorization.Key] = PrivateManager.GetJwtTokenPlain(authorization.Value);
                }
                else
                {
                    context.Request.Headers[authorization.Key] = "";
                }

                await _next(context);
            }
            else if (sign.Key == null || authorization.Key == null)
            {
                await context.Response.WriteAsync("Bad request.");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
