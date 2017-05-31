using System;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TempClassAttribute : Attribute
    {
        public string TemplateId { get; set; }
        public string TempName { get; set; }
    }
}
