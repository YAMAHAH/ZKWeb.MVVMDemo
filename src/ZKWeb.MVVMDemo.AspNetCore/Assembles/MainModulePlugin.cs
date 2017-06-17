
using ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.MVVM.Common.Organization;
using ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.MVVM.Example.CrudExample;
using ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.ProductModule;
using ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.SaleModule;
using ZKWeb.MVVMDemo.AspNetCore.Assembles.WebPlugins.AngularSupport;
using ZKWeb.MVVMDemo.AspNetCore.Assembles.WebPlugins.AngularWebsite;
using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(
        typeof(OrganizationModulePlugin),
        typeof(SaleModulePlugin),
        typeof(ProductModulePlugin),
        typeof(CRUDExampleModulePlugin),
        typeof(AngularModulePlugin),
        typeof(AngularSupportModulePlugin)
        )]
    public class MainModulePlugin : ModulePluginBase
    {
        /// <summary>
        /// 
        /// </summary>
        public MainModulePlugin()
        {
            RootPath = PluginConfigInfo.HostPath;
            Path = "MainModule";
            EName = "MainModule";
            CName = "应用主模块";
            Version = "1.0";
            Description = "应用主模块调用封装";
        }

    }
}
