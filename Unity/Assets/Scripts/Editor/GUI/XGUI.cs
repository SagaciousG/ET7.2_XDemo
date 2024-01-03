using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public partial class XGUI 
    {
        public static Transform Canvas
        {
            get
            {
                var canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
                if (canvas == null)
                {
                    EditorApplication.ExecuteMenuItem("GameObject/UI/Canvas");
                    canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
                }

                return canvas.transform;
            }
        }

        private static void Create(MenuCommand command, string name)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/UIElement/{name}.prefab");
            var context = command.context as GameObject;
            var ui = UnityEngine.Object.Instantiate(prefab, context != null ? context.transform : Canvas, false);
            ui.name = name;
            Undo.RegisterCreatedObjectUndo(ui, $"Assets/UIElement/{name}.prefab");
            Selection.activeObject = ui;
        }
        
        [MenuItem("GameObject/XGUI/RefreshAssets", priority = 0)]
        public static void Refresh()
        {
            var list = FileHelper.GetAllFiles("Assets/UIElement", "*.prefab");
            var sb = new StringBuilder();
            sb.AppendLine("using UnityEditor;\n");
            sb.AppendLine("namespace ET\n{");
            sb.AppendLine("\tpublic partial class XGUI\n\t{");
            foreach (string s in list)
            {
                var n = Path.GetFileNameWithoutExtension(s);
                sb.AppendLine($"\t\t[MenuItem(\"GameObject/XGUI/{n}\", priority = 0)]");
                sb.AppendLine($"\t\tpublic static void {n.Replace(" ", "")}(MenuCommand command)");
                sb.AppendLine("\t\t{");
                sb.AppendLine($"\t\t\tCreate(command, \"{n}\");");
                sb.AppendLine("\t\t}");
            }

            sb.AppendLine("\t}\n}");
            FileHelper.WriteAllText("Assets/Scripts/Editor/GUI/XGUIEx.cs", sb.ToString());
            AssetDatabase.Refresh();
        }
    }
}
