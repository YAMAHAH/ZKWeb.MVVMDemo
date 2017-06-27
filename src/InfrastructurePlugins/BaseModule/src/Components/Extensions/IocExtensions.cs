using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using ZKWeb.Plugin;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Components.Extensions
{
    public static class IocExtensions
    {
        /// <summary>
        /// Automatic register types by export attributes
        /// </summary>
        public static void RegisterExports(IEnumerable<Type> types)
        {
            var filterTypes = types.Where(t => t.GetTypeInfo().GetCustomAttribute<ExportMultipleAttribute>() != null).ToList();

            foreach (var type in filterTypes)
            {
                var typeInfo = type.GetTypeInfo();
                var exportManyAttribute = typeInfo.GetCustomAttribute<ExportMultipleAttribute>();
                if (exportManyAttribute == null)
                {
                    continue;
                }

                // From ExportManyAttribute
                var reuseType = typeInfo.GetCustomAttribute<ReuseAttribute>()?.ReuseType ?? default(ReuseType);
                var contractKey = exportManyAttribute.ContractKey;
                var except = exportManyAttribute.Except;
                var nonPublic = exportManyAttribute.NonPublic;
                var clearExists = exportManyAttribute.ClearExists;
                var baseOnType = exportManyAttribute.BaseOnType;
                var serviceTypes = GetImplementedServiceTypes(type, nonPublic).ToList();

                var baseOnTypes = baseOnType.IsNotNull() ? GetImplementedServiceTypes(baseOnType, nonPublic) : new List<Type>();

                if (except != null && except.Any())
                {
                    // Apply except types
                    serviceTypes = serviceTypes.Where(t => !except.Contains(t) && !baseOnTypes.Contains(t)).ToList();
                }
                if (clearExists)
                {
                    // Apply clear exist
                    serviceTypes.ForEach(t => ZKWeb.Application.Ioc.Unregister(t, contractKey));
                }
                foreach (var item in serviceTypes)
                {
                    // if (!ZKWeb.Application.Ioc. this.IsRegistered(item, type))
                    //{
                    ZKWeb.Application.Ioc.Register(item, type);
                    // }
                }
            }
        }
        /// <summary>
        /// Get base types and interfaces that can be a service type
        /// What type can't be a service type
        /// - It's non public and parameter nonPublicServiceTypes is false
        /// - It's from mscorlib
        /// </summary>
        public static IEnumerable<Type> GetImplementedServiceTypes(Type type, bool nonPublicServiceTypes)
        {
            var mscorlibAssembly = typeof(int).GetTypeInfo().Assembly;
            foreach (var serviceType in GetImplementedTypes(type))
            {
                var serviceTypeInfo = serviceType.GetTypeInfo();
                if ((!serviceTypeInfo.IsNotPublic || nonPublicServiceTypes) &&
                    (serviceTypeInfo.Assembly != mscorlibAssembly))
                {
                    yield return serviceType;
                }
            }
        }

        /// <summary>
        /// Get base types and interfaces
        /// </summary>
        public static IEnumerable<Type> GetImplementedTypes(Type type)
        {
            foreach (var interfaceType in type.GetTypeInfo().GetInterfaces())
            {
                yield return interfaceType;
            }
            var baseType = type;
            while (baseType != null)
            {
                yield return baseType;
                baseType = baseType.GetTypeInfo().BaseType;
            }
        }

        /// <summary>
        /// 注册所有插件程序集并标注ExportMultipleAttribute的实例到容器
        /// </summary>
        /// <param name="iocContainer">IOC容器</param>
        public static void RegisterAssemblyByConvention(this IContainer iocContainer)
        {
            var pluginManager = ZKWeb.Application.Ioc.Resolve<PluginManager>();
            pluginManager.PluginAssemblies.ForEach(asm => RegisterExports(asm.GetTypes()));
        }
        /// <summary>
        /// 注册指定程序集并标注了ExportMultipleAttribute的实例到容器
        /// </summary>
        /// <param name="iocContainer">IOC容器</param>
        public static void RegisterAssemblyByConvention(this IContainer iocContainer, Assembly[] assemblies)
        {
            assemblies.ForEach(asm => RegisterExports(asm.GetTypes()));
        }
    }
}
