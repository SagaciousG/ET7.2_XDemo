using System;
using System.Collections.Generic;
using UnityEngine;
using ET;

namespace ET.Client
{
    public static class PlayerPrefsKey
    {
        public const string GMCommon = "GMCommon";
    }

    public static class PlayerPrefsHelper
    {
        public static void Save(string key, IEnumerable<string> vals)
        {
            PlayerPrefs.SetString(key, vals.FormatStringArr());
            PlayerPrefs.Save();
        }

        public static string[] Get(string key, char symbol = ',')
        {
            var str = PlayerPrefs.GetString(key, "");
            if (string.IsNullOrEmpty(str))
                return Array.Empty<string>();
            return str.Split(symbol) ?? Array.Empty<string>();
        }
    }
}