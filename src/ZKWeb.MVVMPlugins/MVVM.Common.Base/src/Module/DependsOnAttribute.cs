using System;
using System.Collections.Generic;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Module
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependsOnAttribute : Attribute
    {
        public Type[] DependedModuleTypes { get; private set; }

        public DependsOnAttribute(params Type[] dependedModuleTypes)
        {
            DependedModuleTypes = dependedModuleTypes;
        }
    }
}
