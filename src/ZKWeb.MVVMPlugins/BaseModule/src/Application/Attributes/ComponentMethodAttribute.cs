using System;

namespace InfrastructurePlugins.BaseModule.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ComponentMethodAttribute:Attribute
    {
        /// <summary>
        /// 所属模板类类型
        /// </summary>
        public Type TempClassType { get; set; }
        /// <summary>
        /// 模板类名称
        /// </summary>
        public string TempName { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        public bool Enable { get; set; } = true;
        /// <summary>
        /// 默认值
        /// </summary>
        public bool Default { get; set; } = true;
        /// <summary>
        /// 功能标题文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        public string Name { get; set; }

        public ComponentMethodAttribute(string name,string text,bool enable,bool defaultValue)
        {
            Name = name;
            Text = text;
            Enable = enable;
            //如果enable禁用,则default无效
            Default = Enable && defaultValue;
        }
    }
}
