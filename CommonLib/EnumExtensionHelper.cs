using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CommonLib
{
    /// <summary>
    /// 枚举扩展帮助类
    /// </summary>
    public static class EnumExtensionHelper
    {
        /// <summary>  
        /// 扩展方法：根据枚举值得到相应的枚举定义字符串  
        /// </summary>  
        /// <param name="value"></param>  
        /// <param name="enumType"></param>  
        /// <returns></returns>  
        public static String ToEnumString(this int value, Type enumType)
        {
            var nvc = GetEnumStringFromEnumValue(enumType);
            return nvc[value.ToString()];
        }
        public static List<string> GetEnumDescriptionList(Type enumType)
        {
            var collection = GetNvcFromEnumValue(enumType);
            return collection.AllKeys.Select(key => collection[key]).ToList();
        }



        /// <summary>  
        /// 根据枚举类型得到其所有的 值 与 枚举定义字符串 的集合  
        /// </summary>  
        /// <param name="enumType"></param>  
        /// <returns></returns>  
        public static NameValueCollection GetEnumStringFromEnumValue(Type enumType)
        {
            var nvc = new NameValueCollection();
            var typeDescription = typeof(DescriptionAttribute);
            var fields = enumType.GetFields();
            var strText = string.Empty;
            var strValue = string.Empty;
            foreach (var field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    nvc.Add(strValue, field.Name);
                }
            }
            return nvc;
        }

        /// <summary>  
        /// 扩展方法：根据枚举值得到属性Description中的描述, 如果没有定义此属性则返回空串  
        /// </summary>  
        /// <param name="value"></param>  
        /// <param name="enumType"></param>  
        /// <returns></returns>  
        public static String ToEnumDescriptionString(this int value, Type enumType)
        {
            var nvc = GetNvcFromEnumValue(enumType);
            return nvc[value.ToString()];
        }

        /// <summary>  
        /// 根据枚举类型得到其所有的 值 与 枚举定义Description属性 的集合  
        /// </summary>  
        /// <param name="enumType"></param>  
        /// <returns></returns>  
        public static NameValueCollection GetNvcFromEnumValue(Type enumType)
        {
            var nvc = new NameValueCollection();
            var typeDescription = typeof(DescriptionAttribute);
            var fields = enumType.GetFields();
            var strText = string.Empty;
            var strValue = string.Empty;
            foreach (var field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    var arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        var aa = (DescriptionAttribute)arr[0];
                        strText = aa.Description;
                    }
                    else
                    {
                        strText = "";
                    }
                    nvc.Add(strValue, strText);
                }
            }
            return nvc;
        }

        /// <summary>  
        /// 根据枚举类型得到其所有的 值 与 枚举定义Description属性 的集合(列反转 只是用于商品 attr)  
        /// </summary>  
        /// <param name="enumType"></param>  
        /// <returns></returns>  
        public static NameValueCollection GetReversNvcFromEnumValue(Type enumType)
        {
            var nvc = new NameValueCollection();
            var typeDescription = typeof(DescriptionAttribute);
            var fields = enumType.GetFields();
            var strText = string.Empty;
            var strValue = string.Empty;
            foreach (var field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    var arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        var aa = (DescriptionAttribute)arr[0];
                        strText = aa.Description;
                    }
                    else
                    {
                        strText = Guid.NewGuid().ToString();
                    }
                    nvc.Add(strText, strValue);
                }
            }
            return nvc;
        }
    }
}
