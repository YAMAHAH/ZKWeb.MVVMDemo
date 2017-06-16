using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Module
{
    public class ModulePluginManager
    {
        private static ModulePluginManager xInstance;
        public static ModulePluginManager Instance
        {
            get
            {
                if (xInstance == null)
                {
                    xInstance = new ModulePluginManager();
                }
                return xInstance;
            }
        }
        public ModulePluginInfo StartupModule { get; set; }
        private ModulePluginCollection _modules;
        public virtual void StartModules()
        {
            var sortModules = _modules.GetSortedModuleListByDependency();
            var pluginNames = sortModules.Select(m => m.Instance.EName).ToList();
            var contents = string.Empty;
            foreach (var plugin in pluginNames)
            {
                contents += plugin;
                if (!(pluginNames.Last() == plugin)) contents += "," + "\n";
            }
            System.IO.File.WriteAllText(@"d:\plugins.txt", contents);
        }
        public virtual void Initialize(Type startupModule)
        {
            _modules = new ModulePluginCollection(startupModule);
            LoadAllModules();
        }

        private void LoadAllModules()
        {
            List<Type> modulePluginTypes;
            var moduleTypes = FindAllModuleTypes(out modulePluginTypes).Distinct().ToList();

            //注册模块
            RegisterModules(moduleTypes);
            //创建模块
            CreateModules(moduleTypes, modulePluginTypes);

            _modules.EnsureKernelModuleToBeFirst();
            _modules.EnsureStartupModuleToBeLast();

            //设置模块依赖
            SetDependencies();

        }

        private void SetDependencies()
        {
            foreach (var moduleInfo in _modules)
            {
                moduleInfo.Dependencies.Clear();
                foreach (var dependedModuleType in ModulePluginBase.FindDependedModuleTypes(moduleInfo.Type))
                {
                    var dependedModuleInfo = _modules.FirstOrDefault(m => m.Type == dependedModuleType);
                    if (dependedModuleInfo == null)
                    {
                        throw new Exception("Could not find a depended module " + dependedModuleType.AssemblyQualifiedName + " for " + moduleInfo.Type.AssemblyQualifiedName);
                    }
                    if (moduleInfo.Dependencies.FirstOrDefault(pluginInfo => pluginInfo.Type == dependedModuleType) == null)
                    {
                        moduleInfo.Dependencies.Add(dependedModuleInfo);
                    }
                }
            }
        }

        private void CreateModules(List<Type> moduleTypes, List<Type> modulePluginTypes)
        {
            foreach (var moduleType in moduleTypes)
            {
                var moduleObject = Activator.CreateInstance(moduleType) as ModulePluginBase;

                if (moduleObject == null)
                {
                    throw new Exception("This Type is not an plugin module " + moduleType.AssemblyQualifiedName);
                }
                var moduleInfo = new ModulePluginInfo(moduleType, moduleObject, modulePluginTypes.Contains(moduleType));
                _modules.Add(moduleInfo);
                if (moduleType == _modules.StartupModuleType)
                {
                    StartupModule = moduleInfo;
                }
            }
        }

        private void RegisterModules(List<Type> moduleTypes)
        {

        }

        private List<Type> FindAllModuleTypes(out List<Type> modulePluginTypes)
        {
            modulePluginTypes = new List<Type>();
            var modules = ModulePluginBase.FindDependedModuleTypesRecursively(_modules.StartupModuleType);
            return modules;
        }
    }
}
