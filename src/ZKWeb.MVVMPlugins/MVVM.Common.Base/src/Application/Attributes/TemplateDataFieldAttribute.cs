﻿using System;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class TempDataFieldAttribute : Attribute
    {
        public Type TempClassType { get; set; }
        public string TempName { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public bool Queryable { get; set; } = true;
        public bool required { get; set; } = false;
        public bool Visible { get; set; } = true;
        public bool Editable { get; set; } = true;
        public string Default { get; set; }
        public string DataType { get; set; }
        public string ComponentType { get; set; }
        public TempDataFieldAttribute()
        {

        }
    }
}
