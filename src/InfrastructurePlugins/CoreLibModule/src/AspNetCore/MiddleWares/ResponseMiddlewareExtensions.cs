using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibModule.AspNetCore.MiddleWares
{
    public static class ResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseResponseHandler(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            return app.UseMiddleware<ResponseRewindMiddleware>();
        }
    }
}
