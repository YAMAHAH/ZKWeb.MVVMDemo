using System;
using System.Collections.Generic;
using System.Reflection;

namespace ZKWeb.MVVMDemo.AspNetCore.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public class ModulePluginInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public Assembly Assembly { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public string Location { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ModulePluginBase Instance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsLoadedAsPlugin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ModulePluginInfo> Dependencies { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="isLoadedAsPlugin"></param>
        public ModulePluginInfo(Type type, ModulePluginBase instance, bool isLoadedAsPlugin)
        {
            Type = type;
            Instance = instance;
            Assembly = type.GetTypeInfo().Assembly;
            Location = System.IO.Directory.GetParent(".").FullName;
            Dependencies = new List<ModulePluginInfo>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Type.AssemblyQualifiedName ?? Type.FullName;
        }
    }
}
