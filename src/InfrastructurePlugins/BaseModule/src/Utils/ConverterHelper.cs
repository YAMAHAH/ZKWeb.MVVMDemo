using System;
using System.Collections;
using System.ComponentModel;
using System.DrawingCore;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace InfrastructurePlugins.BaseModule.Utils
{
    public static class ConverterHelper
    {
        public static object ConvertNull(Type type)
        {
            if (type == typeof(string))
            {
                return "";
            }
            if (type == typeof(char))
            {
                return ' ';
            }
            if (type == typeof(DateTime))
            {
                return DateTime.MinValue;
            }
            if (type == typeof(TimeSpan))
            {
                return TimeSpan.Zero;
            }
            if (type == typeof(bool))
            {
                return false;
            }
            if (type == typeof(byte[]))
            {
                return null;
            }
            if (type.GetTypeInfo().IsClass)
            {
                return null;
            }
            if (type.GetTypeInfo().IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                return null;
            }
            return Convert.ChangeType(0, type);
        }
        public static float DecreasePrecision(float value, int precision)
        {
            return (float)Math.Round((double)value, precision);
        }

        public static object FromString(string value, Type converterType)
        {
            TypeConverter converter = Activator.CreateInstance(converterType) as TypeConverter;
            return converter.ConvertFromInvariantString(value);
        }

        public static object FromString(Type type, string value)
        {
            if (type == typeof(string))
            {
                return value;
            }
            if ((value == null) || (value == ""))
            {
                return null;
            }
            if (type == typeof(float))
            {
                return float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            }
            if (type == typeof(Enum))
            {
                return Enum.Parse(type, value);
            }
            //if ((type == typeof(Image)) || (type == typeof(Bitmap)))
            //{
            //    using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(value)))
            //    {
            //        return ImageHelper.Load(stream);
            //    }
            //}
            if (type == typeof(Stream))
            {
                return new MemoryStream(Convert.FromBase64String(value));
            }
            if (type == typeof(Type))
            {
                return Type.GetType(value);
            }
            if (type == typeof(string[]))
            {
                value = value.Replace("\r\n", "\r");
                return value.Split(new char[] { '\r' });
            }
            if (type == typeof(Color))
            {
                return new ColorConverter().ConvertFromInvariantString(value);
            }
            return TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);
        }

        public static string ToString(object value)
        {
            if (value == null)
            {
                return "";
            }
            if (value is string)
            {
                return (string)value;
            }
            if (value is float)
            {
                float num = (float)value;
                return num.ToString(CultureInfo.InvariantCulture.NumberFormat);
            }
            if (value is Enum)
            {
                return Enum.Format(value.GetType(), value, "G");
            }
            if (value is Image)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    ImageHelper.Save(value as Image, stream);
                    return Convert.ToBase64String(stream.ToArray());
                }
            }
            if (value is Stream)
            {
                Stream stream2 = value as Stream;
                byte[] buffer = new byte[stream2.Length];
                stream2.Position = 0L;
                stream2.Read(buffer, 0, buffer.Length);
                return Convert.ToBase64String(buffer);
            }
            if (value is string[])
            {
                string str = "";
                foreach (string str2 in value as string[])
                {
                    str = str + str2 + "\r\n";
                }
                if (str.EndsWith("\r\n"))
                {
                    str = str.Remove(str.Length - 2);
                }
                return str;
            }
            if (!(value is Type))
            {
                return TypeDescriptor.GetConverter(value).ConvertToInvariantString(value);
            }
            Type type = (Type)value;
            if (type.GetTypeInfo().Assembly.FullName.StartsWith("mscorlib") || (type.GetTypeInfo().Assembly == typeof(ConverterHelper).GetTypeInfo().Assembly))
            {
                return type.FullName;
            }
            return type.AssemblyQualifiedName;
        }

        public static string ToString(object value, Type converterType)
        {
            TypeConverter converter = Activator.CreateInstance(converterType) as TypeConverter;
            return converter.ConvertToInvariantString(value);
        }

        public static object ChangeType(object value, Type propertyType)
        {
            Type propertyType2 = propertyType;
            if (propertyType.GetTypeInfo().IsGenericType)
            {
                propertyType2 = propertyType.GetGenericArguments()[0];
            }
            if (typeof(IConvertible).IsAssignableFrom(propertyType2))
            {
                return Convert.ChangeType(value, propertyType2);
            }
            return value;
        }
        public static T ConvertTo<T>(object source)
        {
            //string转换为任意类型
            if (source is string)
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
                object temp = tc.ConvertFromInvariantString(source.ToString());
                return (T)temp;
            }

            //任意类型转换为string
            else if (!(source is string) && (typeof(T) == typeof(string)))
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
                if (tc.CanConvertTo(typeof(string)))
                    return (T)(object)tc.ConvertToInvariantString(source.ToString());
                else
                    return (T)(object)source.ToString();
            }

            //如果source是队列
            else if (source is ArrayList && typeof(T).IsArray)
            {
                ArrayList list = (ArrayList)source;
                Type elementType = typeof(T).GetElementType();
                Array arr = Array.CreateInstance(elementType, list.Count);
                TypeConverter tc = TypeDescriptor.GetConverter(elementType);
                for (int i = 0; i < list.Count; i++)
                {
                    arr.SetValue(tc.ConvertFromInvariantString(list[i].ToString()), i);
                }
                return (T)(object)arr;
            }
            else return (T)(object)(string.Empty);
        }
    }
}
