using System.IO;
using System.Text;
using ET;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public class ErrorCodeExporter
    {
        [MenuItem("Build/Excel/Error Code")]
        public static void Build()
        {
            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Bundles/Config/cs/ErrorCodeConfigCategory.bytes");
            var obj = (ErrorCodeConfigCategory) SerializeHelper.Deserialize(typeof (ErrorCodeConfigCategory), asset.bytes, 0, asset.bytes.Length);
            var sb = new StringBuilder();
            sb.AppendLine("namespace ET");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static partial class ErrorCore");
            sb.AppendLine("\t{");
            foreach (var cfg in obj.GetAll())
            {
                if (cfg.Key < 200000)
                {
                    if (!string.IsNullOrEmpty(cfg.Value.Log))
                    {
                        sb.AppendLine($"\t\t/// <summary> {cfg.Value.Log} /// </summary>");
                    }
                    sb.AppendLine($"\t\tpublic const int {cfg.Value.Header} = {cfg.Value.Id};");
                }
            }
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            File.WriteAllText("Assets/Scripts/Core/Module/Network/ErrorCore.cs", sb.ToString());

            sb.Clear();
            sb.AppendLine("namespace ET");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static class ErrorCode");
            sb.AppendLine("\t{");
            foreach (var cfg in obj.GetAll())
            {
                if (cfg.Key >= 200000)
                {
                    if (!string.IsNullOrEmpty(cfg.Value.Log))
                    {
                        sb.AppendLine($"\t\t/// <summary> {cfg.Value.Log} /// </summary>");
                    }
                    sb.AppendLine($"\t\tpublic const int {cfg.Value.Header} = {cfg.Value.Id};");
                }
            }
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            File.WriteAllText("Assets/Scripts/Codes/Model/Share/Module/Message/ErrorCode.cs", sb.ToString());
            AssetDatabase.Refresh();
        }
    
    }
}