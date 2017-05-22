using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SimpleEasy.Core.lib.AspNetCore.MiddleWares
{
    public class RequestRewindMiddleware
    {
        private readonly RequestDelegate next;
        public RequestRewindMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var originalRequestBody = context.Request.Body;
            context.Request.EnableRewind();
            try
            {
                using (MemoryStream m = new MemoryStream())
                {
                    context.Request.Body.CopyTo(m);
                    var s = System.Text.Encoding.UTF8.GetString(m.ToArray());
                    s += "5555";
                    //using (var writeStream = new MemoryStream())
                    //{
                    //    using (var sw = new StreamWriter(writeStream))
                    //    {
                    //        await sw.WriteAsync(s);
                    //    }
                    //    writeStream.Position = 0;
                    //    // await writeStream.CopyToAsync(originalBody);
                    //    context.Request.Body = writeStream;
                    //}
                }
                //this line will rewind the request body, so it could be read again
                context.Request.Body.Position = 0;
                await next(context);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //important, otherwise, even current request will succeed, following request will fail
                context.Request.Body = originalRequestBody;
            }
        }
    }
}
