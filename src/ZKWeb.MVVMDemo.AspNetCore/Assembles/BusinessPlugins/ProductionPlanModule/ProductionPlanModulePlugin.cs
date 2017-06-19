using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZKWeb.MVVMDemo.AspNetCore.Assembles.ZKWeb.MVVMPlugins.MVVM.Common.Base;
using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.ProductionPlanModule
{
    /// <summary>
    /// 生产模块配置类
    /// </summary>
    [DependsOn(typeof(BaseModulePlugin))]
    public class ProductionPlanModulePlugin : ModulePluginBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductionPlanModulePlugin()
        {
            RootPath = PluginConfigInfo.BusinessPluginPath;
            Path = "ProductionPlanModule";
            EName = "ProductionPlanModule";
            CName = "生产计划管理系统";
            Version = "1.0";
            Description = "生产计划管理系统";
        }

    }
}
