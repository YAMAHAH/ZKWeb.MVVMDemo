﻿using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Module
{
    /// <summary>
    /// Used to store AbpModuleInfo objects as a dictionary.
    /// </summary>
    public class ModulePluginCollection : List<ModulePluginInfo>
    {
        public Type StartupModuleType { get; }

        public ModulePluginCollection(Type startupModuleType)
        {
            StartupModuleType = startupModuleType;
        }

        /// <summary>
        /// Gets a reference to a module instance.
        /// </summary>
        /// <typeparam name="TModule">Module type</typeparam>
        /// <returns>Reference to the module instance</returns>
        public TModule GetModule<TModule>() where TModule : ModulePluginBase
        {
            var module = this.FirstOrDefault(m => m.Type == typeof(TModule));
            if (module == null)
            {
                throw new Exception("Can not find module for " + typeof(TModule).FullName);
            }

            return (TModule)module.Instance;
        }

        /// <summary>
        /// Sorts modules according to dependencies.
        /// If module A depends on module B, A comes after B in the returned List.
        /// </summary>
        /// <returns>Sorted list</returns>
        public List<ModulePluginInfo> GetSortedModuleListByDependency()
        {
            var sortedModules = this.SortByDependencies(x => x.Dependencies);
            EnsureKernelModuleToBeFirst(sortedModules);
            EnsureStartupModuleToBeLast(sortedModules, StartupModuleType);
            return sortedModules;
        }

        public static void EnsureKernelModuleToBeFirst(List<ModulePluginInfo> modules)
        {
            var kernelModuleIndex = modules.FindIndex(m => m.Type == typeof(BaseModulePlugin));
            if (kernelModuleIndex <= 0)
            {
                //It's already the first!
                return;
            }

            var kernelModule = modules[kernelModuleIndex];
            modules.RemoveAt(kernelModuleIndex);
            modules.Insert(0, kernelModule);
        }

        public static void EnsureStartupModuleToBeLast(List<ModulePluginInfo> modules, Type startupModuleType)
        {
            var startupModuleIndex = modules.FindIndex(m => m.Type == startupModuleType);
            if (startupModuleIndex >= modules.Count - 1)
            {
                //It's already the last!
                return;
            }

            var startupModule = modules[startupModuleIndex];
            modules.RemoveAt(startupModuleIndex);
            modules.Add(startupModule);
        }

        public void EnsureKernelModuleToBeFirst()
        {
            EnsureKernelModuleToBeFirst(this);
        }

        public void EnsureStartupModuleToBeLast()
        {
            EnsureStartupModuleToBeLast(this, StartupModuleType);
        }
    }
}
