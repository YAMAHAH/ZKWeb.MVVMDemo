using System;
using System.Collections.Generic;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.Attributes
{
    public class TempFilterAttribute : Attribute
    {
        /// <summary>
        /// 所属模板类类型
        /// </summary>
        public Type TempClassType { get; set; }
        /// <summary>
        /// 过滤的类型
        /// </summary>
        public Type[] FilterTypes { get; set; }

        public TempFilterAttribute(params Type[] filterTypes)
        {
            FilterTypes = filterTypes;
        }
    }
}
