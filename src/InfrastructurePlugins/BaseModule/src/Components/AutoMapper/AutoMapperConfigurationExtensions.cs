using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZKWeb.Plugin;
using ZKWebStandard.Extensions;

namespace InfrastructurePlugins.BaseModule.Components.AutoMapper
{
    internal static class AutoMapperConfigurationExtensions
    {
        public static void CreateAutoAttributeMaps(this IMapperConfigurationExpression configuration, Type type)
        {
            foreach (var autoMapAttribute in type.GetTypeInfo().GetCustomAttributes<AutoMapAttributeBase>())
            {
                autoMapAttribute.CreateMap(configuration, type);
            }
        }
        public static void FindAndAutoMapTypes(this IMapperConfigurationExpression configuration)
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

        public static void AddCustomizeProfiles(this IMapperConfigurationExpression configuration)
        {
            IEnumerable<Profile> profiles = ZKWeb.Application.Ioc.ResolveMany<Profile>();
            profiles.ForEach(p => configuration.AddProfile(p));
        }
    }
}
