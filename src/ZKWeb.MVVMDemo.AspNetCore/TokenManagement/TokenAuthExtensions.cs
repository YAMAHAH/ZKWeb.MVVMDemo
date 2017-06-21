using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZKWeb.MVVMDemo.AspNetCore.TokenManagement
{
  /// <summary>
  /// 
  /// </summary>
    public static class TokenAuthExtensions
    {
       /// <summary>
       /// 
       /// </summary>
       /// <param name="app"></param>
       /// <returns></returns>
        public static IApplicationBuilder UseTokenAuth(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            //if (options == null)
            //{
            //    throw new ArgumentNullException(nameof(options));
            //}

            return app.UseMiddleware<TokenAuthMiddleware>();
        }
    }
}
