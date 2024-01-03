using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Unity.Mathematics;

namespace ET
{
    public static class ETExtension
    {
        [StaticField]
        private static Regex isNum = new Regex(@"^(\d|\-\d)\d*\.?\d*$");
        [StaticField]
        private static Dictionary<Enum, NameAttribute> enumNames = new();
        
        public static T GetAttribute<T>(this Enum type) where T : Attribute
        {
            var enumType = type.GetType();
            var fieldInfo = enumType.GetField(type.ToString());
            var attribute = fieldInfo.GetCustomAttribute<T>();
            return attribute;
        }
        public static NameAttribute GetNameAttribute(this Enum type)
        {
            if (enumNames.ContainsKey(type))
                return enumNames[type];
            var enumType = type.GetType();
            var fieldInfo = enumType.GetField(type.ToString());
            var nameAttribute = fieldInfo.GetCustomAttribute<NameAttribute>();
            enumNames[type] = nameAttribute;
            return nameAttribute;
        }
        
        public static float Distance(this float3 a, float3 b)
        {
            float num1 = a.x - b.x;
            float num2 = a.y - b.y;
            float num3 = a.z - b.z;
            return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
        }
        
        
        public static bool IsNullOrEmpty(this string self)
        {
            return string.IsNullOrEmpty(self);
        }
        
        /// <summary>
        /// 替换字符串末尾位置中指定的字符串
        /// </summary>
        /// <param name="s">源串</param>
        /// <param name="searchStr">查找的串</param>
        public static string TrimEndString(this string s, string searchStr)
        {
            var result = s;
            try
            {
                if (string.IsNullOrEmpty(result))
                {
                    return result;
                }
                if (s.Length < searchStr.Length)
                {
                    return result;
                }
                if (s.IndexOf(searchStr, s.Length - searchStr.Length, searchStr.Length, StringComparison.Ordinal) > -1)
                {
                    result = s.Substring(0, s.Length - searchStr.Length);
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
        
        /// <summary>
        /// 替换字符串起始位置(开头)中指定的字符串
        /// </summary>
        /// <param name="s">源串</param>
        /// <param name="searchStr">查找的串</param>
        /// <returns></returns>
        public static string TrimStartString(this string s, string searchStr)
        {
            var result = s;
            try
            {
                if (string.IsNullOrEmpty(result))
                {
                    return result;
                }
                if (s.Length < searchStr.Length)
                {
                    return result;
                }
                if (s.IndexOf(searchStr, 0, searchStr.Length, StringComparison.Ordinal) > -1)
                {
                    result = s.Substring(searchStr.Length);
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
        
              
        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToInitialUpper(this string str)
        {
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }
        
        public static string PathSymbolFormat(this string pathStr)
        {
            return pathStr.Replace("\\", "/");
        }


        public static int ToInt32(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;
            return Convert.ToInt32(str);
        }

        
        public static int[] ToIntArray(this string str, char symbol = '|')
        {
            if (string.IsNullOrEmpty(str))
                return Array.Empty<int>();
            var ss = str.Split(symbol);
            var list = new List<int>();
            foreach (string s in ss)
            {
                if (string.IsNullOrEmpty(s))
                    continue;
                list.Add(Convert.ToInt32(s));
            }

            return list.ToArray();
        }

        public static float3 ToFloat3(this string self, char symbol = ',')
        {
            if (string.IsNullOrEmpty(self))
                return float3.zero;
            var ss = self.Split(symbol);
            if (ss.Length >= 3)
            {
                return new float3(Convert.ToSingle(ss[0]), Convert.ToSingle(ss[1]), Convert.ToSingle(ss[2]));
            }
            else if (ss.Length == 2)
            {
                return new float3(Convert.ToSingle(ss[0]), Convert.ToSingle(ss[1]), 0);
            }
            else if (ss.Length == 1)
            {
                return new float3(Convert.ToSingle(ss[0]), 0, 0);
            }
            return float3.zero;
        }

        public static quaternion ToQuaternion(this string self, char symbol = ',')
        {
            return quaternion.Euler(ToFloat3(self, symbol));
        }
        
        public static Dictionary<string, string> ToMap(this string self)
        {
            var result = new Dictionary<string, string>();
            var arr = self.Split('|');
            foreach (var ss in arr)
            {
                if (string.IsNullOrEmpty(ss))
                    continue;
                var s2 = ss.Split(',');
                result[s2[0]] = s2[1];
            }

            return result;
        }
        
        public static void AddRange<T1, T2>(this Dictionary<T1, T2> map, Dictionary<T1, T2> values)
        {
            foreach (var kv in values)
            {
                map.Add(kv.Key, kv.Value);
            }
        }

        public static string FormatNum(this long val)
        {
            if (val >= math.pow(10, 8))
            {
                return (val * 1f / math.pow(10, 8)).ToString("f2") + "亿";
            }else if (val >= math.pow(10, 4))
            {
                return (val * 1f / math.pow(10, 4)).ToString("f2") + "万";
            }

            return val.ToString();
        }

        public static string ToSymbolString(this int val)
        {
            return val >= 0 ? $"+{val}" : val.ToString();
        }
        
        public static string ToSymbolString(this float val, string format = null)
        {
            return val >= 0 ? $"+{val.ToString(format)}" : val.ToString(format);
        }

        public static string Joint(this IList<string> list, char symbol = '\n')
        {
            var sb = new StringBuilder();
            foreach (var s in list)
            {
                sb.Append(s);
                sb.Append(symbol);
            }

            return sb.ToString().TrimEnd(symbol);
        }
        
        public static bool IsNum(this string str)
        {
            return isNum.IsMatch(str);
        }
    }
}