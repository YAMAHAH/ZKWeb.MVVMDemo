using BusinessPlugins.MVVM.Common.Organization;
using BusinessPlugins.ProductModule;
using BusinessPlugins.SaleModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Module;

namespace ZKWeb.MVVMDemo.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(
        typeof(OrganizationModulePlugin),
        typeof(SaleModulePlugin),
        typeof(ProductModulePlugin))]
    public class StartupModulePlugin : ModulePluginBase
    {
        /// <summary>
        /// 
        /// </summary>
        public StartupModulePlugin()
        {
            EName = "MainModule";
            CName = "Organization Structure For MVVM";
            Version = "1.0";
            Description = "Provide organization entities and domain services";
        }

    }
}
