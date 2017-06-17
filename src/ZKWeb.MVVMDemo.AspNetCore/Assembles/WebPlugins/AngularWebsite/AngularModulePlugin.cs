using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZKWeb.MVVMDemo.AspNetCore.Assembles.WebPlugins.AngularSupport;
using ZKWeb.MVVMDemo.AspNetCore.Assembles.ZKWeb.MVVMPlugins.MVVM.Common.Base;
using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles.WebPlugins.AngularWebsite
{

    /// <summary>
    /// 
    /// </summary>
    [DependsOn(
      typeof(BaseModulePlugin),
      typeof(AngularSupportModulePlugin)
      )]
    public class AngularModulePlugin : ModulePluginBase
    {
        /// <summary>
        /// 
        /// </summary>
        public AngularModulePlugin()
        {
            RootPath = PluginConfigInfo.FrontEndPluginPath;
            Path = "AngularWebsite";
            EName = "AngularWebsite";
            CName = "Angular站点";
            Version = "1.0";
            Description = "Angular展现视图";
        }
    }
}
