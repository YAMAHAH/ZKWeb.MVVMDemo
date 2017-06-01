using System;
using System.Collections.Generic;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions
{
   public static class StringExtensions
    {
        public static string AutoDoubleQuotes(this string str)
        {
            return '"' + str + '"';
        }
    }
}
