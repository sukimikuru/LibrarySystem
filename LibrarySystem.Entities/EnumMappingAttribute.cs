using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace LibrarySystem.Entities
{
    /// <summary>
    /// 枚举项在数据库中的值与在网页中的值的映射关系
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    [ComVisible(true)]
    public class EnumMappingAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        public EnumMappingAttribute(string code, string name)
        {
            DBCode = code;
            DisplayName = name;
        }
        /// <summary>
        /// 枚举编码（记入数据库）
        /// </summary>
        public string DBCode { get; private set; }
        /// <summary>
        /// 枚举显示值（程序显示）
        /// </summary>
        public string DisplayName { get; private set; }
    }
    /// <summary>
    /// 枚举扩展方法(在使用过程要中要注意Enum.ToString()的问题，因为基本上用于映射的这个本身的字符串或者值都是没有用的)
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举定义中的数据库名称
        /// </summary>
        /// <param name="myEnum"></param>
        /// <returns></returns>
        public static string GetDBCode(this Enum myEnum)
        {
            Type type = myEnum.GetType();
            FieldInfo info = type.GetField(myEnum.ToString());
            EnumMappingAttribute ema = info.GetCustomAttributes(typeof(EnumMappingAttribute), true)[0] as EnumMappingAttribute;
            if (ema != null)
                return ema.DBCode;
            else
                return null;
        }
        /// <summary>
        /// 获取枚举定义中的程序显示名称
        /// </summary>
        /// <param name="myEnum"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum myEnum)
        {
            Type type = myEnum.GetType();
            FieldInfo info = type.GetField(myEnum.ToString());
            EnumMappingAttribute spa = info.GetCustomAttributes(typeof(EnumMappingAttribute), true)[0] as EnumMappingAttribute;
            if (spa != null)
                return spa.DisplayName;
            else
                return null;
        }
        /// <summary>
        ///  根据枚举定义中的数据库名称来判断某个数据值是否与某枚举相等
        /// </summary>
        /// <param name="myEnum"></param>
        /// <param name="dbCode"></param>
        /// <returns></returns>
        public static bool EqualByDBCode(this Enum myEnum, string dbCode)
        {
            Type type = myEnum.GetType();
            FieldInfo info = type.GetField(myEnum.ToString());
            EnumMappingAttribute ema = info.GetCustomAttributes(typeof(EnumMappingAttribute), true)[0] as EnumMappingAttribute;
            if (ema != null)
            {
                if (ema.DBCode == dbCode)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        /// <summary>
        /// 根据枚举定义中的程序显示名称来判断某个显示值是否与某枚举相等
        /// </summary>
        /// <param name="myEnum"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static bool EqualByDisplayName(this Enum myEnum, string displayName)
        {
            Type type = myEnum.GetType();
            FieldInfo info = type.GetField(myEnum.ToString());
            EnumMappingAttribute ema = info.GetCustomAttributes(typeof(EnumMappingAttribute), true)[0] as EnumMappingAttribute;
            if (ema != null)
            {
                if (ema.DisplayName == displayName)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        /// <summary>
        /// 把某个数据值翻译成对应的枚举项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T Parse2EnumByDBCode<T>(string str)
        {
            try
            {
                Type t = typeof(T);
                foreach (var fi in t.GetFields(BindingFlags.Static | BindingFlags.Public))
                {
                    EnumMappingAttribute spa = Attribute.GetCustomAttributes(fi, typeof(EnumMappingAttribute), true)[0] as EnumMappingAttribute;
                    if (spa != null && spa.DBCode == str)
                    {
                        return (T)Enum.Parse(typeof(T), fi.Name, true);
                    }
                }
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// 把某个显示值翻译成对应的枚举项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T Parse2EnumByDisplayName<T>(string str)
        {
            try
            {
                Type t = typeof(T);
                foreach (var fi in t.GetFields(BindingFlags.Static | BindingFlags.Public))
                {
                    EnumMappingAttribute spa = Attribute.GetCustomAttributes(fi, typeof(EnumMappingAttribute), true)[0] as EnumMappingAttribute;
                    if (spa != null && spa.DisplayName == str)
                    {
                        return (T)Enum.Parse(typeof(T), fi.Name, true);
                    }
                }
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// 获取某个枚举类型的映射集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<EnumMappingAttribute> GetMappingList<T>()
        {
            List<EnumMappingAttribute> list = new List<EnumMappingAttribute>();
            try
            {
                Type t = typeof(T);
                foreach (var fi in t.GetFields(BindingFlags.Static | BindingFlags.Public))
                {
                    EnumMappingAttribute spa = Attribute.GetCustomAttributes(fi, typeof(EnumMappingAttribute), true)[0] as EnumMappingAttribute;
                    if (spa != null)
                    {
                        list.Add(spa);
                    }
                }
            }
            catch
            {
                list.Clear();
            }
            return list;
        }
    } 
}

