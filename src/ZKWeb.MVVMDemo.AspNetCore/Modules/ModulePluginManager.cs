using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZKWeb.MVVMDemo.AspNetCore.Assembles;

namespace ZKWeb.MVVMDemo.AspNetCore.Modules
{
    /// <summary>
    /// 插件模块管理
    /// </summary>
    public class ModulePluginManager
    {
        private static ModulePluginManager xInstance;
        /// <summary>
        /// 插件模块管理单例
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        public ModulePluginInfo StartupModule { get; set; }
        private ModulePluginCollection _modules;
        /// <summary>
        /// 
        /// </summary>
        public virtual void StartModules()
        {
            var sortModules = _modules.GetSortedModuleListByDependency();
            foreach (var m in sortModules)
            {
                var pluginRootNS = m.Instance.RootPath + "." + m.Instance.Path;
                var pluginPath = Path.Combine(m.Location, m.Instance.RootPath, m.Instance.Path);
                //创建插件目录
                Directory.CreateDirectory(pluginPath);
                //创建源文件目录
                var srcPath = Path.Combine(pluginPath, "src");
                Directory.CreateDirectory(srcPath);
                //创建Application,Domain,components
                Directory.CreateDirectory(Path.Combine(srcPath, "Application"));
                Directory.CreateDirectory(Path.Combine(srcPath, "Domain"));
                Directory.CreateDirectory(Path.Combine(srcPath, "Components"));

                if (srcPath.Contains(PluginConfigInfo.BusinessPluginPath))
                {
                    Directory.CreateDirectory(Path.Combine(srcPath, "Application", "Dtos"));
                    Directory.CreateDirectory(Path.Combine(srcPath, "Application", "Mappers"));
                    Directory.CreateDirectory(Path.Combine(srcPath, "Application", "Services"));
                    Directory.CreateDirectory(Path.Combine(srcPath, "Application", "AngularModule"));

                    Directory.CreateDirectory(Path.Combine(srcPath, "Domain", "Entities"));
                    Directory.CreateDirectory(Path.Combine(srcPath, "Domain", "Filters"));
                    Directory.CreateDirectory(Path.Combine(srcPath, "Domain", "Repositories"));
                    Directory.CreateDirectory(Path.Combine(srcPath, "Domain", "Services"));
                    Directory.CreateDirectory(Path.Combine(srcPath, "Domain", "Filters"));
                    Directory.CreateDirectory(Path.Combine(srcPath, "Domain", "Extensions"));
                }

                //插件信息路径
                var pluginJsonPath = Path.Combine(pluginPath, "Plugin.json");
                if (File.Exists(pluginJsonPath))
                {
                    //覆盖文件内容
                }
                else
                {
                    //新增文件
                    File.Create(pluginJsonPath);
                }
                //创建IPugin.cs
                var pluginCsPath = Path.Combine(srcPath, "Plugin.cs");
                if (File.Exists(pluginCsPath))
                {
                    //覆盖文件内容
                }
                else
                {
                    //新增文件
                    var fileContents =
@"using ZKWeb.Plugin;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule
{
    /// <summary>
    /// 插件载入时的处理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class Plugin : IPlugin
    {
        /// <summary>
        /// 插件载入时处理
        /// </summary>
        public Plugin()
        {
        }
    }
}
";
                    fileContents = fileContents.Replace("BusinessPlugins.SalesModule", pluginRootNS);
                    File.WriteAllText(pluginCsPath, fileContents);
                }
            }

            var pluginNames = sortModules
                .Select(m => m.Instance.EName + "=>" +
                    Path.Combine(m.Location, m.Instance.RootPath, m.Instance.Path)).ToList();
            var contents = string.Empty;
            foreach (var plugin in pluginNames)
            {
                contents += plugin;
                if (!(pluginNames.Last() == plugin)) contents += "," + "\r\n";
            }
            System.IO.File.WriteAllText(@"d:\plugins.txt", contents);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startupModule"></param>
        public virtual void Initialize(Type startupModule)
        {
            _modules = new ModulePluginCollection(startupModule);
            LoadAllModules();
        }
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleTypes"></param>
        /// <param name="modulePluginTypes"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleTypes"></param>
        private void RegisterModules(List<Type> moduleTypes)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modulePluginTypes"></param>
        /// <returns></returns>
        private List<Type> FindAllModuleTypes(out List<Type> modulePluginTypes)
        {
            modulePluginTypes = new List<Type>();
            var modules = ModulePluginBase.FindDependedModuleTypesRecursively(_modules.StartupModuleType);
            return modules;
        }
    }
}
