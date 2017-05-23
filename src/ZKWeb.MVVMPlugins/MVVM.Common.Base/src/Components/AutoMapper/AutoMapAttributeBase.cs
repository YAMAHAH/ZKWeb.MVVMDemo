using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.AutoMapper
{
    public abstract class AutoMapAttributeBase : Attribute
    {
        public Type[] TargetTypes { get; private set; }

        protected AutoMapAttributeBase(params Type[] targetTypes)
        {
            TargetTypes = targetTypes;
        }
        public abstract void CreateMap(IMapperConfigurationExpression configuration, Type type);
    }
}
