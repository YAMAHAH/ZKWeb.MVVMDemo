using Microsoft.AspNetCore.Builder;
using System;

namespace SimpleEasy.Core.lib.AspNetCore.MiddleWares
{
    public static class RequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestHandler(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            return app.UseMiddleware<RequestRewindMiddleware>();
        }
    }
}
