using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Module;

namespace BusinessPlugins.ProductModule
{
    [DependsOn(typeof(BaseModulePlugin))]
    public class ProductModulePlugin : ModulePluginBase
    {
        public ProductModulePlugin()
        {
            EName = "ProductModule";
            CName = "产品管理系统";
            Version = "1.0";
            Description = "产品管理系统";
        }
    }
}
