using System;
using System.Collections.Generic;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Module
{
    /// <summary>
    /// Angular前端模块定义
    /// </summary>
    public interface IAngularModule : IFrontendModule
    {
        /// <summary>
        /// 获取模块的所有模板
        /// </summary>
        List<Type> ModuleComponents { get; }
        /// <summary>
        /// 获取模块所有的模板对象信息
        /// </summary>
        /// <returns></returns>
        List<ComponentClassInfo> GetComponentClassInfoes();
    }
}
