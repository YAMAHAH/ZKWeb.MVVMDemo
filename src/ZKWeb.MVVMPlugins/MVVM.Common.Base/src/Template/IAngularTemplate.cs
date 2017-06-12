using System.Collections.Generic;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Module;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Template
{
    public interface IAngularTemplate : ITemplate
    {
        /// <summary>
        /// 获取模板依赖的所有组件
        /// </summary>
        /// <returns></returns>
        List<ComponentClassInfo> GetDependOnComponents();
        /// <summary>
        /// 模板转换成文本
        /// </summary>
        /// <returns></returns>
        Dictionary<string,string> TransformText();

    }
}
