using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Module;

namespace BusinessPlugins.SaleModule
{
    [DependsOn(typeof(BaseModulePlugin))]
    public class SaleModulePlugin : ModulePluginBase
    {
        public SaleModulePlugin()
        {
            EName = "SaleModule";
            CName = "销售管理模块";
            Version = "1.0";
            Description = "销售管理系统";
        }
    }
}
