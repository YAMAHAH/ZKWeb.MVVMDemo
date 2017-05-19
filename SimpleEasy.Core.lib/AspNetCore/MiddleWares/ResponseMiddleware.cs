using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SimpleEasy.Core.lib.AspNetCore.MiddleWares
{
    public class ResponseRewindMiddleware
    {
        private readonly RequestDelegate next;
        public ResponseRewindMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            Stream originalBody = context.Response.Body;
            try
            {
                using (var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;
                    await next(context);
                    memStream.Position = 0;
                    //读取响应body
                    string responseBody = new StreamReader(memStream).ReadToEnd();
                    //处理body
                     responseBody += "555555555";
                    //把处理后的BODY重新写回memStream
                    //using (var writeStream = new MemoryStream())
                    //{
                    //    using (var sw = new StreamWriter(writeStream))
                    //    {
                    //        await sw.WriteAsync(responseBody);
                    //    }
                    //    writeStream.Position = 0;
                    //    await writeStream.CopyToAsync(originalBody);
                    //}
                   // await context.Response.WriteAsync(responseBody);
                    //拷贝memStream到originalBody
                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                }

            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }
    }
}
