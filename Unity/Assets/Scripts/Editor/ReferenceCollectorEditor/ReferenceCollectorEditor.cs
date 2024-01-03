using System;
using System.Collections.Generic;
using ET;
using UnityEditor;
using UnityEngine;
//Object并非C#基础中的Object，而是 UnityEngine.Object
using Object = UnityEngine.Object;

//自定义ReferenceCollector类在界面中的显示与功能
[CustomEditor(typeof (ReferenceCollector))]
public class ReferenceCollectorEditor: Editor
{
	private ReferenceCollector referenceCollector;

	private Object heroPrefab;
	private string _groupName;

	private GUIElementState _guiState;

	private void OnEnable()
	{
        referenceCollector = (ReferenceCollector) target;
        _guiState = new GUIElementState();
	}

	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.BeginHorizontal();
		{
			_groupName = EditorGUILayout.TextField("AddGroup", _groupName);
			if (GUILayout.Button("+"))
			{
				referenceCollector.data.Add(new ReferenceCollectorGroup(){key = _groupName});
			}
		}
		EditorGUILayout.EndHorizontal();
		
		for (var i = 0; i < referenceCollector.data.Count; i++)
		{
			var group = referenceCollector.data[i];

			var fade = _guiState.GetAnimBool(group.GetHashCode());
			EditorGUILayout.BeginHorizontal();
			fade.value = EditorGUILayout.Foldout(fade.value, group.key);
			if (GUILayout.Button("X", GUILayout.Width(30)))
			{
				referenceCollector.data.RemoveAt(i);
				break;
			}
			EditorGUILayout.EndHorizontal();
			if (fade.value)
			{
				EditorGUILayout.BeginVertical("GroupBox");
				{
					group.key = EditorGUILayout.TextField("groupName", group.key);
					var drags = DragAreaGetObject.GetObject();
					if (drags != null)
					{
						foreach (var drag in drags)
						{
							referenceCollector.Add(group.key, drag);
						}
					}

					for (var j = 0; j < group.Datas.Count; j++)
					{
						var data = group.Datas[j];
						EditorGUILayout.BeginHorizontal();
						GUILayout.TextField(data.key, GUILayout.Width(80));
						EditorGUI.EndChangeCheck();
						data.gameObject = EditorGUILayout.ObjectField(data.gameObject, typeof(Object), false);
						if (EditorGUI.EndChangeCheck())
						{
							data.key = data.gameObject.name;
						}

						if (GUILayout.Button("X", GUILayout.Width(30)))
						{
							group.Datas.RemoveAt(j);
						}

						EditorGUILayout.EndHorizontal();
					}
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndFadeGroup();
		}

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}
	}
}
