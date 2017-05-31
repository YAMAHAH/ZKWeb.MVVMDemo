using System;
using System.Collections.Generic;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TempModelAttribute : Attribute
    {
        public Type TempClassType { get; set; }
        public Type[] TemplateModels { get; set; }
        public TempModelAttribute(params Type[] models)
        {
            TemplateModels = models;
        }
    }
}
