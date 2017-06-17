using ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.MVVM.Common.Organization;
using ZKWeb.MVVMDemo.AspNetCore.Assembles.ZKWeb.MVVMPlugins.MVVM.Common.Base;
using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.MVVM.Example.CrudExample
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BaseModulePlugin),typeof(OrganizationModulePlugin))]
    public class CRUDExampleModulePlugin : ModulePluginBase
    {
        /// <summary>
        /// 
        /// </summary>
        public CRUDExampleModulePlugin()
        {
            RootPath = PluginConfigInfo.BusinessPluginPath;
            Path = "MVVM.Example.CrudExample";
            EName = "MVVM.Example.CrudExample";
            CName = "增删改查实例模块";
            Version = "1.0";
            Description = "增删改查实例模块";
        }
    }
}
