﻿using InfrastructurePlugins.BaseModule.Components.CORSExposeHeaders.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Web;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace InfrastructurePlugins.BaseModule.Components.RequestHandlers
{
    /// <summary>
    /// 处理跨站请求的处理器
    /// </summary>
    [ExportMany]
    public class CorsRequestHandler : IHttpRequestPreHandler
    {
        public void OnRequest()
        {
            //var context = HttpManager.CurrentContext;
            //// 仅对ajax请求启用
            //if (context.Request.IsAjaxRequest())
            //{
            //    // 允许不使用OPTIONS直接发来的请求
            //    context.Response.AddHeader("Access-Control-Allow-Origin", "*");

            //    //充许客户获取的头列表
            //    var exposeHeaders = ZKWeb.Application.Ioc.ResolveMany<ICORSExposeHeader>();
            //    context.Response.AddHeader("Access-Control-Expose-Headers",
            //        string.Join(",", exposeHeaders.Select(e => e.ExposeHeader.ToLower())));
            //    //var accessControlHeader = context.Request.GetHeader("Access-Control-Request-Method");
            //    //|| (context.Request.Method == "GET" && accessControlHeader != null)
            //    // 允许使用OPTIONS探测的请求
            //    if (context.Request.Method == "OPTIONS")
            //    {
            //        var requestHeaders = context.Request.GetHeader("Access-Control-Request-Headers");
            //        var requestMethod = context.Request.GetHeader("Access-Control-Request-Method");
            //        context.Response.AddHeader("Access-Control-Allow-Headers", requestHeaders);
            //        context.Response.AddHeader("Access-Control-Allow-Methods", requestMethod);
            //        context.Response.AddHeader("Access-Control-Max-Age", "1296000");
            //        context.Response.StatusCode = 200;
            //        context.Response.End();
            //    }
            //}
        }
    }
}
