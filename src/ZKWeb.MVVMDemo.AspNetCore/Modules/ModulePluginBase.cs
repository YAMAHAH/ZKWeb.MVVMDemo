using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZKWeb.MVVMDemo.AspNetCore.Assembles.InfrastructurePlugins.BaseModule;
using ZKWeb.MVVMDemo.AspNetCore.Extensions;

namespace ZKWeb.MVVMDemo.AspNetCore.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ModulePluginBase : IModulePlugin
    {
        /// <summary>
        /// 
        /// </summary>
        public ModulePluginBase Instance { get; set; }
        /// <summary>
        /// 插件父亲路径
        /// </summary>
        public string RootPath { get; set; }
        /// <summary>
        /// 插件目录路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 模块英名名称
        /// </summary>
        public string EName { get; set; }
        /// <summary>
        /// 模块中文名称
        /// </summary>
        public string CName { get; set; }
        /// <summary>
        /// 模块版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 模块描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 依赖模块
        /// </summary>
        public IList<ModulePluginBase> Dependencies { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsModulePlugin(Type type)
        {
            return type.GetTypeInfo().IsClass &&
                !type.GetTypeInfo().IsAbstract &&
                !type.GetTypeInfo().IsGenericType &&
                typeof(ModulePluginBase).IsAssignableFrom(type);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        public static List<Type> FindDependedModuleTypes(Type moduleType)
        {
            if (!IsModulePlugin(moduleType))
            {
                throw new Exception("This type is not an Module Plugin:" + moduleType.AssemblyQualifiedName);
            }
            var list = new List<Type>();
            if (moduleType.GetTypeInfo().IsDefined(typeof(DependsOnAttribute), true))
            {
                var dependsOnAttributes = moduleType.GetTypeInfo()
                    .GetCustomAttributes(typeof(DependsOnAttribute), true)
                    .Cast<DependsOnAttribute>();
                foreach (var dependsOnAttribute in dependsOnAttributes)
                {
                    foreach (var dependedModuleType in dependsOnAttribute.DependedModuleTypes)
                    {
                        list.Add(dependedModuleType);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        public static List<Type> FindDependedModuleTypesRecursively(Type moduleType)
        {
            var list = new List<Type>();
            AddModuleAndDependenciesResursively(list, moduleType);
            list.AddIfNotContains(typeof(BaseModulePlugin));
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="moduleType"></param>
        private static void AddModuleAndDependenciesResursively(List<Type> modules, Type moduleType)
        {
            if (!IsModulePlugin(moduleType))
            {
                throw new Exception("This type is not an Module Plugin:" + moduleType.AssemblyQualifiedName);
            }
            if (modules.Contains(moduleType))
            {
                return;
            }
            modules.Add(moduleType);
            var dependedModules = FindDependedModuleTypes(moduleType);
            foreach (var dependedModule in dependedModules)
            {
                AddModuleAndDependenciesResursively(modules, dependedModule);
            }
        }
    }
}
