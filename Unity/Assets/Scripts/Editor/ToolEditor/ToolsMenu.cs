using System;
using System.IO;
using System.Reflection;
using ET;
using MongoDB.Driver;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ET
{
    public static class ToolsMenu
    {
        [MenuItem("Tools/清除数据库")]
        private static void LoadHotDll()
        {
            var mongoClient = new MongoClient("mongodb://127.0.0.1");
            var names = mongoClient.ListDatabaseNames();
            while (names.MoveNext())
            {
                foreach (string s in names.Current)
                {
                    if (s == "admin")
                        continue;
                    mongoClient.DropDatabase(s);
                }
            }

            Debug.LogError("清除数据库。。。end");
        }
        
    }
}