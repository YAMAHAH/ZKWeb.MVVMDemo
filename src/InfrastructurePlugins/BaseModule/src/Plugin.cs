﻿using AutoMapper;
using System.Collections.Generic;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.BaseModule.Components.ActionParameterProviders;
using InfrastructurePlugins.BaseModule.Components.AutoMapper;
using ZKWeb.Plugin;
using ZKWeb.Web;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.src
{
    /// <summary>
    /// 插件载入时的处理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class Plugin : IPlugin
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public Plugin(IEnumerable<Profile> profiles)
        {
            //注册标识了ExportMultipleAttribute的实例到容器
           
            ZKWeb.Application.Ioc.RegisterAssemblyByConvention();
            // 自动注册AutoMapper的映射设置
            Mapper.Initialize(cfg =>
            {
                cfg.FindAndAutoMapTypes();
                cfg.AddCustomizeProfiles();
            });
            // 替换默认的参数提供器
            var originalProvider = ZKWeb.Application.Ioc.Resolve<IActionParameterProvider>();
            ZKWeb.Application.Ioc.Unregister<IActionParameterProvider>();
            ZKWeb.Application.Ioc.RegisterInstance<IActionParameterProvider>(
                new ValidatedActionParameterProvider(originalProvider));
        }
    }
}