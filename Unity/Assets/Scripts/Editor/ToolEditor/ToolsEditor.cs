using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using ET;

namespace ET
{
    public static class ToolsEditor
    {
        [MenuItem("Build/编译Excel")]
        public static void ExcelExporter()
        {
#if UNITY_EDITOR_OSX
            const string tools = "./Tool";
#else
            const string tools = ".\\Tool.exe";
#endif
            ShellHelper.Run($"{tools} --AppType=ExcelExporter --Console=1", "../Bin/");
            
            AssetDatabase.Refresh();
        }
        

        public static void Proto2CS()
        {
#if UNITY_EDITOR_OSX
            const string tools = "./Tool";
#else
            const string tools = ".\\Tool.exe";
#endif
            ShellHelper.Run($"{tools} --AppType=Proto2CS --Console=1", "../Bin/");
        }

        public static void BuildAndRun()
        {
            BuildServer();
            RunServer();
        }

        public static void BuildServer()
        {
            System.Diagnostics.Process.Start("cmd.exe",$"../Tools/buildServer.bat");
        }

        public static void RunServer()
        {
            System.Diagnostics.Process.Start("cmd.exe",$"../Tools/runServer.bat");
        }
        
        
    }
}