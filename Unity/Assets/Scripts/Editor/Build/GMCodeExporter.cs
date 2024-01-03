using System.IO;
using System.Text;
using ET;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public class GMCodeExporter
    {
        [MenuItem("Build/Excel/GM")]
        public static void Build()
        {
            var gmConfig = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Bundles/Config/cs/GMConfigCategory.bytes");
            var gmCategory = (GMConfigCategory) SerializeHelper.Deserialize(typeof (GMConfigCategory), gmConfig.bytes, 0, gmConfig.bytes.Length);
            var sb = new StringBuilder();
            sb.AppendLine("namespace ET\n{");
            sb.AppendLine("\tpublic static class GMCode\n\t{");
            foreach (var kv in gmCategory.GetAll())
            {
                sb.AppendLine($"\t\t// {kv.Value.Desc}");
                sb.AppendLine($"\t\tpublic const string {kv.Value.Code} = \"{kv.Value.Code}\";");
            }
            sb.AppendLine("\t}\n}");
            File.WriteAllText("Assets/Scripts/Codes/Hotfix/Share/GMCode.cs", sb.ToString());
            AssetDatabase.Refresh();
        }
    
    }
}