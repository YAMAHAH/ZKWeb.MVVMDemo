﻿using System;
using System.Collections.Generic;
using System.Linq;
using InfrastructurePlugins.BaseModule.Application.Services.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Module
{
    /// <summary>
    /// 模块管理器
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ModuleManager
    {
        private IEnumerable<IApplicationService> xComponents;

        public IEnumerable<IApplicationService> AllModuleComponents
        {
            get
            {
                if (xComponents == null)
                {
                    xComponents = ZKWeb.Application.Ioc.ResolveMany<IApplicationService>();
                }
                return xComponents;
            }
        }
        private Dictionary<Type, List<Type>> moduleComponentDict;
        public ModuleManager()
        {
            moduleComponentDict = new Dictionary<Type, List<Type>>();
        }

        public void RegisterModuleComponent(Type moduleType, List<Type> components)
        {
            var moduleTypes = moduleComponentDict.GetOrCreate(moduleType, () => new List<Type>());
            var addTypes = components.Where(t => !moduleTypes.Contains(t)).ToList();
            moduleTypes.AddRange(addTypes);
        }

        public void RegisterModuleComponent(Type moduleType, Type template)
        {
            var moduleTypes = moduleComponentDict.GetOrCreate(moduleType, () => new List<Type>());

            if (!moduleTypes.Contains(template))
            {
                moduleTypes.Add(template);
            }
        }

        public List<Type> GetModuleComponents(Type moduleType)
        {
            return moduleComponentDict.GetOrCreate(moduleType, () => new List<Type>());
        }

        /// <summary>
        /// 获取所有模块的模板对象信息
        /// </summary>
        /// <returns></returns>
        public List<ComponentClassInfo> GetModuleComponentClassInfos()
        {
            var injector = ZKWeb.Application.Ioc;
            var modules = injector.ResolveMany<IAngularModule>();
            return modules.SelectMany(m => m.GetComponentClassInfoes()).ToList();
        }
    }
}
