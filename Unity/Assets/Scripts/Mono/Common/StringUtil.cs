using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ET
{
    public static class StringUtil
    {

        public static List<string> Find(this string str, string pattern)
        {
            var regex = new Regex(pattern);
            var match = regex.Match(str);
            var list = new List<string>();
            for (int i = 1; i < match.Groups.Count; i++)
            {
                list.Add(match.Groups[i].Value);
            }

            return list;
        }
        public static Vector3 ToVector3(this string self, char symbol = ',')
        {
            if (string.IsNullOrEmpty(self))
                return Vector3.zero;
            var ss = self.Split(symbol);
            if (ss.Length >= 3)
            {
                return new Vector3(Convert.ToSingle(ss[0]), Convert.ToSingle(ss[1]), Convert.ToSingle(ss[2]));
            }
            else if (ss.Length == 2)
            {
                return new Vector3(Convert.ToSingle(ss[0]), Convert.ToSingle(ss[1]), 0);
            }
            else if (ss.Length == 1)
            {
                return new Vector3(Convert.ToSingle(ss[0]), 0, 0);
            }
            return Vector3.zero;
        }

        public static Quaternion ToRotation(this string self, char symbol = ',')
        {
            return Quaternion.Euler(self.ToVector3(symbol));
        }
        
        public static string ToUnityPath(this string path)
        {
            return path.Replace("\\", "/").Replace($"{Application.dataPath}", "Assets");
        }
    }
}