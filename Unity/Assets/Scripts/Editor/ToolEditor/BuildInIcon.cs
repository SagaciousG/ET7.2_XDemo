using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class BuildInIcon : EditorWindow
{

    [MenuItem(("Window/BuildInIcon"))]
    static void Init()
    {
        EditorWindow.GetWindow<BuildInIcon>("MyUnityTextureWindow");
    }

    Vector2 m_Scroll;
    List<Texture2D> _texture2Ds = new List<Texture2D>();
    private string _searchTxt;

    void Awake()
    {

        var flags = BindingFlags.Static | BindingFlags.NonPublic;
        var info = typeof(EditorGUIUtility).GetMethod("GetEditorAssetBundle", flags);
        var bundle = info.Invoke(null, new object[0]) as AssetBundle;
        UnityEngine.Object[] objs = bundle.LoadAllAssets();
        if (null != objs)
        {
            var names = new HashSet<string>();
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] is Texture2D icon)
                {
                    if (names.Contains(icon.name))
                        continue;
                    _texture2Ds.Add(icon);
                    names.Add(icon.name);
                }
            }
        }
        this._texture2Ds.Sort((t1, t2) => String.CompareOrdinal(t1.name, t2.name));
    }

    void OnGUI()
    {
        _searchTxt = EditorGUILayout.TextField(_searchTxt);
        m_Scroll = GUILayout.BeginScrollView(m_Scroll);
        float width = 50f;
        int count = Mathf.FloorToInt(position.width / width) - 1;

        var list = new List<Texture2D>();

        foreach (var texture2D in _texture2Ds)
        {
            if (string.IsNullOrEmpty(_searchTxt) || texture2D.name.ToLower().Contains(_searchTxt.ToLower()))
                list.Add(texture2D);
        }
        
        for (int i = 0; i < list.Count; i += count)
        {
            GUILayout.BeginHorizontal();
            for (int j = 0; j < count; j++)
            {
                int index = i + j;
                if (index < list.Count)
                {
                    if (GUILayout.Button(new GUIContent(list[index]), GUILayout.Width(width),
                        GUILayout.Height(30)))
                    {
                        GUIUtility.systemCopyBuffer = list[index].name;
                        ShowNotification(new GUIContent(list[index].name, list[index]));
                    }
                }
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }
}