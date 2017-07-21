using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZKWeb.MVVMDemo.AspNetCore.Assembles.InfrastructurePlugins.BaseModule;
using ZKWeb.MVVMDemo.AspNetCore.Modules;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles.BusinessPlugins.ProductionScheduleModule
{
    /// <summary>
    /// 生产模块配置类
    /// </summary>
    [DependsOn(typeof(BaseModulePlugin))]
    public class ProductionScheduleModulePlugin : ModulePluginBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductionScheduleModulePlugin()
        {
            RootPath = PluginConfigInfo.BusinessPluginPath;
            Path = "ProductionScheduleModule";
            EName = "ProductionScheduleModule";
            CName = "生产计划管理系统";
            Version = "1.0";
            Description = "生产计划管理系统";
        }

    }
}
