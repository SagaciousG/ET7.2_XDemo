using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ET;
using UnityEditor;
using UnityEngine;
using ET;
using UnityEditorInternal;
//Object并非C#基础中的Object，而是 UnityEngine.Object
using Object = UnityEngine.Object;


//自定义ReferenceCollector类在界面中的显示与功能
[CustomEditor(typeof (UIReferenceCollector))]
//没有该属性的编辑器在选中多个物体时会提示“Multi-object editing not supported”
[CanEditMultipleObjects]
public class UIReferenceCollectorEditor: Editor
{
	private UIReferenceCollector referenceCollector;
	private static readonly Regex _regex = new Regex(@"^([a-zA-Z]+)(_?)(\d+)$");

	private void DelNullReference()
	{
		for (int i = this.referenceCollector.data.Count - 1; i >= 0; i--)
		{
			UnityEngine.Object o = this.referenceCollector.data[i];
			if (o == null)
			{
				this.referenceCollector.data.RemoveAt(i);
			}
		}
	}

	private void OnEnable()
	{
		//将被选中的gameobject所挂载的ReferenceCollector赋值给编辑器类中的ReferenceCollector，方便操作
		referenceCollector = (UIReferenceCollector) target;
	}

	public override void OnInspectorGUI()
	{
		//使ReferenceCollector支持撤销操作，还有Redo，不过没有在这里使用
		Undo.RecordObject(referenceCollector, "Changed Settings");
		//开始水平布局，如果是比较新版本学习U3D的，可能不知道这东西，这个是老GUI系统的知识，除了用在编辑器里，还可以用在生成的游戏中
		GUILayout.BeginHorizontal();
		//下面几个if都是点击按钮就会返回true调用里面的东西
		if (GUILayout.Button("自动引用"))
		{
			this.AutoFind();
		}
		if (GUILayout.Button("全部删除"))
		{
			referenceCollector.data.Clear();
		}
		if (GUILayout.Button("删除空引用"))
		{
			DelNullReference();
		}
		if (GUILayout.Button("排序"))
		{ 
			referenceCollector.data.Sort((x, y) => string.Compare(y.name, x.name, StringComparison.Ordinal));
			EditorUtility.SetDirty(this);
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(this.referenceCollector.gameObject);
		if (string.IsNullOrEmpty(path))
			GUI.enabled = false;
		if (GUILayout.Button("打开"))
		{
			if (string.IsNullOrEmpty(path))
				return;
			var cutPath = path.Substring("Assets/Bundles/UI/".Length);
			cutPath = cutPath.Replace($"/{referenceCollector.name}.prefab", "");
			var scriptPath = "Assets/Scripts/Codes/ModelView/UI";
			var systemPath = "Assets/Scripts/Codes/HotfixView/UI";

			var menu = new GenericMenu();
			menu.AddItem(new GUIContent("System"), false, () =>
			{
				InternalEditorUtility.OpenFileAtLineExternal(Path.GetFullPath($"{systemPath}/{cutPath}/{referenceCollector.name}ComponentSystemEx.cs"), 0);
			});
			menu.AddItem(new GUIContent("Component"), false, () =>
			{
				InternalEditorUtility.OpenFileAtLineExternal(Path.GetFullPath($"{scriptPath}/{cutPath}/{referenceCollector.name}ComponentEx.cs"), 0);
			});
			menu.ShowAsContext();
		}
		if (GUILayout.Button("生成脚本"))
		{
			GenerateScript(referenceCollector);	
		}
		if (GUILayout.Button("删除相关脚本"))
		{
			DeleteUIScripts(referenceCollector.gameObject);
		}
		GUI.enabled = true;

		if (GUILayout.Button("复制引用代码"))
		{
			var sb = new StringBuilder();
			sb.AppendLine("\t\t\t\tvar rc = self.GetParent<UI>().GameObject.GetComponent<UIReferenceCollector>();");
			foreach (var data in this.referenceCollector.data)
			{
				string fieldName = "";
				switch (data)
				{
					case GameObject go:
						fieldName = go.name;
						break;
					case Component com:
						fieldName = com.gameObject.name;
						break;
				}
				sb.AppendLine($"\t\t\t\tself.{fieldName} = rc.Get<{data.GetType().Name}>(\"{fieldName}\");");
			}
			TextEditor t = new TextEditor();
			t.text = sb.ToString();
			t.OnFocus();
			t.Copy();
		}
		
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.LabelField("拖入文件或对象到此处添加引用");
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		var toggleWidth = 30;
		var nameWidth = 100;
		var objRefWidth = 100;
		var removeWidth = 30;
		var style = new GUIStyle("AM HeaderStyle");
		style.alignment = TextAnchor.MiddleCenter;
		EditorGUILayout.BeginHorizontal();
		// EditorGUILayout.LabelField("Click", style, GUILayout.Width(toggleWidth));
		EditorGUILayout.LabelField("Field", style, GUILayout.Width(nameWidth));
		EditorGUILayout.LabelField("Ref", style, GUILayout.Width(objRefWidth));
		EditorGUILayout.LabelField("Component", style);
		EditorGUILayout.LabelField("", GUILayout.Width(removeWidth));
		EditorGUILayout.EndHorizontal();
		
		var delList = new List<int>();
		//遍历ReferenceCollector中data list的所有元素，显示在编辑器中
		for (int i = referenceCollector.data.Count - 1; i >= 0; i--)
		{
			var data = referenceCollector.data[i];
			if (data == null)
				continue;
			GUILayout.BeginHorizontal();
			GameObject obj = null;
			switch (data)
			{
				case GameObject go:
					obj = go;
					break;
				case Component com:
					obj = com.gameObject;
					break;
			}
			EditorGUILayout.TextField(obj.name, GUILayout.Width(nameWidth));
			EditorGUI.BeginChangeCheck();
			obj = (GameObject)EditorGUILayout.ObjectField(obj, typeof(GameObject), true, GUILayout.Width(objRefWidth));
			if (EditorGUI.EndChangeCheck())
			{
				delList.Add(i);
				AddReference(obj);
				GUILayout.EndHorizontal();
				return;
			}

			var componentNames = new List<string>();
			var componentTypes = new List<Type>();
			var all = obj.GetComponents<Component>();
			foreach (Component component in all)
			{
				componentNames.Add(component.GetType().Name);
				componentTypes.Add(component.GetType());
			}

			var index = componentTypes.IndexOf(data.GetType());
			var newIndex = EditorGUILayout.Popup(index, componentNames.ToArray());
			if (newIndex != index)
			{
				var type = componentTypes[newIndex];
				if (typeof (GameObject) == type)
				{
					referenceCollector.data[i] = obj;
				}
				else
				{
					referenceCollector.data[i] = obj.GetComponent(type);
				}
			}
			
			if (GUILayout.Button("X", GUILayout.Width(removeWidth)))
			{
				//将元素添加进删除list
				delList.Add(i);
			}
			GUILayout.EndHorizontal();
		}
		var eventType = Event.current.type;
		//在Inspector 窗口上创建区域，向区域拖拽资源对象，获取到拖拽到区域的对象
		if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
		{
			// Show a copy icon on the drag
			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

			if (eventType == EventType.DragPerform)
			{
				DragAndDrop.AcceptDrag();
				foreach (var o in DragAndDrop.objectReferences)
				{
					AddReference(o);
				}
			}

			Event.current.Use();
		}

		//遍历删除list，将其删除掉
		foreach (var i in delList)
		{
			referenceCollector.data.RemoveAt(i);
		}

		if (GUI.changed)
		{
			serializedObject.ApplyModifiedProperties();
			serializedObject.UpdateIfRequiredOrScript();
			EditorUtility.SetDirty(this.target);
		}
	}

	//添加元素，具体知识点在ReferenceCollector中说了
	private void AddReference(Object obj)
	{
		var componentNames = new List<string>();
		var componentTypes = new List<Type>();
		GameObject go = null;
		switch (obj)
		{
			case GameObject g:
				go = g;
				break;
			case Component c:
				go = c.gameObject;
				break;
		}

		var all = go.GetComponents<Component>();
		foreach (Component component in all)
		{
			componentNames.Add(component.GetType().Name);
			componentTypes.Add(component.GetType());
		}

		var index = GetBestChoice(componentNames.ToArray());
		var type = componentTypes[index];
		for (int i = 0; i < this.referenceCollector.data.Count; i++)
		{
			Object o = this.referenceCollector.data[i];
			if (o.name == go.name)
			{
				if (typeof (GameObject) == type)
				{
					this.referenceCollector.data[i] = go;
				}
				else
				{
					referenceCollector.data[i] = go.GetComponent(type);
				}
				return;
			}
		}
		if (typeof (GameObject) == type)
		{
			this.referenceCollector.data.Add(go);
		}
		else
		{
			referenceCollector.data.Add(go.GetComponent(type));
		}
	}

	private int GetBestChoice(string[] types)
	{
		var setting = AssetDatabase.LoadAssetAtPath<PSDSetting>("Assets/PsdSetting.asset");
		var dic = new List<string>();
		dic.AddRange(setting.ComponentTypes);
		var max = 0;
		var maxIndex = 0;
		for (int i = 0; i < types.Length; i++)
		{
			var index = dic.IndexOf(types[i]);
			if (index > max)
			{
				maxIndex = i;
				max = index;
			}
		}

		return maxIndex;
	}
	
	private void AutoFind()
	{
		DelNullReference();
		FindInChild(referenceCollector.transform);
	}

	private void FindInChild(Transform node)
	{
		var count = node.childCount;
		for (int i = 0; i < count; i++)
		{
			var child = node.GetChild(i);
			if (child.GetComponent<UIReferenceCollector>() != null)
			{
				continue;
			}
			if (Regex.IsMatch(child.name, @"^[a-z].*"))
			{
				AddReference(child.gameObject);
			}
			FindInChild(child);
		}
	}

	private static void GenerateScript(UIReferenceCollector referenceCollector)
	{
		var prefab = referenceCollector.gameObject;
		var assetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefab);

		if (string.IsNullOrEmpty(assetPath))
			return;
		var cutPath = assetPath.Substring("Assets/Bundles/UI/".Length);
		cutPath = cutPath.Replace($"/{referenceCollector.name}.prefab", "");
		var scriptPath = "Assets/Scripts/Codes/ModelView/UI";
		var systemPath = "Assets/Scripts/Codes/HotfixView/UI";

		var useSB = new StringBuilder();
		var fieldSB = new StringBuilder();
		var initSB = new StringBuilder();
		//----生成Component
		useSB.AppendLine("using UnityEngine.UI;");
		useSB.AppendLine("using UnityEngine;");
		useSB.AppendLine("using ET;");
		HashSet<string> _usedType = new HashSet<string>();
		foreach (var data in referenceCollector.data)
		{
			var nameSpace = data.GetType().Namespace;
			if (nameSpace == "UnityEngine"
			    || nameSpace == "UnityEngine.UI"
			    || nameSpace == "ET"
			    )
				continue;
			if (_usedType.Contains(nameSpace))
				continue;
			useSB.AppendLine($"using {nameSpace};");
			_usedType.Add(nameSpace);
		}

		MultiMap<string, Object> comList = new();
		foreach (var data in referenceCollector.data)
		{
			string fieldName = "";
			switch (data)
			{
				case GameObject go:
					fieldName = go.name;
					break;
				case Component com:
					fieldName = com.gameObject.name;
					break;
			}
			fieldSB.AppendLine($"\t\tpublic {data.GetType().Name} {fieldName};");
			if (_regex.IsMatch(fieldName))
			{
				var res = _regex.Match(fieldName);
				var fn = res.Groups[1].Value;
				var num = res.Groups[3].Value;
				comList.Add(fn, data);
			}
		}
		
		foreach (var data in referenceCollector.data)
		{
			switch (data)
			{
				case XSubUI subUI:
				{
					fieldSB.AppendLine($"\t\tpublic UI {subUI.name}UI;");
					break;
				}
				case XUITab xuiTab:
				{
					fieldSB.AppendLine($"\t\tpublic UI {xuiTab.name}UI;");
					break;
				}
			}
		}

		if (comList.Count > 0)
			useSB.AppendLine($"using System.Collections.Generic;");
		foreach (var kv in comList)
		{
			var data = kv.Value.First();
			fieldSB.AppendLine($"\t\tpublic Dictionary<int, {data.GetType().Name}> {kv.Key} = new();");
			switch (data)
			{
				case XSubUI subUI:
					fieldSB.AppendLine($"\t\tpublic Dictionary<int, UI> {kv.Key}UI = new();");
					break;
			}
		}
		
		var componentTemp = File.ReadAllText("Assets/Scripts/Editor/ReferenceCollectorEditor/ComponentTemplete.txt");
		componentTemp = componentTemp.Replace("[USING]", useSB.ToString());
		componentTemp = componentTemp.Replace("[NAME]", referenceCollector.name);
		componentTemp = componentTemp.Replace("[FIELDS]", fieldSB.ToString());
		
		if (!Directory.Exists($"{scriptPath}Gen/{cutPath}"))
			Directory.CreateDirectory($"{scriptPath}Gen/{cutPath}");
		File.WriteAllText($"{scriptPath}Gen/{cutPath}/{referenceCollector.name}Component.cs", componentTemp);

		if (!File.Exists($"{scriptPath}/{cutPath}/{referenceCollector.name}ComponentEx.cs"))
		{
			var componentEx = File.ReadAllText("Assets/Scripts/Editor/ReferenceCollectorEditor/ComponentEx.txt");
			componentEx = componentEx.Replace("[NAME]", referenceCollector.name);
			if (!Directory.Exists($"{scriptPath}/{cutPath}"))
				Directory.CreateDirectory($"{scriptPath}/{cutPath}");
			File.WriteAllText($"{scriptPath}/{cutPath}/{referenceCollector.name}ComponentEx.cs", componentEx);
		}

		
		//System---------------
		if (!File.Exists($"{systemPath}/{cutPath}/{referenceCollector.name}ComponentSystemEx.cs"))
		{
			var systemTempEx = File.ReadAllText("Assets/Scripts/Editor/ReferenceCollectorEditor/SystemExTemplete.txt");
			systemTempEx = systemTempEx.Replace("[NAME]", referenceCollector.name);
			if (!Directory.Exists($"{systemPath}/{cutPath}"))
				Directory.CreateDirectory($"{systemPath}/{cutPath}");
			File.WriteAllText($"{systemPath}/{cutPath}/{referenceCollector.name}ComponentSystemEx.cs", systemTempEx);
		}
		
		//Event---------------
		foreach (var data in referenceCollector.data)
		{
			string fieldName = "";
			switch (data)
			{
				case GameObject go:
					fieldName = go.name;
					break;
				case Component com:
					fieldName = com.gameObject.name;
					break;
			}
			initSB.AppendLine($"\t\t\tself.{fieldName} = rc.Get<{data.GetType().Name}>(\"{fieldName}\");");
		}
		
		foreach (var kv in comList)
		{
			foreach (Object o in kv.Value)
			{
				string fieldName = "";
				switch (o)
				{
					case GameObject go:
						fieldName = go.name;
						break;
					case Component com:
						fieldName = com.gameObject.name;
						break;
				}
				var res = _regex.Match(fieldName);
				var fn = res.Groups[1].Value;
				var num = res.Groups[3].Value;
				initSB.AppendLine($"\t\t\tself.{kv.Key}.Add({num}, self.{fieldName});");
			}
		}

		var subAwake = new StringBuilder();
		foreach (var data in referenceCollector.data)
		{
			switch (data)
			{
				case XSubUI subUI:
				{
					var n = subUI.gameObject.name;
					subAwake.AppendLine($"\t\t\tself.{n}UI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.{n}.uiPrefab.name, self.{n}.transform);");
					break;
				}
				case UIList uiList:
				{
					subAwake.AppendLine($"\t\t\tself.GetParent<UI>().AddChild<ETUIList, UIList>(self.{uiList.name});");
					break;
				}
				case XUITab xuiTab:
				{
					var n = xuiTab.gameObject.name;
					subAwake.AppendLine($"\t\t\tself.{n}.OnCreateInstance += async (a) =>");
					subAwake.AppendLine("\t\t\t{");
					subAwake.AppendLine($"\t\t\t\tself.{n}UI = await UIHelper.BindSingleUI(self.GetParent<UI>(), self.{n}.uiPrefab.name, a);");
					subAwake.AppendLine($"\t\t\t\tself.{n}UI.SetData();");
					subAwake.AppendLine("\t\t\t};");
					break;
				}
			}
		}

		foreach (var kv in comList)
		{
			foreach (var o in kv.Value)
			{
				switch (o)
				{
					case XSubUI subUI:
					{
						var n = subUI.gameObject.name;
						var res = _regex.Match(n);
						var num = res.Groups[3].Value;
						subAwake.AppendLine($"\t\t\tself.{kv.Key}UI.Add({num}, self.{n}UI);");
						break;
					}
				}
			}
		}
		
		var eventTemp = File.ReadAllText("Assets/Scripts/Editor/ReferenceCollectorEditor/EventTemplete.txt");
		eventTemp = eventTemp.Replace("[USING]", useSB.ToString());
		eventTemp = eventTemp.Replace("[INIT]", initSB.ToString());
		eventTemp = eventTemp.Replace("[SUB_AWAKE]", subAwake.ToString());
		eventTemp = eventTemp.Replace("[NAME]", referenceCollector.name);
		if (!Directory.Exists($"{systemPath}Gen/{cutPath}"))
			Directory.CreateDirectory($"{systemPath}Gen/{cutPath}");
		File.WriteAllText($"{systemPath}Gen/{cutPath}/{referenceCollector.name}Event.cs", eventTemp);


		var uiType = "Assets/Scripts/Codes/ModelView/Module/UI/UIType.cs";
		var fields = new StringBuilder();
		fields.AppendLine("namespace ET.Client");
		fields.AppendLine("{");
		fields.AppendLine("\tpublic static class UIType");
		fields.AppendLine("\t{");
		var existKey = new HashSet<string>();
		if (File.Exists(uiType))
		{
			var regex = new Regex(@"public const string (\w*) = .*;");
			var lines = File.ReadAllLines(uiType);
			foreach (string line in lines)
			{
				if (regex.IsMatch(line))
				{
					var res = regex.Match(line);
					existKey.Add(res.Groups[1].Value);
					fields.AppendLine(line);
				}
			}
		}

		if (!existKey.Contains(referenceCollector.name))
		{
			fields.AppendLine($"\t\tpublic const string {referenceCollector.name} = \"{referenceCollector.name}\";");
			
		}

		fields.AppendLine("\t}");
		fields.AppendLine("}");
		File.WriteAllText(uiType, fields.ToString());
		
		
		AssetDatabase.Refresh();
	}

	private static void DeleteUIScripts(GameObject prefab)
	{
		var assetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefab);

		if (string.IsNullOrEmpty(assetPath))
			return;
		var cutPath = assetPath.Substring("Assets/Bundles/UI/".Length);
		cutPath = cutPath.Replace($"/{prefab.name}.prefab", "");
		var scriptPath = "Assets/Scripts/Codes/ModelView/UI";
		var systemPath = "Assets/Scripts/Codes/HotfixView/UI";

		var paths = new List<string>()
		{
			$"{scriptPath}Gen/{cutPath}/{prefab.name}Component.cs",
			$"{scriptPath}/{cutPath}/{prefab.name}ComponentEx.cs",
			$"{systemPath}/{cutPath}/{prefab.name}ComponentSystemEx.cs",
			$"{systemPath}Gen/{cutPath}/{prefab.name}Event.cs",
		};
		foreach (var path in paths)
		{
			if (File.Exists(path))
				File.Delete(path);
		}
		var uiType = "Assets/Scripts/Codes/ModelView/Module/UI/UIType.cs";
		if (File.Exists(uiType))
		{
			var regex = new Regex(@"public const string (\w*) = .*;");
			var lines = File.ReadAllLines(uiType);
			var fields = new StringBuilder();
			foreach (var line in lines)
			{
				if (regex.IsMatch(line))
				{
					var res = regex.Match(line);
					if (res.Groups[1].Value == prefab.name)
					{
						continue;
					}
				}
				fields.AppendLine(line);
			}
			File.WriteAllText(uiType, fields.ToString());
		}
		AssetDatabase.Refresh();
	}


	[MenuItem("Build/GenerateAllUI")]
	public static void RegenerateAllUI()
	{
		var allFiles = Directory.GetFiles("Assets/Bundles/UI", "*.prefab", SearchOption.AllDirectories);
		foreach (var file in allFiles)
		{
			var ui = AssetDatabase.LoadAssetAtPath<GameObject>(file);
			var rc = ui.GetComponent<UIReferenceCollector>();
			GenerateScript(rc);
		}
	}
}
