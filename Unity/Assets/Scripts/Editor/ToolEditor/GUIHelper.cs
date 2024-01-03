using System;
using System.Reflection;
using ET;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public static class GUIHelper
    {
        private static string _focusKey;
        private static Rect _focusArea;
        private static Rect _downArea;
        public static void Box(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.Box(rect, "", "WhiteBackground");
            GUI.color = Color.white;
        }

        public static bool Button(Rect rect, string txt, GUIStyle style)
        {
            return Button(rect, new GUIContent(txt), style);
        }
        public static bool Button(Rect rect, GUIContent content, GUIStyle style)
        {
            GUILayoutUtility.GetRect(rect.width, rect.height);
            GUI.Box(rect, content, style);
            return Click(rect);
        }
        

        public static TextAnchor TextAlignGUI(TextAnchor align)
        {
            return align;
        }

        public static bool Click(Rect area, bool use = true)
        {
            if (Event.current.type == UnityEngine.EventType.Used)
                return false;

            if (Event.current.type == UnityEngine.EventType.MouseDown)
            {
                if (area.Contains(Event.current.mousePosition))
                    _downArea = area;
            }
            if (Event.current.type == UnityEngine.EventType.MouseUp)
            {
                if (area.Contains(Event.current.mousePosition))
                {
                    if (use)
                        Event.current.Use();
                    return _downArea == area;
                }
            }

            return false;
        }
        
        public static bool DoubleClick(Rect area)
        {
            if (Event.current.type == UnityEngine.EventType.Used)
                return false;
            if (Event.current.type == UnityEngine.EventType.MouseDown)
            {
                if (Event.current.button == 0 && area.Contains(Event.current.mousePosition) && Event.current.clickCount >= 2)
                {
                    Event.current.Use();
                    return true;
                }
            }

            return false;
        }

        public static string EditLable(Rect area, string text, string key)
        {
            if (key == _focusKey)
            {
                text = EditorGUI.TextField(area, text);
                if (Event.current.type == UnityEngine.EventType.KeyDown && Event.current.keyCode == KeyCode.KeypadEnter)
                {
                    _focusKey = "";
                }
            }
            else
            {
                EditorGUI.LabelField(area, text);
                if (GUIHelper.DoubleClick(area))
                {
                    _focusKey = key;
                    _focusArea = area;
                    
                    GUI.SetNextControlName(key);
                    text = EditorGUI.TextField(area, text);
                    GUI.FocusControl(key);
                }
            }
            
            if (Event.current.type == UnityEngine.EventType.MouseDown)
            {
                if (!_focusArea.Contains(Event.current.mousePosition))
                    _focusKey = "";
            }

            return text;
        }

        public static GUIContent GetName(this Type type)
        {
            var res = new GUIContent();
            var nameAttribute = type.GetCustomAttribute<NameAttribute>();
            if (nameAttribute != null)
            {
                res.text = nameAttribute.Name;
                res.tooltip = nameAttribute.Tooltips;
            }
            else
            {
                res.text = type.Name;
            }

            return res;
        }
        
        public static NameAttribute GetNameAttribute(FieldInfo field)
        {
            return field.GetCustomAttribute<NameAttribute>();
        }

        public static void PopTips(GUIContent content)
        {
            if (string.IsNullOrEmpty(content.tooltip))
                return;
           
            var pos = Event.current.mousePosition;
            EditorPopTips.ShowWin(content,  pos);
        }
    }

    public class EditorPopTips: PopupWindowContent
    {
        private static EditorPopTips _popUp;
        
        public static void ShowWin(GUIContent content, Vector2 pos)
        {
            var win = new EditorPopTips();
            win.content = content;
            PopupWindow.Show(new Rect(pos - new Vector2(0, 200), win.GetWindowSize()), win);
        }

        private GUIContent content;

        public override Vector2 GetWindowSize()
        {
            var h = Mathf.CeilToInt((this.content.tooltip?.Length ?? 0) * 18f / 200);
            var size = new Vector2(h == 1? (this.content.tooltip?.Length ?? 0) * 18 : 200, h * 18);
            size = Vector2.Max(new Vector2(200, 200), size);
            return size;
        }

        public override void OnGUI(Rect rect)
        {
            var skin = GUI.skin.label;
            skin.wordWrap = true;
            skin.richText = true;
            skin.alignment = TextAnchor.UpperLeft;
            EditorGUILayout.LabelField(this.content.tooltip, skin, GUILayout.Height(this.editorWindow.position.height));
        }
    }
}