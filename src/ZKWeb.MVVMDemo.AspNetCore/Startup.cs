﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using ZKWeb.MVVMDemo.AspNetCore.Assembles;
using ZKWeb.MVVMDemo.AspNetCore.Modules;
using ZKWeb.MVVMDemo.AspNetCore.Swagger;

namespace ZKWeb.MVVMDemo.AspNetCore
{
    /// <summary>
    /// Asp.Net Core Startup Class
    /// </summary>
    public class Startup : ZKWeb.Hosting.AspNetCore.StartupBase
    {
        /// <summary>
        /// 配置其他服务
        /// </summary>
        protected override void ConfigureOtherServices(IServiceCollection services)
        {
            //添加cros
            services.AddCors(options => options.AddPolicy("defaultCors",
                 p => p.WithOrigins("*", "http://localhost:9000", "http://localhost:53128")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("x-set-zkweb-sessionid")
            ));
            //添加Mvc组件
            services.AddMvcCore().AddApiExplorer();
            // 添加Swgger组件，使用自定义的Api列表提供器
            services.Replace(new ServiceDescriptor(
                typeof(IApiDescriptionGroupCollectionProvider),
                new ZKWebSwaggerApiProvider()));
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<ZKWebSwaggerOperationFilter>();
                c.SchemaFilter<ZKWebSwaggerSchemaFilter>();
                c.DocInclusionPredicate((a, b) => true);
                c.SwaggerDoc("v1", new Info() { Title = "ZKWeb MVVM Demo", Version = "V1" });
            });
        }

        /// <summary>
		/// 配置其他中间件
		/// </summary>
		protected override void ConfigureMiddlewares(IApplicationBuilder app)
        {
            app.UseCors("defaultCors");
            //ModulePluginManager.Instance.Initialize(typeof(MainModulePlugin));
           // ModulePluginManager.Instance.StartModules();
            // 使用错误提示页面
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePages();
            }
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
        }
    }
}
