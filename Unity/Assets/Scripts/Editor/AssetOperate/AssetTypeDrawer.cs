﻿﻿using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public static class AssetTypeDrawer
    {
        private static HashSet<int> _showLabels = new HashSet<int>();
        private static HashSet<int> _componentShow = new HashSet<int>();

        private static string GetSpace(int space)
        {
            return new string(Enumerable.Repeat(' ', space).ToArray());
        }
        
        public static void Draw(GameObject obj, int layer = 0 ,int index = 0)
        {
            var key = (int)Mathf.Pow(10, index) + layer;
            if (GUILayout.Button($"{GetSpace(index)}{(_showLabels.Contains(key) ? '▼' :'▶')}{obj.name}", "dragtab first"))
            {
                if (_showLabels.Contains(key))
                    _showLabels.Remove(key);
                else
                    _showLabels.Add(key);
            }

            
            if (_showLabels.Contains(key))
            {
                if (GUILayout.Button(
                    $"{GetSpace(index + 1)}{(_componentShow.Contains(key) ? '▼' : '▶')}组件",
                    "dragtab first"))
                {
                    if (_componentShow.Contains(key))
                        _componentShow.Remove(key);
                    else
                        _componentShow.Add(key);
                }

                if (_componentShow.Contains(key))
                {
                    var components = obj.GetComponents<Component>();
                    foreach (var component in components)
                    {
                        EditorGUILayout.LabelField($"{GetSpace(index + 7)}{component.GetType().Name}");
                    }
                }
                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    Draw(obj.transform.GetChild(i).gameObject, i, index + 1);
                }
            }
        }
    }
}