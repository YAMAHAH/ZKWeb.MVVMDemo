using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZKWeb.MVVMDemo.AspNetCore.Assembles
{
    /// <summary>
    /// 插件配置信息
    /// </summary>
    public static class PluginConfigInfo
    {
        /// <summary>
        /// 主应用目录
        /// </summary>
        public static string HostPath = "ZKWeb.MVVMDemo.AspNetCore";
        /// <summary>
        /// 基本库插件目录
        /// </summary>
        public static string BasePluginPath = "BasePlugins";
        /// <summary>
        /// 前端插件目录
        /// </summary>
        public static string FrontEndPluginPath = "WebPlugins";
        /// <summary>
        /// 业务插件目录
        /// </summary>
        public static string BusinessPluginPath = "BusinessModulePlugins";

    }
}
