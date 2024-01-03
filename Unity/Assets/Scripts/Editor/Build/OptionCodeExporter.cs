using System.IO;
using System.Text;
using ET;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public class OptionCodeExporter
    {
        [MenuItem("Build/Excel/OptionCode")]
        public static void Build()
        {
            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Bundles/Config/cs/OptionCodeConfigCategory.bytes");
            var obj = (OptionCodeConfigCategory) SerializeHelper.Deserialize(typeof (OptionCodeConfigCategory), asset.bytes, 0, asset.bytes.Length);
            var sb = new StringBuilder();
            sb.AppendLine("//从OptionCode表中生成，请勿编辑");
            sb.AppendLine("namespace ET");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static class OptionCode");
            sb.AppendLine("\t{");
            foreach (var cfg in obj.GetAll())
            {
                if (!string.IsNullOrEmpty(cfg.Value.Tips))
                {
                    sb.AppendLine($"\t\t/// <summary> {cfg.Value.Tips} /// </summary>");
                }
                sb.AppendLine($"\t\tpublic const string {cfg.Value.FieldName} = \"{cfg.Value.FieldName}\";");
            }
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            File.WriteAllText("Assets/Scripts/Codes/Model/Share/Module/OptionCode.cs", sb.ToString());
            AssetDatabase.Refresh();
        }
    
    }
}