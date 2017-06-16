using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Module
{
    public class ModulePluginInfo
    {
        public Assembly Assembly { get; set; }
        public Type Type { get; set; }
        public ModulePluginBase Instance { get; set; }
        public bool IsLoadedAsPlugin { get; set; }
        public List<ModulePluginInfo> Dependencies { get; set; }
        public ModulePluginInfo(Type type, ModulePluginBase instance, bool isLoadedAsPlugin)
        {
            Type = type;
            Instance = instance;
            Assembly = type.GetTypeInfo().Assembly;

            Dependencies = new List<ModulePluginInfo>();
        }

        public override string ToString()
        {
            return Type.AssemblyQualifiedName ?? Type.FullName;
        }
    }
}
