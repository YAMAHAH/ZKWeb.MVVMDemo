using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructurePlugins.BaseModule.Template
{
    public class AngularElement : TemplateElement
    {
        #region 元素创建工厂
        public static TemplateElement Create(string tag, string name)
        {
            var instance = new AngularElement() { Name = name, ElementTag = tag };
            if (instance.Name != default(string)) instance.AddProperty(nameof(instance.Name).ToLower(), instance.Name);
            return instance;
        }
        #endregion

        #region 重载个性化设置
        public override string NormalizationEventKey(string key)
        {
            return "(" + key + ")";
        }

        public override string NormalizationPropertyKey(string key)
        {
            return "[" + key + "]";
        }
        #endregion
    }
}
