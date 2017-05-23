using AutoMapper;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.AutoMapper
{
    internal static class AutoMapperConfigurationExtensions
    {
        public static void CreateAutoAttributeMaps(this IMapperConfigurationExpression configuration, Type type)
        {
            foreach (var autoMapAttribute in type.GetTypeInfo().GetCustomAttributes<AutoMapAttributeBase>())
            {
               autoMapAttribute.CreateMap(configuration, type);
            }
        }
    }
}
