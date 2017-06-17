﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using ZKWeb.MVVMDemo.AspNetCore.Assembles;
using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore
{
    /// <summary>
    /// Asp.Net Core Startup Class
    /// </summary>
    public class Startup : ZKWeb.Hosting.AspNetCore.StartupBase
    {
        /// <summary>
        /// 配置程序
        /// </summary>
        public override void Configure(IApplicationBuilder app)
        {
            ModulePluginManager.Instance.Initialize(typeof(MainModulePlugin));
            ModulePluginManager.Instance.StartModules();
            // 使用错误提示页面
            var env = (IHostingEnvironment)app.ApplicationServices.GetService(typeof(IHostingEnvironment));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePages();
            }
 
            app.UseCors("defaultCors");

            // 使用Swagger中间件
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.InjectOnCompleteJavaScript("/swagger/swagger-site.js");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZKWeb MVVM Demo V1");
            });
            // 使用Mvc中间件
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            // 使用ZKWeb中间件
            base.Configure(app);
            // 注册IServiceProvider
            Application.Ioc.RegisterInstance(app.ApplicationServices);
        }
    }
}
