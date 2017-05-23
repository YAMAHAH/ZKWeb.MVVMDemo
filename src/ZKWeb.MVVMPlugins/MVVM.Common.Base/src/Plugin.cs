using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.ActionParameterProviders;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.AutoMapper;
using ZKWeb.Plugin;
using ZKWeb.Web;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWeb.Plugin.AssemblyLoaders;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src
{
    /// <summary>
    /// 插件载入时的处理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class Plugin : IPlugin
    {
        private void FindAndAutoMapTypes(IMapperConfigurationExpression configuration)
        {
            var pluginManager = ZKWeb.Application.Ioc.Resolve<PluginManager>();
            var allTypes = pluginManager.PluginAssemblies.SelectMany(a => a.GetTypes())
                .Where(t =>
                    t.GetTypeInfo().IsDefined(typeof(AutoMapAttribute)) ||
                    t.GetTypeInfo().IsDefined(typeof(AutoMapFromAttribute)) ||
                    t.GetTypeInfo().IsDefined(typeof(AutoMapToAttribute))
                );

            foreach (var type in allTypes)
            {
                configuration.CreateAutoAttributeMaps(type);
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public Plugin(IEnumerable<Profile> profiles)
        {
            // 自动注册AutoMapper的映射设置
            Mapper.Initialize(cfg =>
            {
                profiles.ForEach(p => cfg.AddProfile(p));
                FindAndAutoMapTypes(cfg);
            });
            // 替换默认的参数提供器
            var originalProvider = ZKWeb.Application.Ioc.Resolve<IActionParameterProvider>();
            ZKWeb.Application.Ioc.Unregister<IActionParameterProvider>();
            ZKWeb.Application.Ioc.RegisterInstance<IActionParameterProvider>(
                new ValidatedActionParameterProvider(originalProvider));
        }
    }
}
