using System;
using System.Collections.Generic;
using System.Text;

namespace ZKWeb.MVVMDemo.AspNetCore.Modules
{
    /// <summary>
    /// 
    /// </summary>

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependsOnAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public Type[] DependedModuleTypes { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependedModuleTypes"></param>

        public DependsOnAttribute(params Type[] dependedModuleTypes)
        {
            DependedModuleTypes = dependedModuleTypes;
        }
    }
}
