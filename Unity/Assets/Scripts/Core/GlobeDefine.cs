using System;
using System.Collections.Generic;
using System.IO;

namespace ET
{

    public static class GlobeDefine
    {
        public static int RouterCheckInterval => Convert.ToInt32(_map["RouterCheckInterval"]);


        [StaticField]
        private static Dictionary<string, string> _map = new();
        static GlobeDefine()
        {
            var lines = File.ReadLines("../Config/GlobeDefine.txt");
            foreach (string line in lines)
            {
                var ss = line.Split('=');
                _map[ss[0]] = ss[1];
            }
        }
    }

}