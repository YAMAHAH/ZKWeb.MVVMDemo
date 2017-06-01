using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions
{
    /// <summary>
    /// Extension methods for all objects.
    /// </summary>
    public static class ObjectExtensions
    {
        public static TypeInfo GetTypeInfoEx(this object obj)
        {
            return obj.GetType().GetTypeInfo();
        }
        public static T GetAttribute<T>(this object obj) where T : Attribute
        {
            return obj.GetType().GetTypeInfo().GetCustomAttribute<T>();
        }

        public static MethodInfo GetAttributeMethod<T>(this object obj) where T : Attribute
        {
            return obj.GetTypeInfoEx().GetMethods().FirstOrDefault(m => m.GetCustomAttribute<T>() != null);
        }
        public static bool IsQueryFilter(this Type type)
        {
            return typeof(IEntityQueryFilter).IsAssignableFrom(type);
        }
        public static bool IsOperationFilter(this Type type)
        {
            return typeof(IEntityOperationFilter).IsAssignableFrom(type);
        }
        public static bool IsQueryOrOperationFilter(this Type type)
        {
            return typeof(IEntityQueryFilter).IsAssignableFrom(type) ||
                 typeof(IEntityOperationFilter).IsAssignableFrom(type);
        }
        public static Assembly GetExecutingAssembly(this object obj)
        {
            return obj.GetType().GetTypeInfo().Assembly;
        }
        /// <summary>
        /// Used to simplify and beautify casting an object to a type. 
        /// </summary>
        /// <typeparam name="T">Type to be casted</typeparam>
        /// <param name="obj">Object to cast</param>
        /// <returns>Casted object</returns>
        public static T As<T>(this object obj)
            where T : class
        {
            return (T)obj;
        }

        /// <summary>
        /// Converts given object to a value type using <see cref="Convert.ChangeType(object,System.TypeCode)"/> method.
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <typeparam name="T">Type of the target object</typeparam>
        /// <returns>Converted object</returns>
        public static T To<T>(this object obj)
            where T : struct
        {
            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Check if an item is in a list.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <param name="list">List of items</param>
        /// <typeparam name="T">Type of the items</typeparam>
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }

        /// <summary>
        /// Check if an item is in a list.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <param name="list">List of items</param>
        /// <typeparam name="T">Type of the items</typeparam>
        public static bool IsNotNull(this object obj)
        {
            return obj == null ? false : true;
        }

        public static bool IsNull(this object obj)
        {
            return obj == null ? true : false;
        }
    }
}
